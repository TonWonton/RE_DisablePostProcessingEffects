#nullable enable
using Hexa.NET.ImGui;
using REFrameworkNET.Callbacks;
using REFrameworkNET.Attributes;
using REFrameworkNET;
using REFrameworkNETPluginConfig;
using REFrameworkNETPluginConfig.Utility;
using LocalExposureType = via.render.ToneMapping.LocalExposureType;
using ReferenceLuminance = via.render.ToneMapping.AutoExposureReferenceLuminance;

namespace RE_DisablePostProcessingEffects
{
	public partial class DisablePostProcessingEffectsPlugin
	{
		/* CONFIG */
		private static Config _config = new Config(GUID);

		//Anti-aliasing
		private static ConfigEntry<AntiAliasingType> _antiAliasingType = _config.Add("Anti-aliasing type", AntiAliasingType.DEFAULT);
		private static ConfigEntry<TemporalAA> _taa = _config.Add("TAA", TemporalAA.Default);
		private static ConfigEntry<TemporalAAAlgorithm> _taaAlgorithm = _config.Add("TAA algorithm", TemporalAAAlgorithm.Default);
		private static ConfigEntry<ToggleType> _taaJitter = _config.Add("TAA jitter", ToggleType.Default);

		//Color
		private static ConfigEntry<ToggleType> _colorCorrect = _config.Add("Color correction", ToggleType.Default);

		//Vignette
		private static ConfigEntry<Vignette> _vignette = _config.Add("Vignette", Vignette.Default);
		private static ConfigEntry<float> _vignetteBrightness = _config.Add("Vignette brightness", 0f);

		//Sharpening
		private static ConfigEntry<SharpnessType> _sharpnessType = _config.Add("Sharpness type", SharpnessType.Default);
		private static ConfigEntry<float> _sharpness = _config.Add("Sharpness",
#if RE9
		0.333f
#endif
#if PRAGMATA
		0.35f
#endif
		);

		//Exposure
		private static ConfigEntry<ToggleType> _exposure = _config.Add("Exposure", ToggleType.Default);
		private static ConfigEntry<float> _ev = _config.Add("Exposure value (EV)",
#if RE9
			2f
#endif
#if PRAGMATA
			1f
#endif
		);

		//Local exposure
		private static ConfigEntry<ToggleType> _localExposure = _config.Add("Local exposure", ToggleType.Default);
		private static ConfigEntry<LocalExposureType> _localExposureType = _config.Add("Local exposure type", LocalExposureType.BlurredLuminance);

		//Auto exposure
		private static ConfigEntry<AutoExposure> _autoExposure = _config.Add("Auto exposure", AutoExposure.Default);
		private static ConfigEntry<float> _autoExposureMinEV = _config.Add("Auto exposure min EV",
#if RE9
		1f
#endif
#if PRAGMATA
		3f
#endif
		);
		private static ConfigEntry<float> _autoExposureMaxEV = _config.Add("Auto exposure max EV", 5f);
		private static ConfigEntry<ReferenceLuminance> _referenceLuminance = _config.Add("Reference luminance type", ReferenceLuminance.Average);

		//Gamma and brightness
		private static ConfigEntry<bool> _customGamma = _config.Add("Custom gamma", false);
		private static ConfigEntry<float> _gamma = _config.Add("Gamma", 1f);
		private static ConfigEntry<bool> _customBrightness = _config.Add("Custom brightness", false);
		private static ConfigEntry<float> _minBrightness = _config.Add("Min brightness", 0f);
		private static ConfigEntry<float> _maxBrightness = _config.Add("Max brightness", 1f);

		//Graphics settings
		private static ConfigEntry<ToggleType> _volumetricFog = _config.Add("Volumetric fog", ToggleType.Default);
		private static ConfigEntry<ToggleType> _filmGrain = _config.Add("Film grain", ToggleType.Default);



