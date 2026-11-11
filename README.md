# D2 Rules skill + plan rows: the control plus C4's flow rules as a skill, cited from the plan

The control plugin (`control-eecee918`) plus two things, both rendered from C4's records. `git diff control-eecee918
d2-rules-skill` is exactly the treatment:

- **A new skill**, `skills/dotnet/dotnet-maxio-business-rules/`: `SKILL.md` (2.5 KB) indexes 18 flows, and
  `flows/<flow>.md` holds each flow's rules, loud first, each with the SDK calls it concerns. It keeps C4's flow rules
  only, by the fixed test of plugin-branches.md (a rule that spans several operations, every request or a background
  job, or whose category is order): 513 of C4's 1,237. Flows: every request (44); the background jobs (dunning,
  entitlement provisioning, exports, invoice issuance, invoicing, renewal, wallet funding, grant expiration, wallet
  provisioning, webhook delivery: 87); and the API's areas by the tag of the rule's first operation (subscriptions
  67, components and usage 64, catalog 62, invoicing 68, customers and payments 54, groups, wallets and features 52,
  site-wide 15).
- **One plan row**: row 12 of the router skill's PRODUCTION READINESS table
  (`skills/dotnet/dotnet-integrate-maxio-advanced-billing/SKILL.md`):

  > | 12 | **Provider rules** | For each flow the scope touches, the Maxio rules `dotnet-maxio-business-rules` lists
  > under it (every request, background jobs, and the rules that span several operations or fix their order), and how
  > the plan honours each one, or `N/A` with the reason the scope never meets it. A row that only names the skill is
  > **not addressed**. MUST load `dotnet-maxio-business-rules`. |

No `knowledge/` folder and no pointer line: the skill and the plan row are D2's whole delivery.

## How it was built

From plugin-experiments at `a502b6b45` (branch `research/br-plugin-screen-build`):

```
python -I test-suite/scripts/br-plugin/build-d2-rules-skill.py --rules test-suite/content/br-plugin/maxio/c4/rules.json
python -I test-suite/scripts/br-plugin/assemble-branch.py --arms <this repo> --branch d2-rules-skill \
    --plugin test-suite/content/br-plugin/maxio/d2-rules-skill --readme <this README> --description "<...>"
```

The record is `test-suite/content/br-plugin/maxio/d2-rules-skill.json`. C4's extraction is described in the
`c4-rule-catalogue` branch README.

## Pins

| Pin | Value |
|---|---|
| Control | `control-eecee918`, plugin tree sha256 `a4ed19b537118186bca4c07bde18cb1d2c43f0777975a6a8335691eb0fb00ce5` |
| Rules | `c4/rules.json`, sha256 `54cbdadd23c0cf6c57e1a686f6cb73c6325b4dd62e07db5e2bd68c6b56569214` |
| Plugin | `plugins/maxio`: tree sha256 `7b5ae6260821c4fb0d0abac0b6f631c98f031e578b36a842e0dd3ce70c723891`, 1,585 files |
| SDK | `sdk/dotnet` without its docs: the SDK pin, `e8cebb1c…3673` |

## Checks

`test-suite/scripts/br-plugin/check-branch.py` on a clone holding only this branch's commit: only adds (the router
skill gains one line), same SDK, `claude plugin validate --strict` passes on Claude Code 2.1.319, installs and is
enabled in an empty `CLAUDE_CONFIG_DIR`.

## Approval

Pending: a second person approves this branch's diff against the control before it is pinned in an arm (RULES 4).
