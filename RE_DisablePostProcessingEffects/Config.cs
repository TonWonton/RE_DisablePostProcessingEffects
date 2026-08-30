using System.Numerics;
using Hexa.NET.ImGui;
using REFrameworkNET.Callbacks;
using REFrameworkNET.Attributes;
using REFrameworkNET;
using REFrameworkNETPluginConfig;
using REFrameworkNETPluginConfig.Utility;
using LocalExposureType = via.render.ToneMapping.LocalExposureType;
using ReferenceLuminance = via.render.ToneMapping.AutoExposureReferenceLuminance;
#if MHWILDS
using ace;
#endif

namespace RE_DisablePostProcessingEffects
{
	public partial class DisablePostProcessingEffectsPlugin
	{
		//Config
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
		private static ConfigEntry<float> _vignetteBrightness = _config.Add("Vignette brightness",
#if MHWILDS
			1f
#else
			0f
#endif
		);

		//Sharpening
		private static ConfigEntry<SharpnessType> _sharpnessType = _config.Add("Sharpness type", SharpnessType.Default);
		private static ConfigEntry<float> _sharpness = _config.Add("Sharpness",
#if RE9
			0.333f
#elif PRAGMATA
			0.35f
#elif MHWILDS
			0f
#else
			default(float)
#endif
		);

		//Exposure
		private static ConfigEntry<ToggleType> _exposure = _config.Add("Exposure", ToggleType.Default);
		private static ConfigEntry<float> _ev = _config.Add("Exposure value (EV)",
#if RE9 || MHWILDS
			2f
#elif PRAGMATA
			1f
#else
			default(float)
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
#elif PRAGMATA
			3f
#elif MHWILDS
			-25f
#else
			default(float)
#endif
		);
		private static ConfigEntry<float> _autoExposureMaxEV = _config.Add("Auto exposure max EV",
#if MHWILDS
			25f
#else
			5f
#endif
		);
		private static ConfigEntry<ReferenceLuminance> _referenceLuminance = _config.Add("Reference luminance type", ReferenceLuminance.Average);

		//Contrast, gamma, and brightness
		private static ConfigEntry<bool> _customContrast = _config.Add("Custom contrast", false);
		private static ConfigEntry<float> _contrast = _config.Add("Contrast",
#if MHWILDS
			0.3f
#else
			1f
#endif
		);
		private static ConfigEntry<bool> _customGamma = _config.Add("Custom gamma", false);
		private static ConfigEntry<float> _gamma = _config.Add("Gamma", 1f);
		private static ConfigEntry<bool> _customBrightness = _config.Add("Custom brightness", false);
		private static ConfigEntry<float> _minBrightness = _config.Add("Min brightness", 0f);
		private static ConfigEntry<float> _maxBrightness = _config.Add("Max brightness", 1f);
		private static ConfigEntry<bool> _customUIGamma = _config.Add("Custom UI gamma", false);
		private static ConfigEntry<float> _uiGamma = _config.Add("UI gamma", 1f);
		private static ConfigEntry<bool> _customUIBrightness = _config.Add("Custom UI brightness", false);
		private static ConfigEntry<float> _minUIBrightness = _config.Add("Min UI brightness", 0f);
		private static ConfigEntry<float> _maxUIBrightness = _config.Add("Max UI brightness", 1f);

		//Graphics settings
		private static ConfigEntry<ToggleType> _volumetricFog = _config.Add("Volumetric fog", ToggleType.Default);
		private static ConfigEntry<ToggleType> _filmGrain = _config.Add("Film grain", ToggleType.Default);
		private static ConfigEntry<LensDistortionSetting> _lensDistortion = _config.Add("Lens distortion", LensDistortionSetting.DEFAULT);



		//Save
		//Only save config if _shouldSaveConfig == true and API.IsDrawingUI() == false || ImGui.TreeNode(GUID_VERSION) == false
		private static bool _shouldSaveConfig = false;
		private static void RequestSaveConfig()
		{
			_shouldSaveConfig = true;
		}

