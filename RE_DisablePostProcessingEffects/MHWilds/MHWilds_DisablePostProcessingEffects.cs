using app;
using REFrameworkNET;
using REFrameworkNET.Attributes;
using via;
using via.render;

namespace RE_DisablePostProcessingEffects
{
	public partial class DisablePostProcessingEffectsPlugin
	{
		//Plugin
		public const string PLUGIN_NAME = "MHWilds_DisablePostProcessingEffects";
		public const string GUID = "MHWilds_DisablePostProcessingEffects";
		public const string VERSION = "2.1.0";



		//Var
		private static CameraManager? _cameraManager;
		private static GraphicsManager? _graphicsManager;



		//Apply
		private static partial void ApplyGameSpecificNonPersistentSettings()
		{
			//NOP
		}

		private static partial void ApplyGameSpecificPersistentSettings()
		{
			if (Initialized)
			{
				GraphicsManager graphicsManager = _graphicsManager;
				cAppGraphicsSetting nowGraphicsSetting = graphicsManager.NowGraphicsSetting;
				ToggleType toggleType;
				bool changed = false;

				toggleType = _filmGrain.Value;
				if (toggleType != ToggleType.Default)
				{
					nowGraphicsSetting.FilmGrain_Enable = toggleType == ToggleType.Enable;
					changed = true;
				}

				LensDistortionSetting lensDistortionSetting = _lensDistortion.Value;
				if (lensDistortionSetting != LensDistortionSetting.DEFAULT)
				{
					nowGraphicsSetting.LensDistortionSetting = lensDistortionSetting.ToRenderConfigLensDistortionSetting();
					changed = true;
				}

				if (changed)
				{
					graphicsManager.setGraphicsSetting(nowGraphicsSetting);
				}
			}
		}



		//Hooks
		[MethodHook(typeof(CameraManager), nameof(CameraManager.onSceneLoadFadeIn), MethodHookType.Post)]
		public static void onSceneLoadFadeIn_Post(ref ulong retVal)
		{
			ApplyPersistentSettings();
		}



		//Init
		private static partial bool TryGetAndCacheToneMapping(ref ToneMapping? toneMapping)
		{
			if (toneMapping != null) return true;
			else
			{
				if (Utility.TryGetCameraGameObject(ref _cameraManager, out GameObject? cameraGameObject))
				{
					ToneMapping? thisToneMapping = cameraGameObject.TryGetComponent<ToneMapping>();
					if (thisToneMapping != null)
					{
						toneMapping = thisToneMapping;
						return true;
					}
				}
			}

			return false;
		}

		private static partial bool TryGetAndCacheLDRPostProcess(ref LDRPostProcess? ldrPostProcess)
		{
			if (ldrPostProcess != null) return true;
			else
			{
				if (Utility.TryGetCameraGameObject(ref _cameraManager, out GameObject? cameraGameObject))
				{
					LDRPostProcess? thisLDRPostProcess = cameraGameObject.TryGetComponent<LDRPostProcess>();
					if (thisLDRPostProcess != null)
					{
						ldrPostProcess = thisLDRPostProcess;
						return true;
					}
				}
			}

			return false;
		}

		private static partial bool TryGetAndCacheVolumetricFogControl(ref VolumetricFogControl? volumetricFogControl)
		{
			if (volumetricFogControl != null) return true;
			else
			{
				if (Utility.TryGetCameraGameObject(ref _cameraManager, out GameObject? cameraGameObject))
				{
					VolumetricFogControl? thisVolumetricFogControl = cameraGameObject.TryGetComponent<VolumetricFogControl>();
					if (thisVolumetricFogControl != null)
					{
						volumetricFogControl = thisVolumetricFogControl;
						return true;
					}
				}
			}

			return false;
		}

		private static partial bool TryInitializeGameSpecific()
		{
			if (Utility.TryGetAndCacheManagedSingleton(ref _graphicsManager) == false) return false;

			return true;
		}
	}
}
