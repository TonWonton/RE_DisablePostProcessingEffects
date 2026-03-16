#nullable enable


namespace RE_DisablePostProcessingEffects
{
	public enum ToggleType
	{
		Disable = 0,
		Default = 1,
		Enable
	}

	public enum AntiAliasingType
	{
		NONE = 0,
		DEFAULT = 1,
		FXAA,
		TAA,
		FXAA_TAA,
		SMAA
	}

	public enum TemporalAA
	{
		Disable = 0,
		Default = 1,
		Legacy,
		Manual,
		Weak,
		Mild,
		Strong
	}

	public enum AutoExposure
	{
		Disable = 0,
		Default = 1,
		Enable,
		FixedEnable
	}

	public enum Vignette
	{
		Disable = 0,
		Default = 1,
		Enable,
		KerarePlus,
		Anamorphic,
		AnamorphicKerarePlus
	}

	public enum SharpnessType
	{
		Default = 1,
		Custom,
		DefaultSharpness,
		FidelityFXCAS,
		NVIDIAImageScaling,
		FidelityFXFSR1,
		TrinityPSSR
	}
}