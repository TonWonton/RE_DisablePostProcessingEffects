#if DEBUG
using REFrameworkNET;
using via.render;


namespace RE_DisablePostProcessingEffects
{
	public partial class DisablePostProcessingEffectsPlugin
	{
		internal static class Utility
		{
			public static void TryDebugPrintValues()
			{
				try
				{
					Log.Info("Trying to debug print values...");
					
					ToneMapping toneMapping = _toneMapping;

					//Color correction
					Log.Info(string.Format("Color correct: {0}", _ldrColorCorrect.Enabled));

					//TAA
					Log.Info(string.Format("TAA: {0}", toneMapping.getTemporalAA()));
					Log.Info(string.Format("TAA algorithm: {0}", toneMapping.getTemporalAAAlgorithm()));
					Log.Info(string.Format("TAA jitter: {0}", toneMapping.EchoEnabled));

					//Exposure
					Log.Info(string.Format("Exposure: {0}", toneMapping.ExposureEnable));
					Log.Info(string.Format("EV: {0}", toneMapping.EV));

					//Auto exposure
					Log.Info(string.Format("Auto exposure: {0}", toneMapping.getAutoExposure()));
					Log.Info(string.Format("Auto exposure min EV: {0}", toneMapping.AutoExposureMinEV));
					Log.Info(string.Format("Auto exposure max EV: {0}", toneMapping.AutoExposureMaxEV));
					Log.Info(string.Format("Reference luminance: {0}", toneMapping.ReferenceLuminance));

					//Local exposure
					Log.Info(string.Format("Local exposure: {0}", toneMapping.EnableLocalExposure));
					Log.Info(string.Format("Local exposure type: {0}", toneMapping.getLocalExposureType()));

					//Vignette
					Log.Info(string.Format("Vignette: {0}", toneMapping.getVignetting()));
					Log.Info(string.Format("Vignette brightness: {0}", toneMapping.VignettingBrightness));

					//Sharpening
					Log.Info(string.Format("Sharpness: {0}", toneMapping.Sharpness));

					//Volumetric fog
					Log.Info(string.Format("Volumetric fog: {0}", _volumetricFogControl.Enabled));

					//RenderConfig (anti-aliasing, sharpness type)
					object result = _resultObject;
					_renderer.HandleInvokeMember_Internal(_getRenderConfigIndex, null, ref result);
					var renderConfig = ((ManagedObject)result).TryAs<RenderConfig>();
					if (renderConfig != null)
					{
						Log.Info(string.Format("Anti-aliasing type: {0}", renderConfig.AnitiAliasingSetting));
						Log.Info(string.Format("Sharpness type: {0}", renderConfig.SharpnessSetting));
					}
				}
				catch { }
			}
		}
	}
}
#endif