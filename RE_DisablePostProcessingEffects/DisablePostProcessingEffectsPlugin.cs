using System.Diagnostics.CodeAnalysis;
using Hexa.NET.ImGui;
using REFrameworkNET.Callbacks;
using REFrameworkNET.Attributes;
using REFrameworkNET;
using REFrameworkNETPluginConfig;
using via.render;


namespace RE_DisablePostProcessingEffects
{
	public partial class DisablePostProcessingEffectsPlugin
	{
		//Plugin
		public const string COPYRIGHT = "";
		public const string COMPANY = "https://github.com/TonWonton/RE_DisablePostProcessingEffects";
		public const string GUID_VERSION = GUID + " v" + VERSION;



		//Native
		private static NativeObject? _renderer;
		private static uint _getRenderConfigIndex;
		private static NativeObject? _displaySettings;
		private static uint _setGammaIndex, _setOutputLowerLimitIndex, _setOutputUpperLimitIndex, _setGammaForOverlayIndex, _setOutputLowerLimitForOverlayIndex, _setOutputUpperLimitForOverlayIndex, _setUseSDRBrightnessOptionForOverlayIndex;

		//Managed
		private static RenderConfig? _renderConfig;
		private static ToneMapping? _toneMapping;
		private static LDRPostProcess? _ldrPostProcess;
		private static LDRColorCorrect? _ldrColorCorrect;
		private static LDRLensDistortion? _ldrLensDistortion;
		private static LDRFilmGrain? _ldrFilmGrain;
		private static VolumetricFogControl? _volumetricFogControl;

		//Result and param
		private static readonly object _resultObject = new object(); //Init with dummy objects
		private static readonly object[] _floatArg = new object[1] { default(float) };
		private static readonly object[] _boolArg = new object[1] { default(bool) };

		//Var
		private static bool _initialized = false;

		[MemberNotNullWhen(true,
			nameof(_renderer),
			nameof(_displaySettings),
			nameof(_renderConfig),
			nameof(_toneMapping),
			nameof(_ldrPostProcess),
			nameof(_ldrColorCorrect),
			nameof(_ldrLensDistortion),
			nameof(_ldrFilmGrain),
			nameof(_volumetricFogControl)
#if RE9
			, nameof(_renderingManager)
#endif
#if PRAGMATA
			, nameof(_cameraSystem)
#endif
#if MHWILDS
			, nameof(_cameraManager)
			, nameof(_graphicsManager)
#endif
		)]
		public static bool Initialized { get { return _initialized; } }



