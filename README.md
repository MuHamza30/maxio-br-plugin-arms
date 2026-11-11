# C1 Spec rule prose: the plugin regenerated from a spec copy with C4's rules written in

The only branch whose plugin is not the control plus additions: `plugins/maxio/` is what codegen-v2 `eecee918`
generates, unedited, from a copy of the Maxio spec with C4's rules written into it. Same generator, harness,
`apimatic.json` and command as the control; only the spec differs.

## What changed in the spec

`test-suite/scripts/br-plugin/build-c1-spec.py`, on a copy of `research/docs/maxio/src/spec` (the pin is never
touched), for each operation with C4 records (rules for every request are not an operation's and stay out):

- **264 operation descriptions** gain a "Business rules" list: each rule's condition, Maxio's outcome and what the
  app must do, loud rules first;
- **169 declared 4xx responses** gain the rules that return them, each with its example message, and **4 statuses**
  a loud rule names but the spec did not declare are added (description only, no body).

Only those lines change: each description is rewritten as a literal block holding its original text plus the
addition, and every changed description is read back to check it. Records name no parameter, so parameter
descriptions are left as they are. The spec copy's tree sha256 is `6e684ac9…7e7c`.

## What changed in the plugin, against the control

- **No skill, manifest, agent or plugin README changed.** The generator puts operation prose only into the SDK.
- **The SDK** (`sdk/dotnet`): 43 source files change, almost all in XML doc comments: 42 `Api/` files gain the rules
  in each method's `<remarks>` (where the skills send the agent for an operation's semantics), and 22 code lines
  change where an operation now declares a 4xx. 3 error types are new (`BulkRecordEventsError`, `ClearSiteError`,
  `RevokeBillingPortalAccessError`). The generated docs change with it: `api-reference.md`, `sdk-map.md` and 4 map
  pages. Without its docs the SDK has 1,508 files, tree sha256 `36472bdc…4906`, so it is not the SDK pin
  (`e8cebb1c…3673`).

## The prose checked against the sandbox

C5's probes (the `c5-observed-notes` branch) ran the task's 19 operations against the Maxio sandbox. Six of the rules
written into this spec are contradicted there, each kept here as the docs state it:

| C4 rule in the prose | The docs | The sandbox |
|---|---|---|
| `createCustomer.12` | an omitted organization is filled from the name | left null |
| `createProduct.102` | a public signup page is created for a new product | none listed |
| `createUsage.7` | a decimal quantity on a non-fractional component is refused | 0.5 accepted, stored as 0 |
| `migrateSubscriptionProduct.11` | this message answers a prorated migration | it also answers a plain migration of a paused subscription |
| `reactivateSubscription.1` | the message names Canceled, Trial Ended or Unpaid | it names Canceled or Trial Ended |
| `reactivateSubscription.7` | without `preserve_balance` the balance is cleared | the old balance stays and the new charge is added |

Whether C1 keeps these as the docs state them (as C3 and C4 do) or drops or corrects them before the screen is open.

## How it was built

From plugin-experiments at `6e21c1046` (branch `research/br-plugin-screen-build`), with a clean codegen-v2 checkout at
`eecee918`:

```
python -I test-suite/scripts/br-plugin/build-c1-spec.py --rules test-suite/content/br-plugin/maxio/c4/rules.json
python -I test-suite/scripts/br-plugin/generate-br-plugin.py --codegen <codegen-v2> \
    --commit eecee918f4c4925690ded4c8fe8b2051d3032be1 --spec test-suite/content/br-plugin/maxio/c1-spec \
    --out test-suite/content/br-plugin/maxio/c1-spec-rule-prose --twice
python -I test-suite/scripts/br-plugin/assemble-branch.py --arms <this repo> --branch c1-spec-rule-prose \
    --plugin test-suite/content/br-plugin/maxio/c1-spec-rule-prose --readme <this README> --description "<...>"
```

Two generations were byte-identical. The records are `c1-spec.json` and `c1-spec-rule-prose.json` beside the output.

## Pins

| Pin | Value |
|---|---|
| codegen-v2 | `eecee918f4c4925690ded4c8fe8b2051d3032be1` |
| Rules | `c4/rules.json`, sha256 `54cbdadd23c0cf6c57e1a686f6cb73c6325b4dd62e07db5e2bd68c6b56569214` |
| Spec copy | tree sha256 `6e684ac93f2d9c42a9bd116b2aefa17ce24c7a67b748b51c3167e7fba9927e7c`, 744 files |
| Plugin | `plugins/maxio`: tree sha256 `f9d7191bdd7a26cabf85ed59707feacfecb524f78a093c42e86d56aef0d5010d`, 1,569 files |
| SDK | `sdk/dotnet` without its docs: tree sha256 `36472bdced4388b3722dd8975b9d476cf9bd17045e5b5f519e9ea1d47aeb4906`, 1,508 files |

## Checks

`test-suite/scripts/br-plugin/check-branch.py --sdk-may-change` on a clone holding only this branch's commit:
`claude plugin validate --strict` passes on Claude Code 2.1.319, and the plugin installs and is enabled in an empty
`CLAUDE_CONFIG_DIR`. Only-adds and same-SDK do not apply to C1.

## Open

- What the C1 arm stages in the clone: the pinned SDK at `sdk/maxio` with the calibration prompt (then the prose
  reaches the agent only through the plugin's bundled copy), or C1's regenerated SDK.

## Approval

Pending: a second person approves this branch before it is pinned in an arm (RULES 4).
