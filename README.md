# RE_DisablePostProcessingEffects

## Description
Disable or change the settings of different post-processing effects such as anti-aliasing, color correct, vignette, sharpening, various brightness options, volumetric fog, film grain, etc. for RE Engine games

## Currently supported games
- Resident Evil Requiem (RE9)

## Dependencies
- REFramework C# API https://github.com/praydog/REFramework
- REFrameworkNETPluginConfig https://github.com/TonWonton/REFrameworkNETPluginConfig

## Installation
### Lua
1. Install REFramework
  - NexusMods: https://www.nexusmods.com/residentevilrequiem/mods/13
  - GitHub: https://github.com/praydog/REFramework-nightly/releases
2. Download the lua script and extract to game folder
  - `RE9_DisablePostProcessingEffects.lua` should be in `\GAME_FOLDER\reframework\autorun\RE9_DisablePostProcessingEffects.lua`

### C#
1. Install prerequisites
  - REFramework + REFramework csharp-api (download and extract both `RE9.zip` AND `csharp-api.zip` to the game folder)
    - https://github.com/praydog/REFramework-nightly/releases
    - Only extract `dinput8.dll` from the `RE9.zip`
  - .NET 10.0 Desktop Runtime x64
    - https://dotnet.microsoft.com/en-us/download/dotnet/10.0
2. Download the plugin and extract to game folder
  - `RE9_DisablePostProcessingEffects.dll` should be in `\GAME_FOLDER\reframework\plugins\managed\RE9_DisablePostProcessingEffects.dll`

- If the `csharp-api` is installed correctly a CMD window will pop up when launching the game
- The first startup after installing the `csharp-api` might take a while. Wait until it is complete. When the game isn't frozen anymore and it says "setting up script watcher" it is done
- The mod settings are under `REFramework.NET script generated UI` instead of the normal `Script generated UI`

## Features
- Disable or change the settings of various post-processing effects
  - Anti-aliasing
    - TAA
  - Color correct
  - Vignette
  - Sharpening
  - Exposure
	- Local exposure
    - Auto exposure
  - Gamma
  - Brightness
  - Volumetric fog
  - Film grain
- All options have a `Default` setting
  - The mod will not change things set to `Default`
  - Allows for choosing which options you want to change without affecting other options or scenes
- Settings are saved to config file and automatically loaded

## Note
- Some of the options might not have an effect depending on your mod and in-game settings
  - Disabling TAA seems to have no effect when using DLSS or other upscalers
  - Changing sharpening may have no effect if TAA is enabled
