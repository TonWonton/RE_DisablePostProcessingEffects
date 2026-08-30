# RE_DisablePostProcessingEffects

## Description
Disable or change the settings of different post-processing effects for RE Engine games

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
  - Contrast
  - Gamma (game and UI)
  - Brightness (game and UI)
  - Volumetric fog
  - Film grain
  - Lens distortion
- All options have a `Default` setting
  - The mod will not change things set to `Default`
  - Allows for choosing which options you want to change without affecting other options or scenes
- Settings are saved to config file and automatically loaded

## Currently supported games
- Resident Evil Requiem (RE9)
- PRAGMATA
- Monster Hunter Wilds (MHWilds)

## Dependencies
- REFramework C# API: https://github.com/praydog/REFramework
- REFrameworkNETPluginConfig: https://github.com/TonWonton/REFrameworkNETPluginConfig

## Installation
### Lua
1. Install REFramework
    - GitHub: https://github.com/praydog/REFramework-nightly/releases
2. Download the lua script and extract to game folder
    - .lua file should be in `\GAME_FOLDER\reframework\autorun\`

### C#
1. Install REFramework and the REFramework C# API
    - GitHub: https://github.com/praydog/REFramework-nightly/releases
2. Install .NET 10.0 Desktop Runtime x64 (if not already installed)
    - https://dotnet.microsoft.com/en-us/download/dotnet/10.0
3. Download the plugin and extract to game folder
    - .dll file should be in `\GAME_FOLDER\reframework\plugins\managed\`

## Note
- Some of the options might not have an effect depending the game, your mod settings, and in-game settings
  - Disabling TAA seems to have no effect when using DLSS or other upscalers
  - Changing sharpening may have no effect if TAA is enabled
- [RE9] The custom contrast option seems to have no effect
- [PRAGMATA] The film grain option seems to have no effect
  - Try to disable sharpening and see if that helps
