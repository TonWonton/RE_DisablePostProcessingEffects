--SCRIPT INFO
local s_GUID = "RE9_DisablePostProcessingEffects"
local s_version = "1.1.0"

local s_GUIDAndVVersion = s_GUID .. " v" .. s_version
local s_logPrefix = "[" .. s_GUID .. "] "
local s_configFileName = s_GUID .. ".lua.json"



--UTILITY
local function GenerateEnumNames(teEnum)
	local tblEnumNames = {}

	for name, value in pairs(teEnum) do
		tblEnumNames[value + 1] = name
	end

	return tblEnumNames
end


--LOG
local function LogInfo(message)
	log.info(s_logPrefix .. message)
end



--CONST
--Custom enums, hardcoded game enums, enum map
local te_ToggleType =
{
	Disable = 0,
	Default = 1,
	Enable = 2
}

local tbl_toggleTypeNames = GenerateEnumNames(te_ToggleType)

--AntiAliasingType
local te_AntiAliasingType =
{
	NONE = 0,
	DEFAULT = 1,
	FXAA = 2,
	TAA = 3,
	FXAA_TAA = 4,
	SMAA = 5
}

local tbl_antiAliasingTypeNames = GenerateEnumNames(te_AntiAliasingType)

local te_RenderConfigAntiAliasingType =
{
	FXAA = 0,
	TAA = 1,
	FXAA_TAA = 2,
	SMAA = 3,
	NONE = 4
}

local tbl_antiAliasingTypeToRenderConfigAntiAliasingType =
{
	[te_AntiAliasingType.NONE] = te_RenderConfigAntiAliasingType.NONE,
	[te_AntiAliasingType.DEFAULT] = te_RenderConfigAntiAliasingType.FXAA_TAA,
	[te_AntiAliasingType.FXAA] = te_RenderConfigAntiAliasingType.FXAA,
	[te_AntiAliasingType.TAA] = te_RenderConfigAntiAliasingType.TAA,
	[te_AntiAliasingType.FXAA_TAA] = te_RenderConfigAntiAliasingType.FXAA_TAA,
	[te_AntiAliasingType.SMAA] = te_RenderConfigAntiAliasingType.SMAA
}

--TemporalAA
local te_TemporalAA =
{
	Disable = 0,
	Default = 1,
	Legacy = 2,
	Manual = 3,
	Weak = 4,
	Mild = 5,
	Strong = 6
}

local tbl_temporalAANames = GenerateEnumNames(te_TemporalAA)

local te_ToneMappingTemporalAA =
{
	Legacy = 0,
	Manual = 1,
	Weak = 2,
	Mild = 3,
	Strong = 4,
	Disable = 5
}

local tbl_temporalAAToToneMappingTemporalAA =
{
	[te_TemporalAA.Disable] = te_ToneMappingTemporalAA.Disable,
	[te_TemporalAA.Default] = te_ToneMappingTemporalAA.Mild,
	[te_TemporalAA.Legacy] = te_ToneMappingTemporalAA.Legacy,
	[te_TemporalAA.Manual] = te_ToneMappingTemporalAA.Manual,
	[te_TemporalAA.Weak] = te_ToneMappingTemporalAA.Weak,
	[te_TemporalAA.Mild] = te_ToneMappingTemporalAA.Mild,
	[te_TemporalAA.Strong] = te_ToneMappingTemporalAA.Strong
}

--LocalExposureType
local te_ToneMappingLocalExposureType =
{
	Legacy = 0,
	BlurredLuminance = 1,
	LocalLaplacian = 2
}

local tbl_localExposureTypeNames = GenerateEnumNames(te_ToneMappingLocalExposureType)

--AutoExposure
local te_AutoExposure =
{
	Disable = 0,
	Default = 1,
	Enable = 2,
	FixedEnable = 3
}

local tbl_autoExposureNames = GenerateEnumNames(te_AutoExposure)

local te_ToneMappingAutoExposure =
{
	Enable = 0,
	FixedEnable = 1,
	Disable = 2
}

local tbl_autoExposureToToneMappingAutoExposure =
{
	[te_AutoExposure.Disable] = te_ToneMappingAutoExposure.Disable,
	[te_AutoExposure.Default] = te_ToneMappingAutoExposure.Enable,
	[te_AutoExposure.Enable] = te_ToneMappingAutoExposure.Enable,
	[te_AutoExposure.FixedEnable] = te_ToneMappingAutoExposure.FixedEnable
}

