// provider-rules.mjs: a plugin's provider rules, pushed to the agent at the moment they matter (Claude Code hooks).
// Generic: everything API-specific is in ./data/ (rules.json, operations.json, errors.json), written by
// plugin-experiments test-suite/scripts/br-plugin/build-d3-hooks.py from a rule catalogue and the API's spec.
//
//   node provider-rules.mjs post-edit        PostToolUse on Edit|Write|MultiEdit: the rules of each operation the
//                                            edited code calls, loud rules first, once per operation per session
//   node provider-rules.mjs after-command    PostToolUse on Bash|PowerShell: what a provider error in the output means
//   node provider-rules.mjs command-failed   PostToolUseFailure on Bash|PowerShell: the same, for a failed command
//   node provider-rules.mjs stop             Stop: once per session, a check of each called operation's rules
//
// It reads the hook's JSON on stdin and prints the hook's JSON answer, or nothing. It never fails a tool call: any
// error of its own ends it silently. Each firing is logged to <tmp>/provider-rules-hooks/<session>.log.
import fs from "node:fs";
import os from "node:os";
import path from "node:path";
import { fileURLToPath } from "node:url";

const HERE = path.dirname(fileURLToPath(import.meta.url));
const DATA = path.join(HERE, "data");
const CODE = /\.(cs|cshtml|razor|fs|vb|ts|tsx|js|jsx|mjs|cjs|py|java|kt|go|rb|php|swift|rs)$/i;
const MAX = 12000;

const load = (f) => JSON.parse(fs.readFileSync(path.join(DATA, f), "utf8"));
const readStdin = () => { try { return JSON.parse(fs.readFileSync(0, "utf8") || "{}"); } catch { return {}; } };

function stateFile(session) {
  const dir = path.join(os.tmpdir(), "provider-rules-hooks");
  fs.mkdirSync(dir, { recursive: true });
  return { dir, file: path.join(dir, `${String(session || "nosession").replace(/[^\w.-]/g, "_")}.json`) };
}
function loadState(session) {
  const { file } = stateFile(session);
  try { return JSON.parse(fs.readFileSync(file, "utf8")); } catch { return { reported: [], called: [], errors: [], stopChecked: false }; }
}
function saveState(session, s) { fs.writeFileSync(stateFile(session).file, JSON.stringify(s)); }
function log(session, entry) {
  const { dir } = stateFile(session);
  const line = JSON.stringify({ utc: new Date().toISOString(), ...entry }) + "\n";
  fs.appendFileSync(path.join(dir, `${String(session || "nosession").replace(/[^\w.-]/g, "_")}.log`), line);
}

function outcome(r) {
  const po = r.provider_outcome || {};
  const parts = [];
  if (po.status) parts.push(String(po.status));
  if (po.error) parts.push(`"${po.error}"`);
  if (po.effect) parts.push(po.effect);
  return parts.length ? parts.join(" ") : "outcome not stated";
}
const ruleLine = (r) => `- ${r.loudness}, ${r.on}/${r.does}: ${String(r.condition).replace(/\.$/, "")} -> ${outcome(r).replace(/\.$/, "")}. Do: ${r.app_must} (${r.id})`;
const byLoud = (a, b) => (a.loudness === "loud" ? 0 : 1) - (b.loudness === "loud" ? 0 : 1) || a.id.localeCompare(b.id);

function rulesFor(rules, opId) {
  return rules.filter((r) => r.scope !== "every-request" && r.operations.includes(opId)).sort(byLoud);
}

function opsCalled(text, ops) {
  const found = [];
  for (const op of ops) {
    const sdk = new RegExp(`\\.\\s*${op.method}\\s*\\(`);
    let hit = sdk.test(text);
    if (!hit && op.pathRegex) {
      const re = new RegExp(op.pathRegex, "i");
      const m = re.exec(text);
      if (m) {
        const lineText = text.slice(text.lastIndexOf("\n", m.index) + 1, text.indexOf("\n", m.index) >>> 0 || undefined);
        const verbs = lineText.match(/\b(Get|Post|Put|Patch|Delete)\b/gi);
        hit = !verbs || verbs.some((v) => v.toUpperCase() === op.http);
      }
    }
    if (hit) found.push(op);
  }
  return found;
}

function cap(text) { return text.length > MAX ? text.slice(0, MAX) + "\n(... cut at the hook's size limit)" : text; }

