# C4 Rule catalogue: the control plus every rule a fresh extraction found in Maxio's spec and docs

The control plugin (`control-eecee918`) plus `plugins/maxio/knowledge/`. `git diff control-eecee918 c4-rule-catalogue`
is exactly the treatment:

- `knowledge/rules.json`: 1,237 rule records (467 loud, 770 silent) in the generic record schema: id, operations,
  scope, `on`/`does` category, loudness, condition, provider outcome, what the app must do, sources (path and exact
  quote), conflict.
- `knowledge/rules.md`: the index. `every-request.md` (44 rules), `background.md` (86, by job), and
  `operations/<area>.md`, 42 files with one section per operation, headed with its SDK call
  (`client.Subscriptions.CreateSubscription`) and route; loud rules first; 264 operations have rules.
- One pointer line, the first bullet of the router skill's grounding rules:

  > - **Provider rules:** every business rule Maxio's own spec and docs state is in `<plugin-root>/knowledge/`:
  >   `rules.md` indexes them (rules for every request, for background jobs, and one file per API area with a section
  >   per operation, headed with its SDK call). Read the rules for every request and for each operation you call: they
  >   are the source for how Maxio behaves; the map stays the source for SDK contracts.

## Where the rules came from

A fresh extraction, never Saeed's knowledge, the graded rules, the sandbox probes or the calibration records:

1. `test-suite/scripts/br-plugin/c4/prepare.py` copied only `research/docs/maxio/src/spec` and `src/content` (as git
   stores them) to a folder outside every repo, indexed the 285 operations, and split them into 7 groups by tag.
2. Seven agents (Claude Opus 5.5), one per group, read only that folder and wrote one record per rule; every quote is
   checked to appear in the file it cites (`c4/validate_records.py`). 1,252 records.
3. `c4/dedup.py` merged 14 rules two groups had both recorded (threshold 0.9).
4. `c4/sample.py` drew each group's random 10% (seed `3669496036`, 128 records); the next group's agent re-checked
   them against the sources: 100 confirmed, 27 corrected (mostly line numbers, a category or a missing source),
   1 rejected (`every-request.5`, not a business rule).
5. `c4/merge.py` applied the verdicts: 1,237 records, `test-suite/content/br-plugin/maxio/c4/rules.json` (sha256
   `54cbdadd…9214`), with the raw records, samples, checks and prompts under `c4/extraction/`.

The docs teach what they state, including where the sandbox disagrees; only C5 corrects them. No person has reviewed
the extraction yet; whoever signs it off must not compare it with the graded rules.

## How it was built

From plugin-experiments at `a502b6b45` (branch `research/br-plugin-screen-build`):

```
python -I test-suite/scripts/br-plugin/build-rule-catalogue.py --name c4-rule-catalogue \
    --rules test-suite/content/br-plugin/maxio/c4/rules.json
python -I test-suite/scripts/br-plugin/assemble-branch.py --arms <this repo> --branch c4-rule-catalogue \
    --plugin test-suite/content/br-plugin/maxio/c4-rule-catalogue --readme <this README> --description "<...>"
```

The record is `test-suite/content/br-plugin/maxio/c4-rule-catalogue.json`.

## Pins

| Pin | Value |
|---|---|
| Control | `control-eecee918`, plugin tree sha256 `a4ed19b537118186bca4c07bde18cb1d2c43f0777975a6a8335691eb0fb00ce5` |
| Rules | `c4/rules.json`, 1,237 records, sha256 `54cbdadd23c0cf6c57e1a686f6cb73c6325b4dd62e07db5e2bd68c6b56569214` |
| Plugin | `plugins/maxio`: tree sha256 `7f89a8edc940c13406e316d490b0eb08e2262d1d657d83006cf582c94f433f9f`, 1,612 files |
| SDK | `sdk/dotnet` without its docs: the SDK pin, `e8cebb1c…3673` |

## Checks

`test-suite/scripts/br-plugin/check-branch.py` on a clone holding only this branch's commit: only adds, same SDK,
`claude plugin validate --strict` passes on Claude Code 2.1.319, installs and is enabled in an empty
`CLAUDE_CONFIG_DIR`.

## Approval

Pending: a second person approves this branch's diff against the control before it is pinned in an arm (RULES 4),
and someone signs off the extraction.