--Vignette
local te_Vignette =
{
	Disable = 0,
	Default = 1,
	Enable = 2,
	KerarePlus = 3,
	Anamorphic = 4,
	AnamorphicKerarePlus = 5
}

local tbl_vignetteNames = GenerateEnumNames(te_Vignette)

local te_ToneMappingVignette =
{
	Enable = 0,
	KerarePlus = 1,
	Disable = 2,
	Anamorphic = 3,
	AnamorphicKerarePlus = 4
}

local tbl_vignetteToToneMappingVignette =
{
	[te_Vignette.Disable] = te_ToneMappingVignette.Disable,
	[te_Vignette.Default] = te_ToneMappingVignette.Enable,
	[te_Vignette.Enable] = te_ToneMappingVignette.Enable,
	[te_Vignette.KerarePlus] = te_ToneMappingVignette.KerarePlus,
	[te_Vignette.Anamorphic] = te_ToneMappingVignette.Anamorphic,
	[te_Vignette.AnamorphicKerarePlus] = te_ToneMappingVignette.AnamorphicKerarePlus
}

--SharpnessType
local te_SharpnessType =
{
	Custom = 0,
	Default = 1,
	DefaultSharpness = 2,
	FidelityFXCAS = 3,
	NVIDIAImageScaling = 4,
	FidelityFXFSR1 = 5,
	TrinityPSSR = 6
}

local tbl_sharpnessTypeNames = GenerateEnumNames(te_SharpnessType)

local te_RenderConfigSharpnessType =
{
	Custom = 0,
	Default = 1,
	FidelityFXCAS = 2,
	NVIDIAImageScaling = 3,
	FidelityFXFSR1 = 4,
	TrinityPSSR = 5
}

local tbl_sharpnessTypeToRenderConfigSharpnessType =
{
	[te_SharpnessType.Custom] = te_RenderConfigSharpnessType.Custom,
	[te_SharpnessType.Default] = te_RenderConfigSharpnessType.Custom,
	[te_SharpnessType.DefaultSharpness] = te_RenderConfigSharpnessType.Default,
	[te_SharpnessType.FidelityFXCAS] = te_RenderConfigSharpnessType.FidelityFXCAS,
	[te_SharpnessType.NVIDIAImageScaling] = te_RenderConfigSharpnessType.NVIDIAImageScaling,
	[te_SharpnessType.FidelityFXFSR1] = te_RenderConfigSharpnessType.FidelityFXFSR1,
	[te_SharpnessType.TrinityPSSR] = te_RenderConfigSharpnessType.TrinityPSSR
}

--ReferenceLuminance
local te_ToneMappingReferenceLuminance =
{
	Maximum = 0,
	Average = 1,
	PercentileClippedAverage = 2
}

local tbl_referenceLuminanceNames = GenerateEnumNames(te_ToneMappingReferenceLuminance)



--CONFIG
local tbl_config =
{
	--int 1 is default
	i_antiAliasingType = 1,
	i_taa = 1,
	i_taaJitter = 1,
	i_colorCorrect = 1,
	i_vignette = 1,
	f_vignetteBrightness = 0.0,
	i_sharpnessType = 1,
	f_sharpness = 0.333,
	i_exposure = 1,
	f_ev = 2.0,
	i_localExposure = 1,
	i_localExposureType = 1,
	i_autoExposure = 1,
	f_autoExposureMinEV = 1.0,
	f_autoExposureMaxEV = 5.0,
	i_referenceLuminance = 1,
	b_customGamma = false,
	f_gamma = 1.0,
	b_customBrightness = false,
	f_minBrightness = 0.0,
	f_maxBrightness = 1.0,
	i_volumetricFog = 1,
	i_filmGrain = 1
}

local function LoadFromJson()
	local tblLoadedConfig = json.load_file(s_configFileName)

	if tblLoadedConfig ~= nil then
        for key, val in pairs(tblLoadedConfig) do
            tbl_config[key] = val
        end
    end
end

local function SaveToJson()
	json.dump_file(s_configFileName, tbl_config)
end



