# Treatment for the codegen-v2 PR #811 pilot: the Maxio plugin the PR generates at 140ea4af, with Maxio's rules/

`plugins/maxio/` is exactly what the generator wrote, byte for byte, unedited (RULES 5): codegen-v2
[PR #811](https://github.com/apimatic/codegen-v2/pull/811) (`feat/plugin-business-rules`) at its head `140ea4af`,
from a build holding Maxio's `rules/`. It is the treatment of the business-rules pilot (plugin-experiments
`research/business-rules/baseline/plan.md`). It starts from the control's commit, so `git diff control-eed5f7cc
pr811-rules-140ea4af` is exactly the treatment:

- `business-rules/` at the plugin root (beside `skills/`): 45 Markdown files, byte for byte the build's `rules/`
  ([MuHamza30/maxio-br-plugin-src](https://github.com/MuHamza30/maxio-br-plugin-src) `src/rules` at `f3c3ec5a`, C4's
  1,237 rules): `rules.md`, `every-request.md`, `background.md` and 42 files under `operations/`.
- One added step, number 5 of *How to ground* in `dotnet-integrate-maxio-advanced-billing`, the instructions the
  `maxio-advanced-billing-planner` agent follows while it writes the plan:

  > 5. **Consult `<plugin-root>/business-rules/` for business knowledge the map does not carry** (the plugin root is
  >    the directory holding `skills/`): every business rule Maxio Advanced Billing's own spec and docs state, as
  >    Markdown. Open `rules.md`, its index; read `every-request.md`, `background.md` where the scope relies on
  >    something Maxio Advanced Billing does on its own (renewals, retries, webhooks), and, under `operations/`, the
  >    section for every operation in scope, headed with its route. Plan for each rule that applies to the scope where
  >    it changes the plan (a step in §1, the fields a request must carry, or a Blocker in §6 where the plan makes a
  >    call Maxio Advanced Billing would refuse) and name the rule's id there. The rules are the source for how Maxio
  >    Advanced Billing behaves; the map stays the source for SDK contracts.

Nothing else changes: no file is removed or edited apart from that one added line.

Generated at the same commit without `rules/`, the plugin is byte-identical to `control-eed5f7cc` (tree
`96a94ef3…`, 1,563 files), as the PR states.

## How it was built

From plugin-experiments at `932ab8ef5` (branch `research/br-plugin-screen-build`), with a clean codegen-v2 checkout
at `140ea4af` and a clean clone of `maxio-br-plugin-src` at `f3c3ec5a`:

```
python -I test-suite/scripts/br-plugin/generate-br-plugin.py --codegen <codegen-v2 checkout> \
    --commit 140ea4af5b7c7982f3b8f9de413ccc3788a5ff14 --unpinned --twice \
    --rules <maxio-br-plugin-src>/src/rules --out test-suite/content/br-plugin/maxio/pr811-rules-140ea4af
python -I test-suite/scripts/br-plugin/assemble-branch.py --arms <this repo> --branch pr811-rules-140ea4af \
    --base control-eed5f7cc --plugin test-suite/content/br-plugin/maxio/pr811-rules-140ea4af \
    --readme <this README> --description "<...>"
```

`--rules` puts the folder into the build as `rules/`, byte for byte, as the CLI uploads `src/rules/`. At this commit
the GenerateV4Skills and AggregatePlugin commands take a `BuildFileName`, which the PR's orchestrators fill through
`Command.From`; so the script's copy of the harness passes `Workspace.GetGlobalBuildFileName()` in both (the one
change to the harness the pilot plan names). The record is
`test-suite/content/br-plugin/maxio/pr811-rules-140ea4af.json`.

## Pins

| Pin | Value |
|---|---|
| codegen-v2 | `140ea4af5b7c7982f3b8f9de413ccc3788a5ff14` (PR #811 head; based on `main` `eed5f7cc`) |
| Control | `control-eed5f7cc`, plugin tree sha256 `96a94ef3a0fc1f52f8846c4b7527adb83509bcc011f9ab2e900b520a2bb4de32` |
| Spec | plugin-experiments `research/docs/maxio/src/spec`, tree sha256 `619868198e3f21002ef62549d982607f2f4c33b5ffdabeb9a00bb368e1484bdc` (as the control) |
| Rules | `maxio-br-plugin-src` `src/rules` at `f3c3ec5aa5a19f34bb638338b59ffcf25fe9bce4`: tree sha256 `84e3b2406c79e3e62c291723b5c16ec95420b491d590de070265f7f58cd567d0` over 45 files |
| Plugin | `plugins/maxio`: tree sha256 `a8a64fea83435fcdf9c5cc6c495760e7462dab5ea94f09f5856f033f9c662619` over 1,608 files |
| SDK | `sdk/dotnet` without its docs: `5769460e…f809`, the same as the control |
| Harness | the copy that passes the build file name: sha256 `7896019b3eafbab49725ff029fbb1173045140aeac83318897519c4c9e2e7a38` |

## Checks

1. Deterministic: two generations are byte-identical.
2. `test-suite/scripts/br-plugin/check-branch.py` on a clone holding only this branch's commit, against the control's
   commit: only adds (the `business-rules/` files, and lines added to the integrate skill), `claude plugin validate
   --strict` passes on Claude Code 2.1.319, and the plugin installs and is enabled in an empty `CLAUDE_CONFIG_DIR`.
   The SDK-pin check compares with the `eecee918` pin, so it is read against the control instead: the same SDK tree.
3. Unedited: `plugins/maxio` is the generator's output byte for byte.

## Approval

Pending: Rafay approves this branch's diff against the control before any run (RULES 4).
