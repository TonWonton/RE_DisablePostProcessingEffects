#nullable enable
using REFrameworkNET;
using via;
using via.render;


namespace RE_DisablePostProcessingEffects
{
	internal static class Extensions
	{
		public static T? TryGetComponent<T>(this GameObject gameObject, string typeName) where T : class
		{
			_System.Type? type = _System.Type.GetType(typeName);
			if (type != null)
			{
				Component? componentFromGameObject = gameObject.getComponent(type);
				if (componentFromGameObject != null)
				{
					if (componentFromGameObject is IObject componentIObject)
					{
						return componentIObject.As<T>();
					}
				}
			}

			return null;
		}

		public static bool IsDefaultOrDisabled(this ToggleType toggleType)
		{
			return toggleType == ToggleType.Default || toggleType == ToggleType.Disable;
		}

		public static RenderConfig.AntiAliasingType AntiAliasingTypeToRenderConfigAntiAliasingType(this AntiAliasingType antiAliasingType)
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

		public static ToneMapping.TemporalAA TemporalAAToToneMappingTemporalAA(this TemporalAA temporalAA)
		{
			switch (temporalAA)
			{
				case TemporalAA.Disable: return ToneMapping.TemporalAA.Disable;
				case TemporalAA.Legacy: return ToneMapping.TemporalAA.Legacy;
				case TemporalAA.Manual: return ToneMapping.TemporalAA.Manual;
				case TemporalAA.Weak: return ToneMapping.TemporalAA.Weak;
				case TemporalAA.Mild: return ToneMapping.TemporalAA.Mild;
				case TemporalAA.Strong: return ToneMapping.TemporalAA.Strong;
				default: return ToneMapping.TemporalAA.Mild;
			}
		}

		public static ToneMapping.AutoExposure AutoExposureToToneMappingAutoExposure(this AutoExposure autoExposure)
		{
			switch (autoExposure)
			{
				case AutoExposure.Disable: return ToneMapping.AutoExposure.Disable;
				case AutoExposure.Enable: return ToneMapping.AutoExposure.Enable;
				case AutoExposure.FixedEnable: return ToneMapping.AutoExposure.FixedEnable;
				default: return ToneMapping.AutoExposure.Enable;
			}
		}

		public static ToneMapping.Vignetting VignetteToToneMappingVignetting(this Vignette vignette)
		{
			switch (vignette)
			{
				case Vignette.Disable: return ToneMapping.Vignetting.Disable;
				case Vignette.Enable: return ToneMapping.Vignetting.Enable;
				case Vignette.KerarePlus: return ToneMapping.Vignetting.KerarePlus;
				case Vignette.Anamorphic: return ToneMapping.Vignetting.Anamorphic;
				case Vignette.AnamorphicKerarePlus: return ToneMapping.Vignetting.AnamorphicKerarePlus;
				default: return ToneMapping.Vignetting.Enable;
			}
		}

		public static RenderConfig.SharpnessType SharpnessTypeToRenderConfigSharpnessType(this SharpnessType sharpnessType)
		{
			switch (sharpnessType)
			{
				case SharpnessType.Custom: return RenderConfig.SharpnessType.Custom;
				case SharpnessType.DefaultSharpness: return RenderConfig.SharpnessType.Default;
				case SharpnessType.FidelityFXCAS: return RenderConfig.SharpnessType.FidelityFXCAS;
				case SharpnessType.NVIDIAImageScaling: return RenderConfig.SharpnessType.NVIDIAImageScaling;
				case SharpnessType.FidelityFXFSR1: return RenderConfig.SharpnessType.FidelityFXFSR1;
				case SharpnessType.TrinityPSSR: return RenderConfig.SharpnessType.TrinityPSSR;
				default: return RenderConfig.SharpnessType.Custom;
			}
		}
	}
}