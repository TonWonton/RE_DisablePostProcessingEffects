# RE_DisablePostProcessingEffects

## Description
Disable or change the settings of different post-processing effects such as anti-aliasing, color correct, vignette, sharpening, various brightness options, volumetric fog, film grain, etc.

## Currently supported games
- Resident Evil Requiem (RE9)

## Dependencies
- REFramework C# API https://github.com/praydog/REFramework
- REFrameworkNETPluginConfig https://github.com/TonWonton/REFrameworkNETPluginConfig

## Prerequisites
- REFramework and the REFramework C# API (both the game specific .zip AND csharp-api.zip) https://github.com/praydog/REFramework-nightly/releases
- .NET 10.0 Desktop Runtime x64 (if you don't already have it installed) https://dotnet.microsoft.com/en-us/download/dotnet/10.0

## Installation
1. Install prerequisites
2. Download the plugin for the correct game and extract to game folder
3. The first startup after installing the `csharp-api` might take a while. Wait until it is complete. When the game isn't frozen anymore and it says "setting up script watcher" it is done
4. Open the REFramework UI -> `REFramework.NET script generated UI` -> change settings

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