		//Apply
		private static partial void ApplyGameSpecificNonPersistentSettings();
		private static void ApplyNonPersistentSettings()
		{
			if (Initialized)
			{
				RenderConfig renderConfig = _renderConfig;
				ToneMapping toneMapping = _toneMapping;
				ToggleType toggleType;
				SharpnessType sharpnessType;

				//Anti-aliasing
				AntiAliasingType antiAliasingType = _antiAliasingType.Value;
				if (antiAliasingType != AntiAliasingType.DEFAULT)
				{
					renderConfig.AnitiAliasingSetting = antiAliasingType.ToRenderConfigAntiAliasingType();
				}

				//Sharpening
				sharpnessType = _sharpnessType.Value;
				if (sharpnessType != SharpnessType.Default)
				{
					renderConfig.SharpnessSetting = sharpnessType.ToRenderConfigSharpnessType();
				}

				//Color correction
				toggleType = _colorCorrect.Value;
				if (toggleType != ToggleType.Default)
				{
					_ldrColorCorrect.Enabled = toggleType == ToggleType.Enable;
				}

				//TAA
				TemporalAA taa = _taa.Value;
				if (taa != TemporalAA.Default)
				{
					toneMapping.setTemporalAA(taa.ToToneMappingTemporalAA());
				}

				TemporalAAAlgorithm taaAlgorithm = _taaAlgorithm.Value;
				if (taaAlgorithm != TemporalAAAlgorithm.Default)
				{
					toneMapping.setTemporalAAAlgorithm(taaAlgorithm.ToToneMappingTemporalAAAlgorithm());
				}

				toggleType = _taaJitter.Value;
				if (toggleType != ToggleType.Default)
				{
					toneMapping.EchoEnabled = toggleType == ToggleType.Enable;
				}

				//Exposure
				toggleType = _exposure.Value;
				if (toggleType != ToggleType.Default)
				{
					toneMapping.ExposureEnable = toggleType == ToggleType.Enable;
					toneMapping.EV = _ev.Value;
				}

				//Auto exposure
				AutoExposure autoExposure = _autoExposure.Value;
				if (autoExposure != AutoExposure.Default)
				{
					toneMapping.setAutoExposure(autoExposure.ToToneMappingAutoExposure());
					toneMapping.AutoExposureMinEV = _autoExposureMinEV.Value;
					toneMapping.AutoExposureMaxEV = _autoExposureMaxEV.Value;
				}

				ReferenceLuminance referenceLuminance = _referenceLuminance.Value;
				if (referenceLuminance != ReferenceLuminance.Default)
				{
					toneMapping.ReferenceLuminance = _referenceLuminance.Value.ToToneMappingReferenceLuminance();
				}

				//Local exposure
				toggleType = _localExposure.Value;
				if (toggleType != ToggleType.Default)
				{
					toneMapping.EnableLocalExposure = toggleType == ToggleType.Enable;
				}

				LocalExposureType localExposureType = _localExposureType.Value;
				if (localExposureType != LocalExposureType.Default)
				{
					toneMapping.setLocalExposureType(localExposureType.ToToneMappingLocalExposureType());
				}

				//Vignette
				Vignette vignette = _vignette.Value;
				if (vignette != Vignette.Default)
				{
					toneMapping.setVignetting(vignette.ToToneMappingVignetting());
					toneMapping.VignettingBrightness = _vignetteBrightness.Value;
				}

				//Sharpening
				sharpnessType = _sharpnessType.Value;
				if (sharpnessType != SharpnessType.Default)
				{
					toneMapping.Sharpness = _sharpness.Value;
				}

				//Volumetric fog
				toggleType = _volumetricFog.Value;
				if (toggleType != ToggleType.Default)
				{
					_volumetricFogControl.Enabled = toggleType == ToggleType.Enable;
				}

				//Apply game specific settings
				ApplyGameSpecificNonPersistentSettings();
			}
		}

		private static partial void ApplyGameSpecificPersistentSettings();
		private static void ApplyPersistentSettings()
		{
			if (Initialized)
			{
				NativeObject displaySettings = _displaySettings;
				ToneMapping toneMapping = _toneMapping;
				object result = _resultObject;
				object[] floatArg = _floatArg;
				object[] boolArg = _boolArg;

				//Contrast
				if (_customContrast.Value)
				{
					toneMapping.Contrast = _contrast.Value;
				}

				//Gamma
				if (_customGamma.Value)
				{
					displaySettings.InvokeSetMethod_Float32(_setGammaIndex, floatArg, _gamma.Value, ref result);
				}

				//Brightness
				if (_customBrightness.Value)
				{
					displaySettings.InvokeSetMethod_Float32(_setOutputLowerLimitIndex, floatArg, _minBrightness.Value, ref result);
					displaySettings.InvokeSetMethod_Float32(_setOutputUpperLimitIndex, floatArg, _maxBrightness.Value, ref result);
				}

				//UI gamma
				if (_customUIGamma.Value)
				{
					displaySettings.InvokeSetMethod_Float32(_setGammaForOverlayIndex, floatArg, _uiGamma.Value, ref result);
				}

				//UI brightness
				bool customUIBrightness = _customUIBrightness.Value;
				if (customUIBrightness)
				{
					displaySettings.InvokeSetMethod_Bool(_setUseSDRBrightnessOptionForOverlayIndex, boolArg, customUIBrightness, ref result);
					displaySettings.InvokeSetMethod_Float32(_setOutputLowerLimitForOverlayIndex, floatArg, _minUIBrightness.Value, ref result);
					displaySettings.InvokeSetMethod_Float32(_setOutputUpperLimitForOverlayIndex, floatArg, _maxUIBrightness.Value, ref result);
				}

				//Apply game specific settings
				ApplyGameSpecificPersistentSettings();
			}
		}



		//Callbacks
		[Callback(typeof(BeginRendering), CallbackType.Pre)]
		public static void BeginRendering_Pre()
		{
			if (_initialized == false)
			{
				Initialize();
				if (_initialized == false) return;
			}

			ApplyNonPersistentSettings();

			if (API.IsDrawingUI() == false)
			{
				//Try save config if not drawing UI
				TrySaveConfig();
			}
		}



