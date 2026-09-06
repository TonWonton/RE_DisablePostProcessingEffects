using System.Diagnostics.CodeAnalysis;
using REFrameworkNET;
using REFrameworkNET.Attributes;
using via.render;
using app;


namespace RE_DisablePostProcessingEffects
{
	public partial class DisablePostProcessingEffectsPlugin
	{
		//Plugin
		public const string PLUGIN_NAME = "RE9_DisablePostProcessingEffects";
		public const string GUID = "RE9_DisablePostProcessingEffects";
		public const string VERSION = "2.1.0";



		//Var
		private static RenderingManager? _renderingManager;



		//Apply
		private static partial void ApplyGameSpecificNonPersistentSettings()
		{
			if (Initialized)
			{
				ToggleType toggleType = _filmGrain.Value;
				if (toggleType != ToggleType.Default)
				{
					_renderingManager._IsFilmGrainCustomFilterEnable = toggleType == ToggleType.Enable;
				}
			}
		}

		private static partial void ApplyGameSpecificPersistentSettings()
		{
			//NOP
		}



		//Hooks
		[MethodHook(typeof(EnvStageManager), nameof(EnvStageManager.OnChangeCurrentStage), MethodHookType.Post)]
		public static void PostOnChangeCurrentStage(ref ulong retVal)
		{
			ApplyPersistentSettings();
		}



		//Init
		private static partial bool TryGetAndCacheToneMapping(ref ToneMapping? toneMapping)
		{
			if (toneMapping != null) return true;
			else
			{
				if (Utility.TryGetAndCacheManagedSingleton(ref _renderingManager))
				{
					ToneMapping? thisToneMapping = _renderingManager._ToneMapping;
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
				if (Utility.TryGetAndCacheManagedSingleton(ref _renderingManager))
				{
					LDRPostProcess? thisLDRPostProcess = _renderingManager._LDRPostProcess;
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
				if (Utility.TryGetAndCacheManagedSingleton(ref _renderingManager))
				{
					VolumetricFogControl? thisVolumetricFogControl = _renderingManager._VolumetricFogControl;
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