--VARIABLES
--Native
local no_renderer = nil
local td_renderer = nil
local no_displaySettings = nil
local td_displaySettings = nil

--Managed
local c_renderingManager = nil
local c_toneMapping = nil
local c_ldrPostProcess = nil
local c_ldrColorCorrect = nil
local c_ldrLensDistortion = nil
local c_ldrFilmGrain = nil
local c_volumetricFogControl = nil

--Lua
local b_initialized = false



--FUNCTIONS
local function ApplyNonPersistentSettings()
	local teToggleType = te_ToggleType
	local iToggleTypeDisable = teToggleType.Disable
	local iToggleTypeDefault = teToggleType.Default
	local iToggleTypeEnable = teToggleType.Enable
	local iToggleType = 1
	local tblConfig = tbl_config

	--Color correction
	local cLDRColorCorrect = c_ldrColorCorrect
	if cLDRColorCorrect ~= nil then
		iToggleType = tblConfig.i_colorCorrect
		if iToggleType ~= iToggleTypeDefault then
			cLDRColorCorrect:call("set_Enabled", iToggleType == iToggleTypeEnable)
		end
	end

	--ToneMapping
	local cToneMapping = c_toneMapping
	if cToneMapping ~= nil then
		--TAA
		local iTemporalAA = tblConfig.i_taa
		if iTemporalAA ~= te_TemporalAA.Default then
			local iToneMappingTemporalAA = tbl_temporalAAToToneMappingTemporalAA[iTemporalAA]
			if iToneMappingTemporalAA ~= nil then
				cToneMapping:call("setTemporalAA", iToneMappingTemporalAA)
			end
		end

		iToggleType = tblConfig.i_taaJitter
		if iToggleType ~= iToggleTypeDefault then
			cToneMapping:call("set_EchoEnabled", iToggleType == iToggleTypeEnable)
		end

		--Exposure
		iToggleType = tblConfig.i_exposure
		if iToggleType ~= iToggleTypeDefault then
			cToneMapping:call("set_ExposureEnable", iToggleType == iToggleTypeEnable)
			cToneMapping:call("set_EV", tblConfig.f_ev)
		end

		local iAutoExposure = tblConfig.i_autoExposure
		if iAutoExposure ~= te_AutoExposure.Default then
			local iToneMappingAutoExposure = tbl_autoExposureToToneMappingAutoExposure[iAutoExposure]
			if iToneMappingAutoExposure ~= nil then
				cToneMapping:call("setAutoExposure", iToneMappingAutoExposure)
				cToneMapping:call("set_AutoExposureMinEV", tblConfig.f_autoExposureMinEV)
				cToneMapping:call("set_AutoExposureMaxEV", tblConfig.f_autoExposureMaxEV)
				cToneMapping:call("set_ReferenceLuminance", tblConfig.i_referenceLuminance)
			end
		end

		iToggleType = tblConfig.i_localExposure
		if iToggleType ~= iToggleTypeDefault then
			cToneMapping:call("set_EnableLocalExposure", iToggleType == iToggleTypeEnable)
			cToneMapping:call("setLocalExposureType", tblConfig.i_localExposureType)
		end

		--Vignette
		local iVignette = tblConfig.i_vignette
		if iVignette ~= te_Vignette.Default then
			local iToneMappingVignette = tbl_vignetteToToneMappingVignette[iVignette]
			if iToneMappingVignette ~= nil then
				cToneMapping:call("setVignetting", iToneMappingVignette)
				cToneMapping:call("set_VignettingBrightness", tblConfig.f_vignetteBrightness)
			end
		end

		--Sharpening
		local iSharpnessType = tblConfig.i_sharpnessType
		if iSharpnessType ~= te_SharpnessType.Default then
			cToneMapping:call("set_Sharpness", tblConfig.f_sharpness)
		end
	end

	--Volumetric fog
	local cVolumetricFogControl = c_volumetricFogControl
	if cVolumetricFogControl ~= nil then
		iToggleType = tblConfig.i_volumetricFog
		if iToggleType ~= iToggleTypeDefault then
			cVolumetricFogControl:call("set_Enabled", iToggleType == iToggleTypeEnable)
		end
	end

	--Film grain
	local cRenderingManager = c_renderingManager
	if cRenderingManager ~= nil then
		iToggleType = tblConfig.i_filmGrain
		if iToggleType ~= iToggleTypeDefault then
			cRenderingManager:call("set__IsFilmGrainCustomFilterEnable(System.Boolean)", iToggleType == iToggleTypeEnable)
		end
	end