		//Init
		private static bool TryInitializeNative()
		{
			//Get shared native objects and cache method indexes
			if (Utility.TryGetAndCacheNativeSingleton<Renderer>(ref _renderer) == false) return false;
			if (_renderer.TryGetAndCacheMethodIndex(ref _getRenderConfigIndex, "get_RenderConfig") == false) return false;

			if (Utility.TryGetAndCacheNativeSingleton<DisplaySettings>(ref _displaySettings) == false) return false;
			if (_displaySettings.TryGetAndCacheMethodIndex(
				ref _setGammaIndex, "set_Gamma",
				ref _setOutputLowerLimitIndex, "set_OutputLowerLimit",
				ref _setOutputUpperLimitIndex, "set_OutputUpperLimit",
				ref _setOutputLowerLimitForOverlayIndex, "set_OutputLowerLimitForOverlay",
				ref _setOutputUpperLimitForOverlayIndex, "set_OutputUpperLimitForOverlay",
				ref _setUseSDRBrightnessOptionForOverlayIndex, "set_UseSDRBrightnessOptionForOverlay",
				ref _setGammaForOverlayIndex, "set_GammaForOverlay"
			) == false) return false;

			return true;
		}

		private static bool TryGetAndCacheRenderConfig([NotNullWhen(true)] ref RenderConfig? renderConfig)
		{
			if (renderConfig != null) return true;
			else
			{
				NativeObject? renderer = _renderer;
				if (renderer != null)
				{
					object result = _resultObject;
					return renderer.InvokeGetMethod_TryReturnValueAs(_getRenderConfigIndex, null, ref result, out renderConfig);
				}
			}

			return false;
		}
		private static partial bool TryGetAndCacheToneMapping([NotNullWhen(true)] ref ToneMapping? toneMapping);
		private static partial bool TryGetAndCacheLDRPostProcess([NotNullWhen(true)] ref LDRPostProcess? ldrPostProcess);
		private static partial bool TryGetAndCacheVolumetricFogControl([NotNullWhen(true)] ref VolumetricFogControl? volumetricFogControl);
		private static bool TryInitializeManaged()
		{
			//RenderConfig
			if (TryGetAndCacheRenderConfig(ref _renderConfig) == false) return false;

			//Get shared managed objects
			if (TryGetAndCacheToneMapping(ref _toneMapping) == false) return false;
			if (TryGetAndCacheLDRPostProcess(ref _ldrPostProcess) == false) return false;
			if (TryGetAndCacheVolumetricFogControl(ref _volumetricFogControl) == false) return false;

			//Cache LDRPostProcess components
			LDRPostProcess ldrPostProcess = _ldrPostProcess;
			if ((_ldrColorCorrect = ldrPostProcess.ColorCorrect) == null) return false;
			if ((_ldrLensDistortion = ldrPostProcess.LensDistortion) == null) return false;
			if ((_ldrFilmGrain = ldrPostProcess.FilmGrain) == null) return false;

			return true;
		}

		private static partial bool TryInitializeGameSpecific();
		private static void Initialize()
		{
			if (TryInitializeNative() == false) return;
			if (TryInitializeManaged() == false) return;
			if (TryInitializeGameSpecific() == false) return;

			//On success
			_initialized = true;
#if DEBUG
			Debug.TryDebugPrintValues();
#endif
			ApplyPersistentSettings();

			Log.Info("Initialized");
		}

		[PluginEntryPoint]
		private static void Load()
		{
			LoadConfig();
			RegisterConfigEvents();
			Log.Info("Loaded " + VERSION);
		}

		[PluginExitPoint]
		private static void Unload()
		{
			UnregisterConfigEvents();
			Log.Info("Unloaded " + VERSION);
		}
	}



	//Log
	internal static class Log
	{
		internal const string PREFIX = "[" + DisablePostProcessingEffectsPlugin.GUID + "] ";
		public static void Info(string message) { API.LogInfo(PREFIX + message); }
		public static void Warning(string message) { API.LogWarning(PREFIX + message); }
		public static void Error(string message) { API.LogError(PREFIX + message); }
	}
}
