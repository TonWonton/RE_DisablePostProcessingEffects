#nullable enable
using REFrameworkNET;
using REFrameworkNET.Attributes;
using app;
using via;
using via.render;


namespace RE_DisablePostProcessingEffects
{
	public partial class DisablePostProcessingEffectsPlugin
	{
		/* PLUGIN INFO */
		public const string PLUGIN_NAME = "PRAGMATA_DisablePostProcessingEffects";
		public const string GUID = "PRAGMATA_DisablePostProcessingEffects";
		public const string VERSION = "1.0.0";



		/* METHODS */
		private static partial void ApplyGameSpecificNonPersistentSettings()
		{

		}

		private static partial void ApplyGameSpecificPersistentSettings()
		{

		}



		/* HOOKS */
		[MethodHook(typeof(EnvironmentSceneManager), nameof(EnvironmentSceneManager.requestLoad), MethodHookType.Post)]
		public static void PostRequestLoad(ref ulong retVal)
		{
			ApplyPersistentSettings();
		}



		/* INITIALIZATION */
		private static partial bool TryInitializeGameSpecificVariables()
		{
			CameraSystem cameraSystem = API.GetManagedSingletonT<CameraSystem>();
			if (cameraSystem == null) return false;

			GameObject? cameraGameObject = cameraSystem.getCameraObject(CameraDefine.Role.Main);
			if (cameraGameObject == null) return false;

			_toneMapping = cameraGameObject.TryGetComponent<ToneMapping>(typeof(ToneMapping).FullName);
			if (_toneMapping == null) return false;

			_ldrPostProcess = cameraGameObject.TryGetComponent<LDRPostProcess>(typeof(LDRPostProcess).FullName);
			if (_ldrPostProcess == null) return false;

			_ldrColorCorrect = _ldrPostProcess.ColorCorrect;
			if (_ldrColorCorrect == null) return false;

			_ldrLensDistortion = _ldrPostProcess.LensDistortion;
			if (_ldrLensDistortion == null) return false;

			_ldrFilmGrain = _ldrPostProcess.FilmGrain;
			if (_ldrFilmGrain == null) return false;

			_volumetricFogControl = cameraGameObject.TryGetComponent<VolumetricFogControl>(typeof(VolumetricFogControl).FullName);
			if (_volumetricFogControl == null) return false;

			return true;
		}
	}
}
