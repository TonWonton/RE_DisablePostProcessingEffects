#nullable enable
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
		/*PLUGIN INFO*/
		public const string COPYRIGHT = "";
		public const string COMPANY = "https://github.com/TonWonton/RE_DisablePostProcessingEffects";
		public const string GUID_VERSION = GUID + " v" + VERSION;



		/* VARIABLES */
		//Native
		private static NativeObject? _renderer;
		private static uint _getRenderConfigIndex;
		private static NativeObject? _displaySettings;
		private static uint _setGammaIndex, _setOutputLowerLimitIndex, _setOutputUpperLimitIndex;

		//Managed
		private static ToneMapping? _toneMapping;
		private static LDRPostProcess? _ldrPostProcess;
		private static LDRColorCorrect? _ldrColorCorrect;
		private static LDRLensDistortion? _ldrLensDistortion;
		private static LDRFilmGrain? _ldrFilmGrain;
		private static VolumetricFogControl? _volumetricFogControl;


		//Variables
		private static readonly object _resultObject = new object();
		private static readonly object[] _boolObject = new object[1];
		private static readonly object[] _floatObject = new object[1];

		//Properties
		private static bool _initialized = false;

		[MemberNotNullWhen(true,
		nameof(_renderer),
		nameof(_displaySettings),
		nameof(_toneMapping),
		nameof(_ldrPostProcess),
		nameof(_ldrColorCorrect),
		nameof(_ldrLensDistortion),
		nameof(_ldrFilmGrain),
		nameof(_volumetricFogControl))]
		public static bool Initialized { get { return _initialized; } }



		/* METHODS */
		private static partial void ApplyGameSpecificNonPersistentSettings();
		private static void ApplyNonPersistentSettings()
		{
			if (Initialized)
			{
				ToggleType toggleType;

				//Color correction
				toggleType = _colorCorrect.Value;
				if (toggleType != ToggleType.Default)
				{
					_ldrColorCorrect.Enabled = toggleType == ToggleType.Enable;
				}

				ToneMapping toneMapping = _toneMapping;
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
#if !PRAGMATA
					toneMapping.ExposureEnable = toggleType == ToggleType.Enable;
#endif
					toneMapping.EV = _ev.Value;
				}

				AutoExposure autoExposure = _autoExposure.Value;
				if (autoExposure != AutoExposure.Default)
				{
					toneMapping.setAutoExposure(autoExposure.ToToneMappingAutoExposure());
					toneMapping.AutoExposureMinEV = _autoExposureMinEV.Value;
					toneMapping.AutoExposureMaxEV = _autoExposureMaxEV.Value;
					toneMapping.ReferenceLuminance = _referenceLuminance.Value;
				}

#if !PRAGMATA
				toggleType = _localExposure.Value;
				if (toggleType != ToggleType.Default)
				{
					toneMapping.EnableLocalExposure = toggleType == ToggleType.Enable;
					toneMapping.setLocalExposureType(_localExposureType.Value);
				}
#endif

				//Vignette
				Vignette vignette = _vignette.Value;
				if (vignette != Vignette.Default)
				{
					toneMapping.setVignetting(vignette.ToToneMappingVignetting());
					toneMapping.VignettingBrightness = _vignetteBrightness.Value;
				}

				//Sharpening
				SharpnessType sharpnessType = _sharpnessType.Value;
				if (sharpnessType != SharpnessType.Default)
				{
					toneMapping.Sharpness = _sharpness.Value;
				}

				//Volumetric fog
				VolumetricFogControl volumetricFogControl = _volumetricFogControl;
				toggleType = _volumetricFog.Value;
				if (toggleType != ToggleType.Default)
				{
					volumetricFogControl.Enabled = toggleType == ToggleType.Enable;
				}

#if !PRAGMATA
				ApplyGameSpecificNonPersistentSettings();
#endif
			}
		}

		private static partial void ApplyGameSpecificPersistentSettings();
		private static void ApplyPersistentSettings()
		{
			if (Initialized)
			{
#if PRAGMATA
				ToggleType toggleType;
				ToneMapping toneMapping = _toneMapping;

				toggleType = _exposure.Value;
				if (toggleType != ToggleType.Default)
				{
					toneMapping.ExposureEnable = toggleType == ToggleType.Enable;
				}

				toggleType = _localExposure.Value;
				if (toggleType != ToggleType.Default)
				{
					toneMapping.EnableLocalExposure = toggleType == ToggleType.Enable;
					toneMapping.setLocalExposureType(_localExposureType.Value);
				}
#endif

				object result = _resultObject;
				object[] boolObject = _boolObject;
				object[] floatObject = _floatObject;

				NativeObject renderer = _renderer;
				renderer.HandleInvokeMember_Internal(_getRenderConfigIndex, null, ref result);
				var renderConfig = ((ManagedObject)result).TryAs<RenderConfig>();
				if (renderConfig != null)
				{
					//Anti-aliasing
					AntiAliasingType antiAliasingType = _antiAliasingType.Value;
					if (antiAliasingType != AntiAliasingType.DEFAULT)
					{
						renderConfig.AnitiAliasingSetting = antiAliasingType.ToRenderConfigAntiAliasingType();
					}

					//Sharpening
					SharpnessType sharpnessType = _sharpnessType.Value;
					if (sharpnessType != SharpnessType.Default)
					{
						renderConfig.SharpnessSetting = sharpnessType.ToRenderConfigSharpnessType();
					}
				}

				NativeObject displaySettings = _displaySettings;
				//Gamma
				if (_customGamma.Value)
				{
					floatObject[0] = _gamma.Value;
					displaySettings.HandleInvokeMember_Internal(_setGammaIndex, floatObject, ref result);
				}

				//Brightness
				if (_customBrightness.Value)
				{
					floatObject[0] = _minBrightness.Value;
					displaySettings.HandleInvokeMember_Internal(_setOutputLowerLimitIndex, floatObject, ref result);

					floatObject[0] = _maxBrightness.Value;
					displaySettings.HandleInvokeMember_Internal(_setOutputUpperLimitIndex, floatObject, ref result);
				}

#if RE9 || PRAGMATA

#else
				ApplyGameSpecificNonPersistentSettings();
#endif
			}
		}

		[Callback(typeof(BeginRendering), CallbackType.Pre)]
		public static void PreBeginRendering()
		{
			if (_initialized == false)
			{
				Initialize();
				return;
			}

			ApplyNonPersistentSettings();
		}

		private static partial bool TryInitializeGameSpecificVariables();
		private static void Initialize()
		{
			//Renderer
			if (_renderer == null) _renderer = API.GetNativeSingleton(typeof(Renderer).FullName);
			if (_renderer == null) return;

			var rendererTypeDef = _renderer.GetTypeDefinition();
			if (rendererTypeDef == null) return;
			_getRenderConfigIndex = rendererTypeDef.FindMethod("get_RenderConfig").GetIndex();

			//DisplaySettings
			if (_displaySettings == null) _displaySettings = API.GetNativeSingleton(typeof(DisplaySettings).FullName);
			if (_displaySettings == null) return;

			var displaySettingsTypeDef = _displaySettings.GetTypeDefinition();
			if (displaySettingsTypeDef == null) return;
			_setGammaIndex = displaySettingsTypeDef.FindMethod("set_Gamma").GetIndex();
			_setOutputLowerLimitIndex = displaySettingsTypeDef.FindMethod("set_OutputLowerLimit").GetIndex();
			_setOutputUpperLimitIndex = displaySettingsTypeDef.FindMethod("set_OutputUpperLimit").GetIndex();

			//Initialize game specific variables
			if (TryInitializeGameSpecificVariables())
			{
#if DEBUG
				Utility.TryDebugPrintValues();
#endif
				_initialized = true;
				ApplyPersistentSettings();
				Log.Info("Initialization successful");
				//Log.Info("Default TAAAlgorithm: " + _toneMapping!.getTemporalAAAlgorithm());
			}
		}



		/* EVENT HANDLING */
		private static void OnSettingsChanged()
		{
			_config.SaveToJson();
		}



		/* INITIALIZATION */
		[PluginEntryPoint]
		private static void Load()
		{
			RegisterConfigEvents();
			_config.LoadFromJson();
			Log.Info("Loaded " + VERSION);
		}

		[PluginExitPoint]
		private static void Unload()
		{
			UnregisterConfigEvents();
			Log.Info("Unloaded " + VERSION);
		}

		private static void RegisterConfigEvents()
		{
			foreach (ConfigEntryBase configEntry in _config.Values)
			{
				configEntry.ValueChanged += OnSettingsChanged;
			}

			_antiAliasingType.ValueChanged += ApplyPersistentSettings;
			_sharpnessType.ValueChanged += ApplyPersistentSettings;

			_customGamma.ValueChanged += ApplyPersistentSettings;
			_gamma.ValueChanged += ApplyPersistentSettings;
			_customBrightness.ValueChanged += ApplyPersistentSettings;
			_minBrightness.ValueChanged += ApplyPersistentSettings;
			_maxBrightness.ValueChanged += ApplyPersistentSettings;

#if PRAGMATA
			_exposure.ValueChanged += ApplyPersistentSettings;
			_ev.ValueChanged += ApplyPersistentSettings;
			_localExposure.ValueChanged += ApplyPersistentSettings;
			_localExposureType.ValueChanged += ApplyPersistentSettings;
#endif
		}

		private static void UnregisterConfigEvents()
		{
			foreach (ConfigEntryBase configEntry in _config.Values)
			{
				configEntry.ValueChanged -= OnSettingsChanged;
			}

			_antiAliasingType.ValueChanged -= ApplyPersistentSettings;
			_sharpnessType.ValueChanged -= ApplyPersistentSettings;

			_customGamma.ValueChanged -= ApplyPersistentSettings;
			_gamma.ValueChanged -= ApplyPersistentSettings;
			_customBrightness.ValueChanged -= ApplyPersistentSettings;
			_minBrightness.ValueChanged -= ApplyPersistentSettings;
			_maxBrightness.ValueChanged -= ApplyPersistentSettings;

#if PRAGMATA
			_exposure.ValueChanged -= ApplyPersistentSettings;
			_ev.ValueChanged -= ApplyPersistentSettings;
			_localExposure.ValueChanged -= ApplyPersistentSettings;
			_localExposureType.ValueChanged -= ApplyPersistentSettings;
#endif
		}
	}

	internal static class Log
	{
		internal const string PREFIX = "[" + DisablePostProcessingEffectsPlugin.GUID + "] ";
		public static void Info(string message) { API.LogInfo(PREFIX + message); }
		public static void Warning(string message) { API.LogWarning(PREFIX + message); }
		public static void Error(string message) { API.LogError(PREFIX + message); }
	}
}