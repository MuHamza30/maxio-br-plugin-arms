# Control for the codegen-v2 PR #811 pilot: the Maxio plugin codegen-v2 `main` generates at eed5f7cc

`plugins/maxio/` is exactly what the generator wrote, byte for byte, unedited (RULES 5). It is the control of the
business-rules pilot (plugin-experiments `research/business-rules/baseline/plan.md`): codegen-v2 `main` at `eed5f7cc`,
which is also the base of [PR #811](https://github.com/apimatic/codegen-v2/pull/811), with no `rules/` in the build.
The treatment branch, `pr811-rules-140ea4af`, starts from this branch's commit, so `git diff control-eed5f7cc
pr811-rules-140ea4af` is exactly the treatment.

This branch starts from `control-eecee918` only so the repo's files (`.gitattributes`, the marketplace) carry over;
`git diff control-eecee918 control-eed5f7cc` is what codegen-v2 `main` changed between the two commits, not a
treatment.

## How it was built

From plugin-experiments at `932ab8ef5` (branch `research/br-plugin-screen-build`), with a clean codegen-v2 checkout
at `eed5f7cc`:

```
python -I test-suite/scripts/br-plugin/generate-br-plugin.py --codegen <codegen-v2 checkout> \
    --commit eed5f7cc1d19075d4bf3a961ca546c58104ed90a --unpinned --twice \
    --out test-suite/content/br-plugin/maxio/control-eed5f7cc
python -I test-suite/scripts/br-plugin/assemble-branch.py --arms <this repo> --branch control-eed5f7cc \
    --base control-eecee918 --plugin test-suite/content/br-plugin/maxio/control-eed5f7cc --readme <this README> \
    --description "<...>"
```

The harness, `test-suite/scripts/br-plugin/BrPluginHarness.cs`, is used unchanged (the same file as
`control-eecee918`'s): it runs, in process and in order, the activities codegen-v2's Functions host runs for
`POST /plugin` (`apimatic plugin generate`) and writes out the `plugin.zip` AggregatePlugin makes. The build input is
`test-suite/content/br-plugin/maxio/apimatic.json` (C# only, SDK bundled) with the Maxio spec beside it as `spec/`.
`--unpinned` lets the script generate at a commit other than the business-rule SDK pin's (`eecee918`). The record is
`test-suite/content/br-plugin/maxio/control-eed5f7cc.json`.

## Pins

| Pin | Value |
|---|---|
| codegen-v2 | `eed5f7cc1d19075d4bf3a961ca546c58104ed90a` (`main`, and the base of PR #811) |
| Spec | plugin-experiments `research/docs/maxio/src/spec`, tree sha256 `619868198e3f21002ef62549d982607f2f4c33b5ffdabeb9a00bb368e1484bdc` (the same spec as `control-eecee918`) |
| Rules | none: the build has no `rules/` |
| Plugin | `plugins/maxio`: tree sha256 `96a94ef3a0fc1f52f8846c4b7527adb83509bcc011f9ab2e900b520a2bb4de32` over 1,563 files |
| SDK | `sdk/dotnet` without its docs: tree sha256 `5769460e1beae2324e640f2577b57feba041a08837505432410a5f7428f3f809` over 1,502 files. Not the business-rule SDK pin (`e8cebb1c…`, generated at `eecee918`): `main` changed the C# SDK since |
| Harness | `BrPluginHarness.cs` sha256 `587ff0d2d61e944d68bb70ab0dfb0cc82d45cc66f7c0e9862e43085785fede21` |

## Checks

1. Deterministic: two generations are byte-identical.
2. `test-suite/scripts/br-plugin/check-branch.py` on a clone holding only this branch's commit: `claude plugin
   validate --strict` passes on Claude Code 2.1.319, and the plugin installs and is enabled in an empty
   `CLAUDE_CONFIG_DIR`. The SDK-pin check is skipped (`--sdk-may-change`), for the reason under Pins.
3. Unedited: `plugins/maxio` is the generator's output byte for byte; `.gitattributes` (`* -text`) keeps every
   checkout byte for byte.

## Approval

Pending: a second person approves this branch before it is pinned in an arm (RULES 4).
