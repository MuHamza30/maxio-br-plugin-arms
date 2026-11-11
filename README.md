# C3 Portal docs: the control plus Maxio's portal guides

The control plugin (`control-eecee918`) plus Maxio's own portal content, copied unedited. `git diff control-eecee918
c3-portal-docs` is exactly the treatment:

- `plugins/maxio/sdk/dotnet/docs/`: the 74 files of the Maxio portal source's `content/` (65 Markdown pages, 1 HTML
  page, 8 `toc.yml`), byte for byte as git stores them: concepts, API usage (errors and rate limits, duplicate
  prevention, list operations, versioning), webhooks, the three walkthrough stubs and the announcements.
- `plugins/maxio/sdk/dotnet/docs/index.md`: one row per page, its title and its path.
- One pointer line, the first bullet of the router skill's grounding rules
  (`skills/dotnet/dotnet-integrate-maxio-advanced-billing/SKILL.md`):

  > - **Provider docs:** Maxio's own portal guides ship with this plugin as Markdown at
  >   `<plugin-root>/sdk/dotnet/docs/`, and `index.md` there lists every page by title. Read the pages on the flows
  >   you implement: they are the source for how Maxio behaves; the map stays the source for SDK contracts.

The pages teach what the docs state, including where the sandbox disagrees (maxio-v03, maxio-p1); that is C3's
content, not a bug to fix.

## How it was built

From plugin-experiments at `5e6039e84` (branch `research/br-plugin-screen-build`), after the control was generated
there (`control-eecee918`'s README):

```
python -I test-suite/scripts/br-plugin/build-c3-portal-docs.py
python -I test-suite/scripts/br-plugin/assemble-branch.py --arms <this repo> --branch c3-portal-docs \
    --plugin test-suite/content/br-plugin/maxio/c3-portal-docs --readme <this README> --description "<...>"
```

The record is `test-suite/content/br-plugin/maxio/c3-portal-docs.json`.

## Pins

| Pin | Value |
|---|---|
| Control | `control-eecee918`, plugin tree sha256 `a4ed19b537118186bca4c07bde18cb1d2c43f0777975a6a8335691eb0fb00ce5` |
| Content | plugin-experiments `research/docs/maxio/src/content`, 74 files, tree sha256 `5f0852ba53160f3648d94483586aaeed70257d2903defe4ec145e0db211608d4` (as copied, LF) |
| Plugin | `plugins/maxio`: tree sha256 `f0149d4aaee6b97640c4ed0fd5c0bc40087a3adc497c0eeb97a0947f2f1cd333`, 1,641 files |
| SDK | `sdk/dotnet` without its docs (`README.md`, `api-reference.md`, `sdk-map.md`, `map/`, `docs/`): the SDK pin, `e8cebb1c…3673` |

## Checks

`test-suite/scripts/br-plugin/check-branch.py` on a clone holding only this branch's commit: only adds (against the
control's commit), same SDK, `claude plugin validate --strict` passes on Claude Code 2.1.319, and the plugin installs
and is enabled in an empty `CLAUDE_CONFIG_DIR`.

## Approval

Pending: a second person approves this branch's diff against the control before it is pinned in an arm (RULES 4).
