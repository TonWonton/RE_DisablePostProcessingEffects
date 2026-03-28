#nullable enable
using REFrameworkNET;
using app;
using REFrameworkNET.Attributes;


namespace RE_DisablePostProcessingEffects
{
	public partial class DisablePostProcessingEffectsPlugin
	{
		/* PLUGIN INFO */
		public const string PLUGIN_NAME = "RE9_DisablePostProcessingEffects";
		public const string GUID = "RE9_DisablePostProcessingEffects";
		public const string VERSION = "1.1.0";



		/* VARIABLES */
		private static RenderingManager? _renderingManager;



		/* METHODS */
		private static partial void ApplyGameSpecificNonPersistentSettings()
		{
			RenderingManager? renderingManager = _renderingManager;
			if (renderingManager != null)
			{
				ToggleType toggleType = _filmGrain.Value;
				if (toggleType != ToggleType.Default)
				{
					renderingManager._IsFilmGrainCustomFilterEnable = toggleType == ToggleType.Enable;
				}
			}
		}

		private static partial void ApplyGameSpecificPersistentSettings()
		{

		}



		/* HOOKS */
		[MethodHook(typeof(EnvStageManager), nameof(EnvStageManager.OnChangeCurrentStage), MethodHookType.Post)]
		public static void PostEnvStageManagerOnChangeCurrentStage(ref ulong retVal)
		{
			ApplyPersistentSettings();
		}



		/* INITIALIZATION */
		private static partial bool TryInitializeGameSpecificVariables()
		{
			if (_renderingManager == null) _renderingManager = API.GetManagedSingletonT<RenderingManager>();
			if (_renderingManager == null) return false;

			_toneMapping = _renderingManager._ToneMapping;
			if (_toneMapping == null) return false;

			_ldrPostProcess = _renderingManager._LDRPostProcess;
			if (_ldrPostProcess == null) return false;

			_ldrColorCorrect = _ldrPostProcess.ColorCorrect;
			if (_ldrColorCorrect == null) return false;

			_ldrLensDistortion = _ldrPostProcess.LensDistortion;
			if (_ldrLensDistortion == null) return false;
			
			_ldrFilmGrain = _ldrPostProcess.FilmGrain;
			if (_ldrFilmGrain == null) return false;

			_volumetricFogControl = _renderingManager._VolumetricFogControl;
			if (_volumetricFogControl == null) return false;

			return true;
		}
	}
}