end

local function ApplyPersistentSettings()
	if b_initialized then
		local tblConfig = tbl_config
	
		local noRenderer = no_renderer
		if noRenderer ~= nil then
			--Get RenderConfig
			local cRenderConfig = sdk.call_native_func(noRenderer, td_renderer, "get_RenderConfig")
			if cRenderConfig ~= nil then
				--Anti-aliasing
				local iAntiAliasingType = tblConfig.i_antiAliasingType
				if iAntiAliasingType ~= te_AntiAliasingType.DEFAULT then
					local iRenderConfigAntiAliasingType = tbl_antiAliasingTypeToRenderConfigAntiAliasingType[iAntiAliasingType]
					if iRenderConfigAntiAliasingType ~= nil then
						cRenderConfig:call("set_AnitiAliasingSetting", iRenderConfigAntiAliasingType)
					end
				end

				--Sharpening
				local iSharpnessType = tblConfig.i_sharpnessType
				if iSharpnessType ~= te_SharpnessType.Default then
					local iRenderConfigSharpnessType = tbl_sharpnessTypeToRenderConfigSharpnessType[iSharpnessType]
					if iRenderConfigSharpnessType ~= nil then
						cRenderConfig:call("set_SharpnessSetting", iRenderConfigSharpnessType)
					end
				end
			end
		end

		local noDisplaySettings = no_displaySettings
		local tdDisplaySettings = td_displaySettings
		if noDisplaySettings ~= nil and tdDisplaySettings ~= nil then
			if tblConfig.b_customGamma then
				sdk.call_native_func(noDisplaySettings, tdDisplaySettings, "set_Gamma", tblConfig.f_gamma)
			end

			if tblConfig.b_customBrightness then
				sdk.call_native_func(noDisplaySettings, tdDisplaySettings, "set_OutputLowerLimit", tblConfig.f_minBrightness)
				sdk.call_native_func(noDisplaySettings, tdDisplaySettings, "set_OutputUpperLimit", tblConfig.f_maxBrightness)
			end
		end
	end
end

local function PreOnChangeCurrentStage(args)
end

local function PostOnChangeCurrentStage(retVal)
	ApplyPersistentSettings()
end

local function Initialize()
	if no_renderer == nil then no_renderer = sdk.get_native_singleton("via.render.Renderer") end
	if no_renderer == nil then return end

	if td_renderer == nil then td_renderer = sdk.find_type_definition("via.render.Renderer") end
	if td_renderer == nil then return end

	if no_displaySettings == nil then no_displaySettings = sdk.get_native_singleton("via.render.DisplaySettings") end
	if no_displaySettings == nil then return end

	if td_displaySettings == nil then td_displaySettings = sdk.find_type_definition("via.render.DisplaySettings") end
	if td_displaySettings == nil then return end

	if c_renderingManager == nil then c_renderingManager = sdk.get_managed_singleton("app.RenderingManager") end
	if c_renderingManager == nil then return end

	local cRenderingManager = c_renderingManager
	if c_toneMapping == nil then c_toneMapping = cRenderingManager._ToneMapping end
	if c_toneMapping == nil then return end

	if c_ldrPostProcess == nil then c_ldrPostProcess = cRenderingManager._LDRPostProcess end
	if c_ldrPostProcess == nil then return end

	local cLDRPostProcess = c_ldrPostProcess
	if c_ldrColorCorrect == nil then c_ldrColorCorrect = cLDRPostProcess:call("get_ColorCorrect") end
	if c_ldrColorCorrect == nil then return end

	if c_ldrLensDistortion == nil then c_ldrLensDistortion = cLDRPostProcess:call("get_LensDistortion") end
	if c_ldrLensDistortion == nil then return end

	if c_ldrFilmGrain == nil then c_ldrFilmGrain = cLDRPostProcess:call("get_FilmGrain") end
	if c_ldrFilmGrain == nil then return end

	if c_volumetricFogControl == nil then c_volumetricFogControl = cRenderingManager._VolumetricFogControl end
	if c_volumetricFogControl == nil then return end

	local tdEnvStageManager = sdk.find_type_definition("app.EnvStageManager")
	if tdEnvStageManager == nil then return end

	local mOnChangeCurrentStage = tdEnvStageManager:get_method("OnChangeCurrentStage")
	if mOnChangeCurrentStage == nil then return end

	sdk.hook(mOnChangeCurrentStage, PreOnChangeCurrentStage, PostOnChangeCurrentStage)
	b_initialized = true
	ApplyPersistentSettings()
	LogInfo("Initialized")
