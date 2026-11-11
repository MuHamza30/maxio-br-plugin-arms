# C5 Observed notes: C4 plus what scripted probes of the Maxio sandbox showed

The control plugin (`control-eecee918`) plus `plugins/maxio/knowledge/`, rendered by C4's renderer over C4's records
and C5's. `git diff control-eecee918 c5-observed-notes` is exactly the treatment; against `c4-rule-catalogue` it adds
41 observed records and a pointer line that says some rules come from the sandbox:

- `knowledge/`: 1,278 rules (491 loud): C4's 1,237 from the spec and docs, and 41 with source `observed` (the probe
  and the date). `rules.md` indexes `every-request.md`, `background.md` and `operations/<area>.md`, as in C4.
- One pointer line, the first bullet of the router skill's grounding rules:

  > - **Provider rules:** every business rule Maxio's own spec and docs state, and what probes of the Maxio sandbox
  >   showed, is in `<plugin-root>/knowledge/`: `rules.md` indexes them (rules for every request, for background jobs,
  >   and one file per API area with a section per operation, headed with its SDK call). Read the rules for every
  >   request and for each operation you call: they are the source for how Maxio behaves; the map stays the source for
  >   SDK contracts.

## How the observations were gathered

The fixed method of plugin-screen-build.md, never the graded rules or the contract probes:

1. **The probe list was written and pushed before any probe ran:** `test-suite/scripts/br-plugin/c5/PROBES.md`
   (commit `b517bbceb`). Operations: the 19 the task prompt's 10 features use. Kinds: K1 twice, K2 each subscription
   state (`active`, `on_hold`, `canceled`), K3 boundary values for the numbers and dates the features set, K4 only the
   schema's required fields, K5 a read at once after a write, K6 two identical writes at the same moment.
2. **`c5/probe.py` ran them on the benchmark sandbox site `cp-exp-1`** on 2026-11-11, claimed in `WAVE-LOCK.md` and
   cleared after: 553 calls. The 76 probe subscriptions were purged with their customers and the 11 probe products
   archived; a search afterwards found none left. No subdomain or key is in the log.
3. **An agent that saw only the observations, C4's records and the operation list** wrote the records
   (`c5/records-prompt.md`): one per business rule the observations show and C4 lacks or states differently. 41
   records (24 loud, 17 silent), in `test-suite/content/br-plugin/maxio/c5/rules.json` beside the observations.

**Seven disagree with C4**, and each says it follows the sandbox: `createCustomer.12` (organization left null),
`createProduct.102` (no public signup page listed), `createUsage.7` (0.5 accepted and stored as 0),
`migrateSubscriptionProduct.11` (the prorated-migration message for a paused subscription's plain migration),
`every-request.14` (a resume date with no time stored as midnight, not noon), `reactivateSubscription.1` (the message
leaves out Unpaid), `reactivateSubscription.7` (the old balance kept, the new charge added). The C4 records stay in
the catalogue beside them.

Limits: the log keeps 2,000 characters of each answer, so two reads after a write (the product list after
`createProduct`, the customer's subscriptions after a repeated `createSubscription`) cannot show the new item. The
site needs a collection method that takes no card for a subscription with no card; that is this site's setting.

## How it was built

From plugin-experiments at `7a27ad359` (branch `research/br-plugin-screen-build`):

```
python -I test-suite/scripts/br-plugin/build-rule-catalogue.py --name c5-observed-notes \
    --rules test-suite/content/br-plugin/maxio/c4/rules.json --rules test-suite/content/br-plugin/maxio/c5/rules.json
python -I test-suite/scripts/br-plugin/assemble-branch.py --arms <this repo> --branch c5-observed-notes \
    --plugin test-suite/content/br-plugin/maxio/c5-observed-notes --readme <this README> --description "<...>"
```

The record is `test-suite/content/br-plugin/maxio/c5-observed-notes.json`. C4's extraction is described in the
`c4-rule-catalogue` branch README.

## Pins

| Pin | Value |
|---|---|
| Control | `control-eecee918`, plugin tree sha256 `a4ed19b537118186bca4c07bde18cb1d2c43f0777975a6a8335691eb0fb00ce5` |
| Rules | `c4/rules.json` (`54cbdadd…9214`, 1,237) and `c5/rules.json` (sha256 `961158ebeeabee4e414fc8bb3b6dc94d59da464b3234d3445192e0abaebd29e7`, 41) |
| Observations | `c5/observations.jsonl`, sha256 `cf6c70a3366978b8be7e25dba415626d5fa6d7540b4fccc0c2ae1818a1cb05b3`, 553 calls |
| Plugin | `plugins/maxio`: tree sha256 `f12954610b3d1acd3a16273e11ed581dfd52a815357263daff49aa6441dbfaae`, 1,612 files |
| SDK | `sdk/dotnet` without its docs: the SDK pin, `e8cebb1c…3673` |

## Checks

`test-suite/scripts/br-plugin/check-branch.py` on a clone holding only this branch's commit: only adds, same SDK,
`claude plugin validate --strict` passes on Claude Code 2.1.319, installs and is enabled in an empty
`CLAUDE_CONFIG_DIR`.

## Approval

Pending: a second person approves this branch's diff against the control before it is pinned in an arm (RULES 4).
