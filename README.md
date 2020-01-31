# ror2

Mods for [Risk of Rain 2](https://store.steampowered.com/app/632360/Risk_of_Rain_2/)

## Mods

[FuncKeyFairy](https://github.com/dasmods/ror2/tree/master/Mods/FuncKeyFairy) - Drops items when pressing F2, F3, or F4.

## Workflow

Commands are made using cake (C# Make), a cross platform build automation system. You must have Powershell installed on Windows.

### Getting Started

First, navigate to the project root in Powershell.

Then, initialize a ror2.config.json file (ignored by git) by running:

```
.\build.ps1 -Target InitConfig
```

Then, open ror2.config.json and enter your specific config settings:

_example ror2.config.json (use "/" as file separators not "\\")_

```json
{
  "ror2Dir": "D:/Steam/steamapps/common/Risk of Rain 2",
  "enabledMods": ["FuncKeyFairy"]
}
```

### Building 

To create a build, run

```
.\build.ps1
```

The built files will be in `<YOUR_PROJECT_ROOT>/bin`

### Installing and Playing

To create a build, install the `enabledMods` (from ror2.config.json), and play RoR2, ensure Steam is running and run the command:

```
.\build.ps1 -Target Play
```