end



--CALLBACKS
local function PreBeginRendering()
	if b_initialized == false then
		Initialize()
		if b_initialized == false then return end
	end

	ApplyNonPersistentSettings()
end



--MAIN
LoadFromJson()
re.on_config_save(SaveToJson)
re.on_pre_application_entry("BeginRendering", PreBeginRendering)
LogInfo("Loaded " .. s_version)



--SCRIPT GENERATED UI
local function combo(label, currentValue, names)
    local changed, newValue = imgui.combo(label, currentValue + 1, names)
    return changed, newValue - 1
end

re.on_draw_ui(function()
	if imgui.tree_node(s_GUIDAndVVersion) then
		local bChanged = false
		local bAnyChanged = false
		local tblConfig = tbl_config

		local tblToggleTypeNames = tbl_toggleTypeNames

		local fIndent = 24.0

		--Anti-aliasing
		imgui.new_line()
		imgui.text("Anti-aliasing")
		imgui.separator()
		imgui.text("Note: Changing anti-aliasing type to DEFAULT requires changing the in-game anti-aliasing option or game restart to revert the changes.")
		bChanged, tblConfig.i_antiAliasingType = combo("Anti-aliasing type", tblConfig.i_antiAliasingType, tbl_antiAliasingTypeNames)
		bAnyChanged = bAnyChanged or bChanged

		imgui.spacing()
		imgui.indent(fIndent)
		imgui.text("TAA")
		imgui.separator()
		bChanged, tblConfig.i_taa = combo("TAA", tblConfig.i_taa, tbl_temporalAANames)
		bChanged, tblConfig.i_taaJitter = combo("TAA jitter", tblConfig.i_taaJitter, tblToggleTypeNames)
		imgui.unindent(fIndent)

		--Vignette
		imgui.new_line()
		imgui.text("Vignette")
		imgui.separator()
		bChanged, tblConfig.i_vignette = combo("Vignette", tblConfig.i_vignette, tbl_vignetteNames)
		local bIsVignetteDisabledOrDefault = tblConfig.i_vignette == 0 or tblConfig.i_vignette == 1
		imgui.begin_disabled(bIsVignetteDisabledOrDefault)
		bChanged, tblConfig.f_vignetteBrightness = imgui.drag_float("Vignette brightness", tblConfig.f_vignetteBrightness, 0.001, -1.0, 1.0)
		imgui.end_disabled()

		--Color
		imgui.new_line()
		imgui.text("Color")
		imgui.separator()
		bChanged, tblConfig.i_colorCorrect = combo("Color correction", tblConfig.i_colorCorrect, tblToggleTypeNames)

		--Sharpening
		imgui.new_line()
		imgui.text("Sharpening")
		imgui.separator()
		bChanged, tblConfig.i_sharpnessType = combo("Sharpness type", tblConfig.i_sharpnessType, tbl_sharpnessTypeNames)
		bAnyChanged = bAnyChanged or bChanged
		local isSharpeningDefault = tblConfig.i_sharpnessType == 1
		imgui.begin_disabled(isSharpeningDefault)
		bChanged, tblConfig.f_sharpness = imgui.drag_float("Sharpness", tblConfig.f_sharpness, 0.001, 0.0, 10.0)
		imgui.end_disabled()

		--Exposure
		imgui.new_line()
		imgui.text("Exposure")
		imgui.separator()
		bChanged, tblConfig.i_exposure = combo("Exposure", tblConfig.i_exposure, tblToggleTypeNames)
		local bIsExposureDisabledOrDefault = tblConfig.i_exposure == 0 or tblConfig.i_exposure == 1
		imgui.begin_disabled(bIsExposureDisabledOrDefault)
		bChanged, tblConfig.f_ev = imgui.drag_float("EV", tblConfig.f_ev, 0.001, -10.0, 10.0)
		imgui.end_disabled()
		
		imgui.spacing()
		imgui.indent(fIndent)
		imgui.text("Local exposure")
		imgui.separator()
		bChanged, tblConfig.i_localExposure = combo("Local exposure", tblConfig.i_localExposure, tblToggleTypeNames)
		local bIsLocalExposureDisabledOrDefault = tblConfig.i_localExposure == 0 or tblConfig.i_localExposure == 1
		imgui.begin_disabled(bIsLocalExposureDisabledOrDefault)
		bChanged, tblConfig.i_localExposureType = combo("Local exposure type", tblConfig.i_localExposureType, tbl_localExposureTypeNames)
		imgui.end_disabled()
		imgui.unindent(fIndent)

		imgui.spacing()
		imgui.indent(fIndent)
		imgui.text("Auto exposure")
		imgui.separator()
		bChanged, tblConfig.i_autoExposure = combo("Auto exposure", tblConfig.i_autoExposure, tbl_autoExposureNames)
		local bIsAutoExposureDisabledOrDefault = tblConfig.i_autoExposure == 0 or tblConfig.i_autoExposure == 1
		imgui.begin_disabled(bIsAutoExposureDisabledOrDefault)
		bChanged, tblConfig.i_referenceLuminance = combo("Reference luminance", tblConfig.i_referenceLuminance, tbl_referenceLuminanceNames)
		bChanged, tblConfig.f_autoExposureMinEV = imgui.drag_float("Auto exposure min EV", tblConfig.f_autoExposureMinEV, 0.001, -10.0, 10.0)
		bChanged, tblConfig.f_autoExposureMaxEV = imgui.drag_float("Auto exposure max EV", tblConfig.f_autoExposureMaxEV, 0.001, -10.0, 10.0)
		imgui.end_disabled()
		imgui.unindent(fIndent)

		--Gamma and brightness
		imgui.new_line()
		imgui.text("Gamma and brightness")
		imgui.separator()
		imgui.indent(fIndent)
		imgui.text("Note: Disabling custom gamma or brightness requires changing the in-game brightness options or game restart to revert the changes.")

		imgui.spacing()
		imgui.text("Gamma")
		imgui.separator()
		bChanged, tblConfig.b_customGamma = imgui.checkbox("Custom gamma", tblConfig.b_customGamma)
		bAnyChanged = bAnyChanged or bChanged
		imgui.begin_disabled(tblConfig.b_customGamma == false)
		bChanged, tblConfig.f_gamma = imgui.drag_float("Gamma", tblConfig.f_gamma, 0.001, 0.0, 5.0)
		bAnyChanged = bAnyChanged or bChanged
		imgui.end_disabled()

		imgui.spacing()
		imgui.text("Brightness")
		imgui.separator()
		bChanged, tblConfig.b_customBrightness = imgui.checkbox("Custom brightness", tblConfig.b_customBrightness)
		bAnyChanged = bAnyChanged or bChanged
		imgui.begin_disabled(tblConfig.b_customBrightness == false)
		bChanged, tblConfig.f_minBrightness = imgui.drag_float("Min brightness", tblConfig.f_minBrightness, 0.001, -10.0, 10.0)
		bAnyChanged = bAnyChanged or bChanged
		bChanged, tblConfig.f_maxBrightness = imgui.drag_float("Max brightness", tblConfig.f_maxBrightness, 0.001, -10.0, 10.0)
		bAnyChanged = bAnyChanged or bChanged
		imgui.end_disabled()
		imgui.unindent(fIndent)

		--Graphics settings
		imgui.new_line()
		imgui.text("Graphics settings")
		imgui.separator()
		imgui.text("Note: Changing film grain requires a game restart to apply the changes.")
		bChanged, tblConfig.i_volumetricFog = combo("Volumetric fog", tblConfig.i_volumetricFog, tblToggleTypeNames)
		bChanged, tblConfig.i_filmGrain = combo("Film grain", tblConfig.i_filmGrain, tblToggleTypeNames)
		imgui.new_line()

		if bAnyChanged then ApplyPersistentSettings() end

		imgui.tree_pop()
	end
end)