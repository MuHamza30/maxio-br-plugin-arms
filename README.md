# Business rules for the planner: the control plus C4's rules as Markdown, consulted while the plan is written

The control plugin (`control-eecee918`) plus `plugins/maxio/business-rules/`. `git diff control-eecee918
business-rules-planner` is exactly the treatment:

- `business-rules/`, Markdown only, at the plugin root (beside `skills/`):
  - `rules.md`: the index, with one row per operation that has rules;
  - `every-request.md` (44 rules);
  - `background.md` (86 rules, by job);
  - `operations/<area>.md`: 42 files, one section per operation, headed with its SDK call
    (`client.Subscriptions.CreateSubscription`) and route, loud rules first. 264 operations have rules.
- One added step, number 5 of *How to ground* in `dotnet-integrate-maxio-advanced-billing`. That section is the
  instructions the `maxio-advanced-billing-planner` agent follows while it writes `maxio-advanced-billing-plan.md`
  (and the main agent's, when it writes the plan itself):

  > 5. **Consult `<plugin-root>/business-rules/` for business knowledge the map does not carry** (the plugin root is
  >    the directory holding `skills/`): every business rule Maxio's own spec and docs state, as Markdown. Open
  >    `rules.md`, its index; read `every-request.md`, `background.md` where the scope relies on something Maxio does
  >    on its own (renewals, retries, webhooks), and, under `operations/`, the section for every operation in scope,
  >    headed with its SDK call. Plan for each rule that applies to the scope where it changes the plan (a step in §1,
  >    the fields a request must carry, or a Blocker in §6 where the plan makes a call Maxio would refuse) and name
  >    the rule's id there. The rules are the source for how Maxio behaves; the map stays the source for SDK
  >    contracts.

Nothing else changes: no file is removed or edited apart from that one added line, and the planner agent's own file
is untouched (its instructions are the integrate skill).

## How it differs from C4

The same 1,237 records (`c4/rules.json`, sha256 `54cbdadd…9214`) through the same renderer, so every Markdown file is
byte for byte C4's `knowledge/` file, except that `rules.md` drops its line about `rules.json`. Three differences:

| | C4 Rule catalogue | This branch |
|---|---|---|
| Folder | `knowledge/` | `business-rules/` |
| Formats | Markdown plus `rules.json` | Markdown only |
| Where the plugin points to it | The first bullet of the integrate skill's *Grounding rules*, read by every agent that loads the skill | A step of *How to ground*, the planner's grounding pass while it writes the plan |

So an arm against C4 tests where the pointer sits (the plan step against a standing rule), with the rules held
fixed. Where the rules came from, and its open sign-off, is C4's README.

## How it was built

Requested by Hamza on 2026-11-11. From plugin-experiments at `f0501faad` (branch `research/br-plugin-screen-build`):

```
python -I test-suite/scripts/br-plugin/build-rule-catalogue.py --name business-rules-planner \
    --rules test-suite/content/br-plugin/maxio/c4/rules.json
python -I test-suite/scripts/br-plugin/assemble-branch.py --arms <this repo> --branch business-rules-planner \
    --plugin test-suite/content/br-plugin/maxio/business-rules-planner --readme <this README> --description "<...>"
```

The record is `test-suite/content/br-plugin/maxio/business-rules-planner.json`.

## Pins

| Pin | Value |
|---|---|
| Control | `control-eecee918`, plugin tree sha256 `a4ed19b537118186bca4c07bde18cb1d2c43f0777975a6a8335691eb0fb00ce5` |
| Rules | `c4/rules.json`, 1,237 records, sha256 `54cbdadd23c0cf6c57e1a686f6cb73c6325b4dd62e07db5e2bd68c6b56569214` |
| Plugin | `plugins/maxio`: tree sha256 `ad076434859b43348567cc827982a69e9bfd26fef77c95a7440115ed84fd2786`, 1,611 files |
| SDK | `sdk/dotnet` without its docs: the SDK pin, `e8cebb1c…3673` |

## Checks

`test-suite/scripts/br-plugin/check-branch.py` on a clone holding only this branch's commit: only adds, same SDK,
`claude plugin validate --strict` passes on Claude Code 2.1.319, installs and is enabled in an empty
`CLAUDE_CONFIG_DIR`.

## Approval

Pending: a second person approves this branch's diff against the control before it is pinned in an arm (RULES 4).
The C4 extraction's sign-off covers its rules too.