		private static void TrySaveConfig()
		{
			if (_shouldSaveConfig)
			{
				_shouldSaveConfig = false;
				_config.SaveToJson();
			}
		}



		//Plugin Generated UI
		private static readonly Vector4 _red = new Vector4(0.8f, 0f, 0f, 1f);

		[Callback(typeof(ImGuiDrawUI), CallbackType.Pre)]
		public static void ImGuiDrawUI_Pre()
		{
			if (API.IsDrawingUI() && ImGui.TreeNode(GUID_VERSION))
			{
				const float SLIDER_STEP_0p01 = 0.01f;
				const float SLIDER_STEP_0p001 = 0.001f;

				ToggleType toggleType;
				int labelNr = 0;
#if DEBUG
				//Debug
				ImGuiF.Category("Debug");
				if (ImGui.Button("Debug print values"))
				{
					Debug.TryDebugPrintValues();
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
				ImGui.Text("Note: Changing vignette to Default requires a game restart to revert the changes.");
				_vignette.Combo().ResetButton(ref labelNr).GetValue(out Vignette vignette);
				_vignetteBrightness.BeginDisabled(vignette == Vignette.Disable || vignette == Vignette.Default).DragFloat(SLIDER_STEP_0p001, -1f, 1f).EndDisabled().ResetButton(ref labelNr);

				//Color
				ImGuiF.Category("Color");
#if MHWILDS
				ImGui.Text("Note: If color correction is disabled it is recommended to enable custom contrast, brightness, and UI brightness and increase the values.");
#endif
				_colorCorrect.Combo().ResetButton(ref labelNr);

				//Sharpening
				ImGuiF.Category("Sharpening");
#if MHWILDS
				ImGui.Text("Note: Changing sharpening type to Default requires a game restart to revert the changes.");
#endif
				ImGui.Text("Note: The sharpening is often tied to the TAA. If it is not working as intended, try changing the TAA settings.");
				_sharpnessType.Combo().ResetButton(ref labelNr).GetValue(out SharpnessType sharpnessType);
				_sharpness.BeginDisabled(sharpnessType == SharpnessType.Default).DragFloat(SLIDER_STEP_0p001, 0f, 10f).EndDisabled().ResetButton(ref labelNr);

				//Exposure
				ImGuiF.Category("Exposure");
#if PRAGMATA
				ImGui.Text("Note: Changing exposure to Default requires a game restart to revert the changes.");
#endif
				_exposure.Combo().ResetButton(ref labelNr).GetValue(out toggleType);
				_ev.BeginDisabled(toggleType.IsDefaultOrDisabled()).DragFloat(SLIDER_STEP_0p001, -100f, 100f).EndDisabled().ResetButton(ref labelNr);

				ImGuiF.Category("Local exposure");
#if PRAGMATA || MHWILDS
				ImGui.Text("Note: Changing local exposure to Default requires a game restart to revert the changes.");
#endif
				_localExposure.Combo().ResetButton(ref labelNr).GetValue(out toggleType);
				_localExposureType.BeginDisabled(toggleType.IsDefaultOrDisabled()).Combo().EndDisabled().ResetButton(ref labelNr);

				ImGuiF.Category("Auto exposure");
				_autoExposure.Combo().ResetButton(ref labelNr).GetValue(out AutoExposure autoExposure);
				bool isAutoExposureBeginDisabled = autoExposure == AutoExposure.Disable || autoExposure == AutoExposure.Default;
				_referenceLuminance.BeginDisabled(isAutoExposureBeginDisabled).Combo().EndDisabled().ResetButton(ref labelNr);
				_autoExposureMinEV.BeginDisabled(isAutoExposureBeginDisabled).DragFloat(SLIDER_STEP_0p01, -100f, 100f).EndDisabled().ResetButton(ref labelNr);
				_autoExposureMaxEV.BeginDisabled(isAutoExposureBeginDisabled).DragFloat(SLIDER_STEP_0p01, -100f, 100f).EndDisabled().ResetButton(ref labelNr);

				//Contrast, gamma, and brightness
				ImGuiF.Category("Contrast, gamma, and brightness");
				ImGui.Indent(ImGuiF.IndentSize); ImGui.Text("Note: Disabling custom contrast requires a game restart to revert the changes."); ImGui.Unindent(ImGuiF.IndentSize);
				ImGui.Indent(ImGuiF.IndentSize); ImGui.Text("Note: Disabling custom gamma, brightness, UI gamma, or UI brightness requires changing the in-game brightness options or game restart to revert the changes."); ImGui.Unindent(ImGuiF.IndentSize);
				ImGui.Indent(ImGuiF.IndentSize); ImGui.TextColored(_red, "Note: Custom gamma, brightness, UI gamma, and UI brightness are not compatible with HDR. Use the in-game options instead."); ImGui.Unindent(ImGuiF.IndentSize);

				ImGuiF.SubCategory("Contrast");
				_customContrast.Checkbox().ResetButton(ref labelNr).GetValue(out bool customContrast);
				_contrast.BeginDisabled(!customContrast).DragFloat(SLIDER_STEP_0p001, -10f, 10f).EndDisabled().ResetButton(ref labelNr);
				ImGuiF.EndSubCategory();

				ImGuiF.SubCategory("Gamma");
				_customGamma.Checkbox().ResetButton(ref labelNr).GetValue(out bool customGamma);
				_gamma.BeginDisabled(!customGamma).DragFloat(SLIDER_STEP_0p001, 0f, 10f).EndDisabled().ResetButton(ref labelNr);
				ImGuiF.EndSubCategory();

				ImGuiF.SubCategory("Brightness");
				_customBrightness.Checkbox().ResetButton(ref labelNr).GetValue(out bool customBrightness);
				bool isBrightnessBeginDisabled = !customBrightness;
				_minBrightness.BeginDisabled(isBrightnessBeginDisabled).DragFloat(SLIDER_STEP_0p001, -10f, 10f).EndDisabled().ResetButton(ref labelNr);
				_maxBrightness.BeginDisabled(isBrightnessBeginDisabled).DragFloat(SLIDER_STEP_0p001, -10f, 10f).EndDisabled().ResetButton(ref labelNr);
				ImGuiF.EndSubCategory();

				ImGuiF.SubCategory("UI gamma");
				_customUIGamma.Checkbox().ResetButton(ref labelNr).GetValue(out bool customUIGamma);
				_uiGamma.BeginDisabled(!customUIGamma).DragFloat(SLIDER_STEP_0p001, 0f, 10f).EndDisabled().ResetButton(ref labelNr);
				ImGuiF.EndSubCategory();

				ImGuiF.SubCategory("UI brightness");
				_customUIBrightness.Checkbox().ResetButton(ref labelNr).GetValue(out bool customUIBrightness);
				bool isUIBrightnessBeginDisabled = !customUIBrightness;
				_minUIBrightness.BeginDisabled(isUIBrightnessBeginDisabled).DragFloat(SLIDER_STEP_0p001, -10f, 10f).EndDisabled().ResetButton(ref labelNr);
				_maxUIBrightness.BeginDisabled(isUIBrightnessBeginDisabled).DragFloat(SLIDER_STEP_0p001, -10f, 10f).EndDisabled().ResetButton(ref labelNr);
				ImGuiF.EndSubCategory();

				//Graphics settings
				ImGuiF.Category("Graphics settings");
				ImGui.Text("Note: Changing any of the graphics settings to Default requires a game restart to revert the changes.");
#if RE9
				ImGui.Text("Note: Changing film grain requires a game restart to apply the changes.");
#endif
				_volumetricFog.Combo().ResetButton(ref labelNr);
				_filmGrain.Combo().ResetButton(ref labelNr);
				_lensDistortion.Combo().ResetButton(ref labelNr);

				//Pop tree
				ImGui.TreePop();
			}
			else
			{
				//Try save config if not drawing UI or TreeNode
				TrySaveConfig();
			}
		}
	}
}
