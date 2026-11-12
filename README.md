# maxio-br-plugin-arms

The plugins of the Maxio business-rule plugin screen in
[plugin-experiments](https://github.com/apimatic/plugin-experiments) (task `maxio-br`, eShopOnWeb, C#, Claude Code
2.1.319, Sonnet 5.5 medium). The build plan is `research/business-rules/plan-details/plugin-screen-build.md` there,
and the scripts that built every branch are under `test-suite/scripts/br-plugin/` on its branch
`research/br-plugin-screen-build`.

One branch per arm. Each branch is a Claude Code marketplace named `maxio-br-plugin-arms` with one plugin, `maxio`,
under `plugins/maxio/`. Every treatment branch starts from the control's commit, so `git diff control-eecee918
<branch>` is exactly the treatment; all but C1 only add to the control (C1 is regenerated from a changed spec).

| Branch | Commit | Arm | What it holds |
|---|---|---|---|
| `control-eecee918` | `1afb1e53` | Control | The plugin codegen-v2 `eecee918` generates from the pinned Maxio spec, C# only, unedited: 9 skills, the planner agent, the bundled SDK |
| `c4-rule-catalogue` | `d084e14f` | C4 Rule catalogue | The control plus `knowledge/`: 1,237 rules a fresh extraction found in Maxio's spec and docs, by operation, and one pointer line |
| `c1-spec-rule-prose` | `b17aa575` | C1 Spec rule prose | Regenerated from a spec copy with C4's rules in 264 operation descriptions and 173 4xx responses; the SDK's doc comments carry them |
| `c3-portal-docs` | `e88bedb1` | C3 Portal docs | The control plus Maxio's 74 portal content files in `sdk/dotnet/docs/`, an index and one pointer line |
| `d2-rules-skill` | `dd40bd45` | D2 Rules skill + plan rows | The control plus a skill holding C4's 513 flow rules in 18 flows, and a "provider rules" row in the plan |
| `d3-hooks` | `becd8c17` | D3 Hooks | The control plus a generic hook script pushing C4's rules after edits, after failed commands and at Stop |
| `c5-observed-notes` | `5192eeb6` | C5 Observed notes | C4 plus 41 rules from scripted probes of the Maxio sandbox (7 disagree with the docs), and one pointer line |
| `business-rules-planner` | `f36f394e` | Business rules for the planner | The control plus `business-rules/`: C4's 1,237 rules as Markdown only, and one *How to ground* step telling the planner to consult them while it writes the plan |

The codegen-v2 [PR #811](https://github.com/apimatic/codegen-v2/pull/811) pilot (plugin-experiments
`research/business-rules/baseline/plan.md`) has its own pair. Both are generated, unedited, at newer codegen-v2
commits, so they bundle the C# SDK `main` makes at `eed5f7cc`, not the SDK pin; the treatment starts from its own
control, so `git diff control-eed5f7cc pr811-rules-140ea4af` is exactly the treatment:

| Branch | Commit | Arm | What it holds |
|---|---|---|---|
| `control-eed5f7cc` | `2282c7bd` | Pilot control | The plugin codegen-v2 `main` generates at `eed5f7cc` (PR #811's base), with no `rules/`: 9 skills, the planner agent, the bundled SDK |
| `pr811-rules-140ea4af` | `ecd57ebc` | Pilot treatment | The plugin PR #811 generates at `140ea4af` from a build with Maxio's `rules/` (C4's 1,237 rules): `business-rules/`, a byte-for-byte copy, and one *How to ground* step for the planner |

Every branch passes `check-branch.py` on a clone holding only its commit: `claude plugin validate --strict` on Claude
Code 2.1.319, installs and is enabled in an empty `CLAUDE_CONFIG_DIR`, and (except C1) only adds and keeps the SDK
pin; the pilot pair's SDK is the same in both, but not the pin. Each branch's README records how it was built (exact
commands), its pins and who approved its diff.

**None is approved yet.** A second person approves each branch before it is pinned in an arm (RULES 4). Experiments
pin a commit sha, never a branch name. The repo is private so a run with web search on cannot find another arm's
content; arms clone only their pinned commit (`isolatedClone`), so a run's clone holds no other branch.
