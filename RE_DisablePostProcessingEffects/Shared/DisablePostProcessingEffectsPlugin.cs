#nullable enable
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
		//Singletons
		private static NativeObject? _displaySettings;
		private static uint _setGammaIndex, _setOutputLowerLimitIndex, _setOutputUpperLimitIndex;
		private static NativeObject? _renderer;
		private static uint _getRenderConfigIndex;

		//References
		private static ToneMapping? _toneMapping;
		private static LDRPostProcess? _ldrPostProcess;
		private static LDRColorCorrect? _ldrColorCorrect;
		private static LDRLensDistortion? _ldrLensDistortion;
		private static LDRFilmGrain? _ldrFilmGrain;
		private static VolumetricFogControl? _volumetricFogControl;


		//Variables
		private static readonly object[] _floatObject = new object[1];
		private static readonly object[] _boolObject = new object[1];
		private static readonly object _resultObject = new object();

		private static bool _initialized = false;



		/* METHODS */
		private static partial void ApplyGameSpecificNonPersistentSettings();
		private static void ApplyNonPersistentSettings()
		{
			ToggleType toggleType;

			//Color correction
			LDRColorCorrect? lDRColorCorrect = _ldrColorCorrect;
			if (lDRColorCorrect != null)
			{
				toggleType = _colorCorrect.Value;
				if (toggleType != ToggleType.Default)
				{
					lDRColorCorrect.Enabled = toggleType == ToggleType.Enable;
				}
			}

			//ToneMapping
			ToneMapping? toneMapping = _toneMapping;
			if (toneMapping != null)
			{
				//TAA
				TemporalAA taa = _taa.Value;
				if (taa != TemporalAA.Default)
				{
					toneMapping.setTemporalAA(taa.TemporalAAToToneMappingTemporalAA());
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

				AutoExposure autoExposure = _autoExposure.Value;
				if (autoExposure != AutoExposure.Default)
				{
					toneMapping.setAutoExposure(autoExposure.AutoExposureToToneMappingAutoExposure());
					toneMapping.AutoExposureMinEV = _autoExposureMinEV.Value;
					toneMapping.AutoExposureMaxEV = _autoExposureMaxEV.Value;
					toneMapping.ReferenceLuminance = _referenceLuminance.Value;
				}

				toggleType = _localExposure.Value;
				if (toggleType != ToggleType.Default)
				{
					toneMapping.EnableLocalExposure = toggleType == ToggleType.Enable;
					toneMapping.setLocalExposureType(_localExposureType.Value);
				}

				//Vignette
				Vignette vignette = _vignette.Value;
				if (vignette != Vignette.Default)
				{
					toneMapping.setVignetting(vignette.VignetteToToneMappingVignetting());
					toneMapping.VignettingBrightness = _vignetteBrightness.Value;
				}

				//Sharpening
				SharpnessType sharpnessType = _sharpnessType.Value;
				if (sharpnessType != SharpnessType.Default)
				{
					toneMapping.Sharpness = _sharpness.Value;
				}
			}

			//Volumetric fog
			VolumetricFogControl? volumetricFogControl = _volumetricFogControl;
			if (volumetricFogControl != null)
			{
				toggleType = _volumetricFog.Value;
				if (toggleType != ToggleType.Default)
				{
					volumetricFogControl.Enabled = toggleType == ToggleType.Enable;
				}
			}

			ApplyGameSpecificNonPersistentSettings();
		}

		private static partial void ApplyGameSpecificPersistentSettings();
		private static void ApplyPersistentSettings()
		{
			if (_initialized)
			{
				NativeObject? renderer = _renderer;
				if (renderer != null)
				{
					object result = _resultObject;

					renderer.HandleInvokeMember_Internal(_getRenderConfigIndex, null, ref result);
					var renderConfig = ((ManagedObject)result).TryAs<RenderConfig>();
					if (renderConfig != null)
					{
						//Anti-aliasing
						AntiAliasingType antiAliasingType = _antiAliasingType.Value;
						if (antiAliasingType != AntiAliasingType.DEFAULT)
						{
							renderConfig.AnitiAliasingSetting = antiAliasingType.AntiAliasingTypeToRenderConfigAntiAliasingType();
						}

						//Sharpening
						SharpnessType sharpnessType = _sharpnessType.Value;
						if (sharpnessType != SharpnessType.Default)
						{
							renderConfig.SharpnessSetting = sharpnessType.SharpnessTypeToRenderConfigSharpnessType();
						}
					}
				}

				NativeObject? displaySettings = _displaySettings;
				if (displaySettings != null)
				{
					object result = _resultObject;
					object[] floatObject = _floatObject;

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
				}

				ApplyGameSpecificPersistentSettings();
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
		}

		private static partial bool TryInitializeGameSpecificVariables();
		private static void Initialize()
		{
			//Renderer
			if (_renderer == null) _renderer = API.GetNativeSingleton("via.render.Renderer");
			if (_renderer == null) return;

			var rendererTypeDef = _renderer.GetTypeDefinition();
			if (rendererTypeDef == null) return;
			_getRenderConfigIndex = rendererTypeDef.FindMethod("get_RenderConfig").GetIndex();

			//DisplaySettings
			if (_displaySettings == null) _displaySettings = API.GetNativeSingleton("via.render.DisplaySettings");
			if (_displaySettings == null) return;

			var displaySettingsTypeDef = _displaySettings.GetTypeDefinition();
			if (displaySettingsTypeDef == null) return;
			_setGammaIndex = displaySettingsTypeDef.FindMethod("set_Gamma").GetIndex();
			_setOutputLowerLimitIndex = displaySettingsTypeDef.FindMethod("set_OutputLowerLimit").GetIndex();
			_setOutputUpperLimitIndex = displaySettingsTypeDef.FindMethod("set_OutputUpperLimit").GetIndex();

			//Initialize game specific variables
			if (TryInitializeGameSpecificVariables())
			{
				_initialized = true;
				ApplyPersistentSettings();
				Log.Info("Initialization successful");
			}
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