using System.Diagnostics.CodeAnalysis;
using REFrameworkNET;
using via;
using via.render;

namespace RE_DisablePostProcessingEffects
{
	internal static class EnumExtensions
	{
		internal static bool IsDefaultOrDisabled(this ToggleType toggleType)
		{
			return toggleType == ToggleType.Default || toggleType == ToggleType.Disable;
		}

		internal static RenderConfig.AntiAliasingType ToRenderConfigAntiAliasingType(this AntiAliasingType antiAliasingType)
		{
			switch (antiAliasingType)
			{
				case AntiAliasingType.NONE: return RenderConfig.AntiAliasingType.NONE;
				case AntiAliasingType.FXAA: return RenderConfig.AntiAliasingType.FXAA;
				case AntiAliasingType.TAA: return RenderConfig.AntiAliasingType.TAA;
				case AntiAliasingType.FXAA_TAA: return RenderConfig.AntiAliasingType.FXAA_TAA;
				case AntiAliasingType.SMAA: return RenderConfig.AntiAliasingType.SMAA;
				default: return RenderConfig.AntiAliasingType.FXAA_TAA;
			}
		}

		internal static ToneMapping.TemporalAA ToToneMappingTemporalAA(this TemporalAA temporalAA)
		{
			switch (temporalAA)
			{
				case TemporalAA.Disable: return ToneMapping.TemporalAA.Disable;
				case TemporalAA.Legacy: return ToneMapping.TemporalAA.Legacy;
				case TemporalAA.Manual: return ToneMapping.TemporalAA.Manual;
				case TemporalAA.Weak: return ToneMapping.TemporalAA.Weak;
				case TemporalAA.Mild: return ToneMapping.TemporalAA.Mild;
				case TemporalAA.Strong: return ToneMapping.TemporalAA.Strong;
				default: return ToneMapping.TemporalAA.Manual;
			}
		}

		internal static ToneMapping.TemporalAAAlgorithm ToToneMappingTemporalAAAlgorithm(this TemporalAAAlgorithm temporalAAAlgorithm)
		{
			switch (temporalAAAlgorithm)
			{
				case TemporalAAAlgorithm.Legacy: return ToneMapping.TemporalAAAlgorithm.Legacy;
				case TemporalAAAlgorithm.Standard: return ToneMapping.TemporalAAAlgorithm.Standard;
				case TemporalAAAlgorithm.StandardV2: return ToneMapping.TemporalAAAlgorithm.StandardV2;
				case TemporalAAAlgorithm.StandardV3: return ToneMapping.TemporalAAAlgorithm.StandardV3;
				case TemporalAAAlgorithm.Sharp: return ToneMapping.TemporalAAAlgorithm.Sharp;
				default: return ToneMapping.TemporalAAAlgorithm.StandardV2;
			}
		}

		internal static ToneMapping.LocalExposureType ToToneMappingLocalExposureType(this LocalExposureType localExposureType)
		{
			switch (localExposureType)
			{
				case LocalExposureType.Legacy: return ToneMapping.LocalExposureType.Legacy;
				case LocalExposureType.BlurredLuminance: return ToneMapping.LocalExposureType.BlurredLuminance;
#if !MHWILDS
				case LocalExposureType.LocalLaplacian: return ToneMapping.LocalExposureType.LocalLaplacian;
#endif
				default: return ToneMapping.LocalExposureType.BlurredLuminance;
			}
		}

		internal static ToneMapping.AutoExposure ToToneMappingAutoExposure(this AutoExposure autoExposure)
		{
			switch (autoExposure)
			{
				case AutoExposure.Disable: return ToneMapping.AutoExposure.Disable;
				case AutoExposure.Enable: return ToneMapping.AutoExposure.Enable;
				case AutoExposure.FixedEnable: return ToneMapping.AutoExposure.FixedEnable;
				default: return ToneMapping.AutoExposure.Enable;
			}
		}

		internal static ToneMapping.AutoExposureReferenceLuminance ToToneMappingReferenceLuminance(this ReferenceLuminance referenceLuminance)
		{
			switch (referenceLuminance)
			{
				case ReferenceLuminance.Maximum: return ToneMapping.AutoExposureReferenceLuminance.Maximum;
				case ReferenceLuminance.Average: return ToneMapping.AutoExposureReferenceLuminance.Average;
				case ReferenceLuminance.PercentileClippedAverage: return ToneMapping.AutoExposureReferenceLuminance.PercentileClippedAverage;
				default: return ToneMapping.AutoExposureReferenceLuminance.Average;
			}
		}

		internal static ToneMapping.Vignetting ToToneMappingVignetting(this Vignette vignette)
		{
			switch (vignette)
			{
				case Vignette.Disable: return ToneMapping.Vignetting.Disable;
				case Vignette.Enable: return ToneMapping.Vignetting.Enable;
				case Vignette.KerarePlus: return ToneMapping.Vignetting.KerarePlus;
				case Vignette.Anamorphic: return ToneMapping.Vignetting.Anamorphic;
				case Vignette.AnamorphicKerarePlus: return ToneMapping.Vignetting.AnamorphicKerarePlus;
#if RE9
				default: return ToneMapping.Vignetting.Enable;
#elif MHWILDS
				default: return ToneMapping.Vignetting.AnamorphicKerarePlus;
#else
				default: return ToneMapping.Vignetting.Disable;
#endif
			}
		}

		internal static RenderConfig.SharpnessType ToRenderConfigSharpnessType(this SharpnessType sharpnessType)
		{
			switch (sharpnessType)
			{
				case SharpnessType.Custom: return RenderConfig.SharpnessType.Custom;
				case SharpnessType.DefaultSharpness: return RenderConfig.SharpnessType.Default;
				case SharpnessType.FidelityFXCAS: return RenderConfig.SharpnessType.FidelityFXCAS;
				case SharpnessType.NVIDIAImageScaling: return RenderConfig.SharpnessType.NVIDIAImageScaling;
				case SharpnessType.FidelityFXFSR1: return RenderConfig.SharpnessType.FidelityFXFSR1;
				case SharpnessType.TrinityPSSR: return RenderConfig.SharpnessType.TrinityPSSR;
#if MHWILDS
				default: return RenderConfig.SharpnessType.FidelityFXFSR1;
#else
				default: return RenderConfig.SharpnessType.Custom;
#endif
			}
		}

		internal static RenderConfig.LensDistortionSetting ToRenderConfigLensDistortionSetting(this LensDistortionSetting lensDistortionSetting)
		{
			switch (lensDistortionSetting)
			{
				case LensDistortionSetting.OFF: return RenderConfig.LensDistortionSetting.OFF;
				case LensDistortionSetting.ON: return RenderConfig.LensDistortionSetting.ON;
				case LensDistortionSetting.DistortionOnly: return RenderConfig.LensDistortionSetting.DistortionOnly;
#if MHWILDS
				default: return RenderConfig.LensDistortionSetting.ON;
#else
				default: return RenderConfig.LensDistortionSetting.OFF;
#endif
			}
		}
	}
}
