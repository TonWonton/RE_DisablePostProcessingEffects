using System.Diagnostics.CodeAnalysis;
using REFrameworkNET;
using REFrameworkNET.Attributes;
using app;
using via;
using via.render;


namespace RE_DisablePostProcessingEffects
{
	public partial class DisablePostProcessingEffectsPlugin
	{
		//Plugin
		public const string PLUGIN_NAME = "PRAGMATA_DisablePostProcessingEffects";
		public const string GUID = "PRAGMATA_DisablePostProcessingEffects";
		public const string VERSION = "2.1.0";



		//Var
		private static CameraSystem? _cameraSystem;



		//Apply
		private static partial void ApplyGameSpecificNonPersistentSettings()
		{
			//NOP
		}

		private static partial void ApplyGameSpecificPersistentSettings()
		{
			//NOP
		}



		//Hooks
		[MethodHook(typeof(EnvironmentSceneManager), nameof(EnvironmentSceneManager.requestLoad), MethodHookType.Post)]
		public static void PostRequestLoad(ref ulong retVal)
		{
			ApplyPersistentSettings();
		}



		//Init
		private static partial bool TryGetAndCacheToneMapping(ref ToneMapping? toneMapping)
		{
			if (toneMapping != null) return true;
			else
			{
				if (Utility.TryGetCameraGameObject(ref _cameraSystem, out GameObject? cameraGameObject))
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
				if (Utility.TryGetCameraGameObject(ref _cameraSystem, out GameObject? cameraGameObject))
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
				if (Utility.TryGetCameraGameObject(ref _cameraSystem, out GameObject? cameraGameObject))
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
			//Nothing to initialize
			return true;
		}
	}
}
