# maxio-br-plugin-arms

The plugins of the Maxio business-rule plugin screen in
[plugin-experiments](https://github.com/apimatic/plugin-experiments) (task `maxio-br`, eShopOnWeb, C#, Claude Code
2.1.319, Sonnet 5.5 medium). The build plan is `research/business-rules/plan-details/plugin-screen-build.md` there.

One branch per arm. Each branch is a Claude Code marketplace named `maxio-br-plugin-arms` with one plugin, `maxio`,
under `plugins/maxio/`. Every treatment branch starts from the control's commit and only adds to it, so
`git diff control-eecee918 <branch>` is exactly the treatment.

| Branch | Arm | What it will hold |
|---|---|---|
| `control-eecee918` | Control | The plugin codegen-v2 `eecee918` generates from the pinned Maxio spec, C# only, unedited |
| `c4-rule-catalogue` | C4 Rule catalogue | The control plus `knowledge/` files holding every rule record of a fresh extraction |
| `c1-spec-rule-prose` | C1 Spec rule prose | Regenerated from a spec copy with rule prose and 4xx examples written in |
| `c3-portal-docs` | C3 Portal docs | The control plus Maxio's portal docs as Markdown, an index and one pointer line |
| `d2-rules-skill` | D2 Rules skill + plan rows | The control plus a rules skill grouped by flow and plan rows that cite it |
| `d3-hooks` | D3 Hooks | The control plus a generic hook script reading C4's records |
| `c5-observed-notes` | C5 Observed notes | C4 plus records from scripted sandbox probes |

None is built yet. Each branch's README records how it was built (exact commands), its pins and who approved its diff.
Experiments pin a commit sha, never a branch name. The repo is private so a run with web search on cannot find another
arm's content.