		/* PLUGIN GENERATED UI */
		[Callback(typeof(ImGuiDrawUI), CallbackType.Pre)]
		public static void PreImGuiDrawUI()
		{
			if (API.IsDrawingUI() && ImGui.TreeNode(GUID_VERSION))
			{
				const float SLIDER_STEP_0p001 = 0.001f;

				ToggleType toggleType;
				int labelNr = 0;

				//Debug
#if DEBUG
				ImGuiF.Category("Debug");
				if (ImGui.Button("Debug print values"))
				{
					Utility.TryDebugPrintValues();
				}
#endif

				//Anti-aliasing
				ImGuiF.Category("Anti-aliasing");
				ImGui.Text("Note: Changing anti-aliasing type to DEFAULT requires changing the in-game anti-aliasing option or game restart to revert the changes.");
				_antiAliasingType.Combo().ResetButton(ref labelNr);

				ImGuiF.SubCategory("TAA");
				_taa.Combo().ResetButton(ref labelNr);
				_taaAlgorithm.Combo().ResetButton(ref labelNr);
				_taaJitter.Combo().ResetButton(ref labelNr);
				ImGuiF.EndSubCategory();

				//Vignette
				ImGuiF.Category("Vignette");
				_vignette.Combo().ResetButton(ref labelNr).GetValue(out Vignette vignette);
				_vignetteBrightness.BeginDisabled(vignette == Vignette.Disable || vignette == Vignette.Default).DragFloat(SLIDER_STEP_0p001, -1f, 1f).EndDisabled().ResetButton(ref labelNr);

				//Color
				ImGuiF.Category("Color");
				_colorCorrect.Combo().ResetButton(ref labelNr);

				//Sharpening
				ImGuiF.Category("Sharpening");
				_sharpnessType.Combo().ResetButton(ref labelNr).GetValue(out SharpnessType sharpnessType);
				_sharpness.BeginDisabled(sharpnessType == SharpnessType.Default).DragFloat(SLIDER_STEP_0p001, 0f, 10f).EndDisabled().ResetButton(ref labelNr);

				//Exposure
				ImGuiF.Category("Exposure");
#if PRAGMATA
				ImGui.Text("Note: Changing exposure to Default requires a game restart to revert the changes.");
#endif
				_exposure.Combo().ResetButton(ref labelNr).GetValue(out toggleType);
				_ev.BeginDisabled(toggleType.IsDefaultOrDisabled()).DragFloat(SLIDER_STEP_0p001, -10f, 10f).EndDisabled().ResetButton(ref labelNr);

				ImGuiF.SubCategory("Local exposure");
#if PRAGMATA
				ImGui.Text("Note: Changing local exposure to Default requires a game restart to revert the changes.");
#endif
				_localExposure.Combo().ResetButton(ref labelNr).GetValue(out toggleType);
				_localExposureType.BeginDisabled(toggleType.IsDefaultOrDisabled()).Combo().EndDisabled().ResetButton(ref labelNr);
				ImGuiF.EndSubCategory();

				ImGuiF.SubCategory("Auto exposure");
				_autoExposure.Combo().ResetButton(ref labelNr).GetValue(out AutoExposure autoExposure);
				bool isAutoExposureBeginDisabled = autoExposure == AutoExposure.Disable || autoExposure == AutoExposure.Default;
				_referenceLuminance.BeginDisabled(isAutoExposureBeginDisabled).Combo().EndDisabled().ResetButton(ref labelNr);
				_autoExposureMinEV.BeginDisabled(isAutoExposureBeginDisabled).DragFloat(SLIDER_STEP_0p001, -10f, 10f).EndDisabled().ResetButton(ref labelNr);
				_autoExposureMaxEV.BeginDisabled(isAutoExposureBeginDisabled).DragFloat(SLIDER_STEP_0p001, -10f, 10f).EndDisabled().ResetButton(ref labelNr);
				ImGuiF.EndSubCategory();

				//Gamma and brightness
				ImGuiF.Category("Gamma and brightness");
				ImGui.Indent(ImGuiF.IndentSize); ImGui.Text("Note: Disabling custom gamma or brightness requires changing the in-game brightness options or game restart to revert the changes."); ImGui.Unindent(ImGuiF.IndentSize);
				ImGuiF.SubCategory("Gamma");
				_customGamma.Checkbox().ResetButton(ref labelNr).GetValue(out bool customGamma);
				_gamma.BeginDisabled(!customGamma).DragFloat(SLIDER_STEP_0p001, 0f, 5f).EndDisabled().ResetButton(ref labelNr);
				ImGuiF.EndSubCategory();

				ImGuiF.SubCategory("Brightness");
				_customBrightness.Checkbox().ResetButton(ref labelNr).GetValue(out bool customBrightness);
				bool isBrightnessBeginDisabled = !customBrightness;
				_minBrightness.BeginDisabled(isBrightnessBeginDisabled).DragFloat(SLIDER_STEP_0p001, -10f, 10f).EndDisabled().ResetButton(ref labelNr);
				_maxBrightness.BeginDisabled(isBrightnessBeginDisabled).DragFloat(SLIDER_STEP_0p001, -10f, 10f).EndDisabled().ResetButton(ref labelNr);
				ImGuiF.EndSubCategory();

				//Graphics settings
				ImGuiF.Category("GRAPHICS SETTINGS");
#if PRAGMATA
				ImGui.Text("Note: Changing volumetric fog to Default requires a game restart to revert the changes.");
#endif
				ImGui.Text("Note: Changing film grain requires a game restart to apply the changes.");
				_volumetricFog.Combo().ResetButton(ref labelNr);
				_filmGrain.Combo().ResetButton(ref labelNr);

				ImGui.TreePop();
			}
		}
	}
}