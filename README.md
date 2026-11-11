# D3 Hooks: the control plus a generic hook script that pushes C4's rules when they matter

The control plugin (`control-eecee918`) plus `plugins/maxio/hooks/`. `git diff control-eecee918 d3-hooks` is exactly
the treatment; the skills are unchanged and there is no pointer line:

- `hooks/hooks.json`, which Claude Code loads from a plugin's `hooks/` folder:
  - **PostToolUse on Edit|Write|MultiEdit**: when edited code (a `.cs`, `.ts`, `.py`… file, not a plan) calls an
    operation, by its SDK method (`.CreateSubscription(`) or its route in raw HTTP code, the rules of that operation,
    loud first, once per operation per session;
  - **PostToolUse on Bash|PowerShell**, and **PostToolUseFailure** on them: when the output carries a documented
    Maxio error (its message, or its status with the operation), what it means and what to do;
  - **Stop**: once per session, a check listing the rules of each operation the code called; the agent confirms
    each or fixes the code, then stops.
- `hooks/provider-rules.mjs`: the one script behind all four, generic for any API (Node; it never fails a tool call,
  and logs each firing to `<tmp>/provider-rules-hooks/<session>.log`).
- `hooks/data/`: `rules.json` (C4's 1,237 records), `operations.json` (285 operations: SDK call, HTTP method, route
  and a route pattern), `errors.json` (191 entries, one per documented status and message, with the rules that name
  it).

The branch's second commit is this rebuild: the same records and script, with no double period after a Maxio outcome that ends in one. The first commit is superseded.

## How it was built

From plugin-experiments at `6e21c1046` (branch `research/br-plugin-screen-build`):

```
python -I test-suite/scripts/br-plugin/build-d3-hooks.py --rules test-suite/content/br-plugin/maxio/c4/rules.json
python -I test-suite/scripts/br-plugin/assemble-branch.py --arms <this repo> --branch d3-hooks \
    --plugin test-suite/content/br-plugin/maxio/d3-hooks --readme <this README> --description "<...>"
```

The record is `test-suite/content/br-plugin/maxio/d3-hooks.json`. C4's extraction is described in the
`c4-rule-catalogue` branch README.

## Pins

| Pin | Value |
|---|---|
| Control | `control-eecee918`, plugin tree sha256 `a4ed19b537118186bca4c07bde18cb1d2c43f0777975a6a8335691eb0fb00ce5` |
| Rules | `c4/rules.json`, sha256 `54cbdadd23c0cf6c57e1a686f6cb73c6325b4dd62e07db5e2bd68c6b56569214` |
| Script | `d3/provider-rules.mjs`, sha256 `62e16cb3c179f95a8881e49272f27fa9e8da3d08abb0315aa3c03cb7501f69e9` |
| Plugin | `plugins/maxio`: tree sha256 `2866cec2ccfd456b66ef4a59826a1423fd936cb986d6dc9387e3dd09b0c092c9`, 1,571 files |
| SDK | `sdk/dotnet` without its docs: the SDK pin, `e8cebb1c…3673` |

## Checks

`test-suite/scripts/br-plugin/check-branch.py` on a clone holding only this branch's commit: only adds, same SDK,
`claude plugin validate --strict` passes on Claude Code 2.1.319, installs and is enabled in an empty
`CLAUDE_CONFIG_DIR`. Each hook mode was run against made-up input, and `d3/probe_hooks.py` ran one real headless session on 2.1.319
(Haiku 5.5, allowed only to write files and run node, $0.0065) with the plugin installed from the first commit
(`fabc8a1b`; the rebuild changes only punctuation in rule lines): the post-edit, command-failed and stop hooks all
fired, and the agent answered the stop check. Whether they run beside the arm's own `app-repo-guard` PreToolUse hook is
checked in the arm's dry run.

## Approval

Pending: a second person approves this branch's diff against the control before it is pinned in an arm (RULES 4).