function postEdit(input, rules, ops, api) {
  const ti = input.tool_input || {};
  const file = ti.file_path || ti.notebook_path || "";
  if (!CODE.test(file)) return null;
  const text = [ti.content, ti.new_string, ...(Array.isArray(ti.edits) ? ti.edits.map((e) => e.new_string) : [])]
    .filter(Boolean).join("\n");
  if (!text) return null;
  const s = loadState(input.session_id);
  const called = opsCalled(text, ops);
  for (const op of called) if (!s.called.includes(op.operationId)) s.called.push(op.operationId);
  const fresh = called.filter((op) => !s.reported.includes(op.operationId) && rulesFor(rules, op.operationId).length);
  if (!fresh.length) { saveState(input.session_id, s); return null; }
  const parts = [`${api} rules for the operations this edit to ${path.basename(file)} calls (loud rules first; a loud rule makes ${api} refuse the request, a silent one gives a result with no error):`];
  for (const op of fresh) {
    s.reported.push(op.operationId);
    parts.push(`\n${op.call} (${op.http} ${op.path}):`, ...rulesFor(rules, op.operationId).map(ruleLine));
  }
  saveState(input.session_id, s);
  log(input.session_id, { event: "post-edit", file, operations: fresh.map((o) => o.operationId) });
  return { hookSpecificOutput: { hookEventName: "PostToolUse", additionalContext: cap(parts.join("\n")) } };
}

function errorsIn(text, errors, ops, s) {
  const low = text.toLowerCase();
  const hits = [];
  for (const e of errors) {
    if (s.errors.includes(e.key)) continue;
    const msg = e.error && e.error.length >= 6 && low.includes(e.error.toLowerCase());
    const statusAndOp = e.status && new RegExp(`\\b${e.status}\\b`).test(text) &&
      e.operations.some((id) => { const op = ops.find((o) => o.operationId === id); return op && new RegExp(`\\b${op.method}\\b`).test(text); });
    if (msg || statusAndOp) hits.push(e);
  }
  return hits;
}

function afterCommand(input, rules, ops, errors, api, eventName) {
  const raw = eventName === "PostToolUseFailure" ? String(input.error || "") : JSON.stringify(input.tool_response || "");
  if (!/\b[45]\d\d\b/.test(raw) && !errors.some((e) => e.error && raw.toLowerCase().includes(e.error.toLowerCase()))) return null;
  const s = loadState(input.session_id);
  const hits = errorsIn(raw, errors, ops, s);
  if (!hits.length) return null;
  const byId = new Map(rules.map((r) => [r.id, r]));
  const parts = [`What the ${api} error(s) in this output mean, from ${api}'s own spec and docs:`];
  for (const e of hits) {
    s.errors.push(e.key);
    parts.push(`\n${e.status || ""} ${e.error ? `"${e.error}"` : ""}`.trim() + ":", ...e.rules.map((id) => byId.get(id)).filter(Boolean).map(ruleLine));
  }
  saveState(input.session_id, s);
  log(input.session_id, { event: eventName, errors: hits.map((e) => e.key) });
  return { hookSpecificOutput: { hookEventName: eventName, additionalContext: cap(parts.join("\n")) } };
}

function stop(input, rules, ops, api) {
  if (input.stop_hook_active) return null;
  const s = loadState(input.session_id);
  if (s.stopChecked || !s.called.length) return null;
  const parts = [`Before you finish: check the code against ${api}'s rules for each operation it calls. For each rule below, confirm the code honours it, or fix the code; then finish.`];
  for (const id of s.called) {
    const op = ops.find((o) => o.operationId === id);
    const rs = rulesFor(rules, id);
    if (op && rs.length) parts.push(`\n${op.call} (${op.http} ${op.path}):`, ...rs.map(ruleLine));
  }
  s.stopChecked = true;
  saveState(input.session_id, s);
  if (parts.length === 1) return null;
  log(input.session_id, { event: "stop", operations: s.called });
  return { decision: "block", reason: cap(parts.join("\n")) };
}

try {
  const mode = process.argv[2];
  const input = readStdin();
  const meta = load("operations.json");
  const rules = load("rules.json");
  const ops = meta.operations;
  let answer = null;
  if (mode === "post-edit") answer = postEdit(input, rules, ops, meta.api);
  else if (mode === "after-command") answer = afterCommand(input, rules, ops, load("errors.json"), meta.api, "PostToolUse");
  else if (mode === "command-failed") answer = afterCommand(input, rules, ops, load("errors.json"), meta.api, "PostToolUseFailure");
  else if (mode === "stop") answer = stop(input, rules, ops, meta.api);
  if (answer) process.stdout.write(JSON.stringify(answer));
} catch {
  // A hook of this plugin never blocks or fails a tool call.
}
process.exit(0);
