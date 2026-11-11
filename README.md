# Control arm: the Maxio plugin codegen-v2 generates at eecee918

`plugins/maxio/` is exactly what the generator wrote, byte for byte, unedited (RULES 5). Every treatment branch of
this repo starts from this branch's commit and only adds to it.

## How it was built

From plugin-experiments at `9c2d0c33f` (branch `research/br-plugin-screen-build`), with a clean codegen-v2 checkout
at `eecee918`:

```
python -I test-suite/scripts/br-plugin/generate-br-plugin.py \
    --codegen <codegen-v2 checkout> --commit eecee918f4c4925690ded4c8fe8b2051d3032be1 --twice
```

The script copies `test-suite/scripts/br-plugin/BrPluginHarness.cs` into codegen-v2's test project for the run, and
removes it afterwards. The harness runs, in process and in order, the activities codegen-v2's Functions host runs for
`POST /plugin` (`apimatic plugin generate`): ValidateApimaticConfig, ValidateSpecFile, ValidatePackageSettings,
GenerateSdk, GenerateV4Skills and AggregatePlugin. It calls each activity's own command handler, with the test
project's in-memory blob stores in place of Azure storage, and writes out the `plugin.zip` AggregatePlugin makes.
The build input is `test-suite/content/br-plugin/maxio/apimatic.json` with the Maxio spec beside it as `spec/`.
The record of the generation is `test-suite/content/br-plugin/maxio/control.json`.

`apimatic.json` copies the published Maxio plugin's metadata (`context-plugins/plugin-marketplace`, `plugins/maxio`,
0.2.2) and declares only `csharp`, with no publishing block and no package settings, so the C# SDK is bundled:

```json
{
  "plugin": {
    "pluginId": "maxio",
    "pluginName": "Maxio",
    "pluginVersion": "0.2.2",
    "author": { "name": "APIMatic", "email": "developer@apimatic.io" },
    "license": "MIT",
    "homepage": "https://context.apimatic.io/",
    "repository": "https://github.com/context-plugins/plugin-marketplace"
  },
  "languages": { "csharp": {} }
}
```

## Pins

| Pin | Value |
|---|---|
| codegen-v2 | `eecee918f4c4925690ded4c8fe8b2051d3032be1` |
| Spec | plugin-experiments `research/docs/maxio/src/spec`, tree sha256 `619868198e3f21002ef62549d982607f2f4c33b5ffdabeb9a00bb368e1484bdc` (744 files, as git stores them, LF) |
| SDK | `plugins/maxio/sdk/dotnet` without `README.md`, `api-reference.md`, `sdk-map.md` and `map/`: tree sha256 `e8cebb1cd8e89812ddb8bad45c38c6a0c1bfbfa91504e1c9402fe4b5b1203673` over 1,505 files, the same as the business-rule SDK pin (`test-suite/content/br-sdk/PIN.json`, arm `br-maxio-sdk`) |
| Plugin | `plugins/maxio`: tree sha256 `a4ed19b537118186bca4c07bde18cb1d2c43f0777975a6a8335691eb0fb00ce5` over 1,566 files (`test-suite/scripts/content-tree-sha.ps1`) |
| Harness | `BrPluginHarness.cs` sha256 `587ff0d2d61e944d68bb70ab0dfb0cc82d45cc66f7c0e9862e43085785fede21` |

## What the plugin holds

- `.claude-plugin/plugin.json` (skills `./skills/dotnet`, agent `./scoped/agents/maxio-advanced-billing-planner.md`),
  `.codex-plugin/plugin.json`, `.cursor-plugin/plugin.json`, a root `plugin.json` and the plugin `README.md`.
- 9 C# skills under `skills/dotnet/`: `dotnet-integrate-maxio-advanced-billing`, `dotnet-getting-started` and
  `dotnet-authentication` (generated for Maxio), and `dotnet-client-initialization`, `dotnet-calling-endpoints`,
  `dotnet-models`, `dotnet-error-handling`, `dotnet-configuration-resilience` and `dotnet-testing` (the same for
  every API).
- The planner agent, `scoped/agents/maxio-advanced-billing-planner.md` (Claude Code only).
- The bundled C# SDK, `sdk/dotnet/`, with its generated `README.md`, `api-reference.md`, `sdk-map.md` and `map/`.

## Checks

1. Same SDK: `sdk/dotnet` without the generated docs has the SDK pin's tree sha.
2. Deterministic: three generations, the last two from the committed scripts, are byte-identical.
3. Valid: `claude plugin validate --strict` passes on Claude Code 2.1.319.
4. Loads: installs and loads in an empty `CLAUDE_CONFIG_DIR` on Claude Code 2.1.319, from a clone holding only the
   pinned commit, as `new-run.ps1` installs a plugin arm.
5. Unedited: `plugins/maxio` is the generator's output byte for byte; `.gitattributes` (`* -text`) keeps every
   checkout byte for byte.

## Approval

Pending: a second person approves this branch before it is pinned in an arm (RULES 4).
