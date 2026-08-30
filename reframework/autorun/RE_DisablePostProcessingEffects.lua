--1. Get game info
--log.info("[RE_DisablePostProcessingEffects] Game name: " .. reframework:get_game_name())
local _stringGameName = reframework:get_game_name()

local function IsRE9() return _stringGameName == "re9" end
local function IsPRAGMATA() return _stringGameName == "pragmata" end
local function IsMHWILDS() return _stringGameName == "mhwilds" end

local RE9 = IsRE9()
local PRAGMATA = IsPRAGMATA()
local MHWILDS = IsMHWILDS()





--2. Get script info
local function GetGUID()
	if RE9 then return "RE9_DisablePostProcessingEffects" end
	if PRAGMATA then return "PRAGMATA_DisablePostProcessingEffects" end
	if MHWILDS then return "MHWilds_DisablePostProcessingEffects" end

	return nil
end
local _stringGUID = GetGUID()

local function GetVersion()
	if RE9 then return "2.0.0" end
	if PRAGMATA then return "2.0.0" end
	if MHWILDS then return "2.0.0" end

	return nil
end
local _stringVersion = GetVersion()

local _stringGUIDVersion = _stringGUID .. " v" .. _stringVersion





--Log
local _stringLogPrefix = "[" .. _stringGUID .. "] "
local _stringConfigFileName = _stringGUID .. ".lua.json"

local function LogInfo(stringArg)
	log.info(_stringLogPrefix .. stringArg)
end





--3. Config
local function GetConfigTable()
	local classTableConfig = {}

	classTableConfig.iAntiAliasingType = 1
	classTableConfig.iTAA = 1
	classTableConfig.iTemporalAAAlgorithm = 1
	classTableConfig.iTAAJitter = 1
	classTableConfig.iColorCorrection = 1
	classTableConfig.iVignette = 1
	if MHWILDS then classTableConfig.fVignetteBrightness = 1.0
	else classTableConfig.fVignetteBrightness = 0.0 end
	classTableConfig.iSharpnessType = 1
	if RE9 then classTableConfig.fSharpness = 0.333
	elseif PRAGMATA then classTableConfig.fSharpness = 0.35
	elseif MHWILDS then classTableConfig.fSharpness = 0.0
	else classTableConfig.fSharpness = 0.0 end
	classTableConfig.iExposure = 1
	if RE9 or MHWILDS then classTableConfig.fEV = 2.0
	elseif PRAGMATA then classTableConfig.fEV = 1.0
	else classTableConfig.fEV = 0.0 end
	classTableConfig.iLocalExposure = 1
	classTableConfig.iLocalExposureType = 1
	classTableConfig.iAutoExposure = 1
	if RE9 then classTableConfig.fAutoExposureMinEV = 1.0
	elseif PRAGMATA then classTableConfig.fAutoExposureMinEV = 3.0
	elseif MHWILDS then classTableConfig.fAutoExposureMinEV = -25.0
	else classTableConfig.fAutoExposureMinEV = 0.0 end
	if MHWILDS then classTableConfig.fAutoExposureMaxEV = 25.0
	else classTableConfig.fAutoExposureMaxEV = 5.0 end
	classTableConfig.iReferenceLuminanceType = 1
	classTableConfig.bCustomContrast = false
	if MHWILDS then classTableConfig.fContrast = 0.3
	else classTableConfig.fContrast = 1.0 end
	classTableConfig.bCustomGamma = false
	classTableConfig.fGamma = 1.0
	classTableConfig.bCustomBrightness = false
	classTableConfig.fMinBrightness = 0.0
	classTableConfig.fMaxBrightness = 1.0
	classTableConfig.bCustomUIGamma = false
	classTableConfig.fUIGamma = 1.0
	classTableConfig.bCustomUIBrightness = false
	classTableConfig.fMinUIBrightness = 0.0
	classTableConfig.fMaxUIBrightness = 1.0
	classTableConfig.iVolumetricFog = 1
	classTableConfig.iFilmGrain = 1
	classTableConfig.iLensDistortion = 1

	return classTableConfig
end
local _classTableConfig = GetConfigTable()

local function LoadFromJson()
	local classTableConfigLoaded = json.load_file(_stringConfigFileName)
	local classTableConfig = _classTableConfig

	if classTableConfigLoaded ~= nil then
		for key, value in pairs(classTableConfigLoaded) do
			classTableConfig[key] = value
		end
	end
end
local function SaveToJson()
	json.dump_file(_stringConfigFileName, _classTableConfig)
end





--4. Enum
local function GenerateEnumNames(enumArg)
	local enumNames = {}

	for name, value in pairs(enumArg) do
		enumNames[value + 1] = name
	end

	return enumNames
end

local _enumToggleType =
{
	Disable = 0,
	Default = 1,
	Enable = 2
}
local _enumToggleTypeNames = GenerateEnumNames(_enumToggleType)

local _enumAntiAliasingType =
{
	NONE = 0,
	DEFAULT = 1,
	FXAA = 2,
	TAA = 3,
	FXAA_TAA = 4,
	SMAA = 5
}
local _enumAntiAliasingTypeNames = GenerateEnumNames(_enumAntiAliasingType)
local _toRenderConfigAntiAliasingType =
{
	[_enumAntiAliasingType.NONE] = 4,
	[_enumAntiAliasingType.DEFAULT] = 2,
	[_enumAntiAliasingType.FXAA] = 0,
	[_enumAntiAliasingType.TAA] = 1,
	[_enumAntiAliasingType.FXAA_TAA] = 2,
	[_enumAntiAliasingType.SMAA] = 3
}

local _enumTemporalAA =
{
	Disable = 0,
	Default = 1,
	Legacy = 2,
	Manual = 3,
	Weak = 4,
	Mild = 5,
	Strong = 6
}
local _enumTemporalAANames = GenerateEnumNames(_enumTemporalAA)
local _toToneMappingTemporalAA =
{
	[_enumTemporalAA.Disable] = 5,
	[_enumTemporalAA.Default] = 1,
	[_enumTemporalAA.Legacy] = 0,
	[_enumTemporalAA.Manual] = 1,
	[_enumTemporalAA.Weak] = 2,
	[_enumTemporalAA.Mild] = 3,
	[_enumTemporalAA.Strong] = 4
}

local _enumTemporalAAAlgorithm =
{
	Legacy = 0,
	Default = 1,
	Standard = 2,
	StandardV2 = 3,
	StandardV3 = 4,
	Sharp = 5
}
local _enumTemporalAAAlgorithmNames = GenerateEnumNames(_enumTemporalAAAlgorithm)
local _toToneMappingTemporalAAAlgorithm =
{
	[_enumTemporalAAAlgorithm.Default] = 2,
	[_enumTemporalAAAlgorithm.Legacy] = 0,
	[_enumTemporalAAAlgorithm.Standard] = 1,
	[_enumTemporalAAAlgorithm.StandardV2] = 2,
	[_enumTemporalAAAlgorithm.StandardV3] = 3,
	[_enumTemporalAAAlgorithm.Sharp] = 4
}

local function GetLocalExposureTypeEnum()
	local enumLocalExposureType = {}

	enumLocalExposureType.Legacy = 0
	enumLocalExposureType.BlurredLuminance = 1
	if MHWILDS == false then enumLocalExposureType.LocalLaplacian = 2 end

	return enumLocalExposureType
end
local _enumLocalExposureType = GetLocalExposureTypeEnum()
local _enumLocalExposureTypeNames = GenerateEnumNames(_enumLocalExposureType)

local _enumAutoExposure =
{
	Disable = 0,
	Default = 1,
	Enable = 2,
	FixedEnable = 3
}
local _enumAutoExposureNames = GenerateEnumNames(_enumAutoExposure)
local _toToneMappingAutoExposure =
{
	[_enumAutoExposure.Disable] = 2,
	[_enumAutoExposure.Default] = 0,
	[_enumAutoExposure.Enable] = 0,
	[_enumAutoExposure.FixedEnable] = 1
}

local _enumReferenceLuminanceType =
{
	Maximum = 0,
	Average = 1,
	PercentileClippedAverage = 2
}
local _enumReferenceLuminanceTypeNames = GenerateEnumNames(_enumReferenceLuminanceType)

local _enumVignette =
{
	Disable = 0,
	Default = 1,
	Enable = 2,
	KerarePlus = 3,
	Anamorphic = 4,
	AnamorphicKerarePlus = 5
}
local _enumVignetteNames = GenerateEnumNames(_enumVignette)
local function GetVignetteEnumMap()
	local toToneMappingVignetting = {}

	toToneMappingVignetting[_enumVignette.Disable] = 2
	if RE9 then toToneMappingVignetting[_enumVignette.Default] = 0
	elseif MHWILDS then toToneMappingVignetting[_enumVignette.Default] = 4
	else toToneMappingVignetting[_enumVignette.Default] = 2 end
	toToneMappingVignetting[_enumVignette.Enable] = 0
	toToneMappingVignetting[_enumVignette.KerarePlus] = 1
	toToneMappingVignetting[_enumVignette.Anamorphic] = 3
	toToneMappingVignetting[_enumVignette.AnamorphicKerarePlus] = 4

	return toToneMappingVignetting
end
local _toToneMappingVignetting = GetVignetteEnumMap()

local _enumSharpnessType =
{
	Custom = 0,
	Default = 1,
	DefaultSharpness = 2,
	FidelityFXCAS = 3,
	NVIDIAImageScaling = 4,
	FidelityFXFSR1 = 5,
	TrinityPSSR = 6
}
local _enumSharpnessTypeNames = GenerateEnumNames(_enumSharpnessType)
local function GetSharpnessTypeEnumMap()
	local toRenderConfigSharpnessType = {}

	if MHWILDS then toRenderConfigSharpnessType[_enumSharpnessType.Default] = 4
	else toRenderConfigSharpnessType[_enumSharpnessType.Default] = 0 end
	toRenderConfigSharpnessType[_enumSharpnessType.Custom] = 0
	toRenderConfigSharpnessType[_enumSharpnessType.DefaultSharpness] = 1
	toRenderConfigSharpnessType[_enumSharpnessType.FidelityFXCAS] = 2
	toRenderConfigSharpnessType[_enumSharpnessType.NVIDIAImageScaling] = 3
	toRenderConfigSharpnessType[_enumSharpnessType.FidelityFXFSR1] = 4
	toRenderConfigSharpnessType[_enumSharpnessType.TrinityPSSR] = 5

	return toRenderConfigSharpnessType
end
local _toRenderConfigSharpnessType = GetSharpnessTypeEnumMap()

local _enumLensDistortionSetting =
{
	OFF = 0,
	DEFAULT = 1,
	ON = 2,
	DistortionOnly = 3
}
local _enumLensDistortionSettingNames = GenerateEnumNames(_enumLensDistortionSetting)
local function GetLensDistortionSettingEnumMap()
	local toRenderConfigLensDistortionSetting = {}

	toRenderConfigLensDistortionSetting[_enumLensDistortionSetting.OFF] = 2
	if MHWILDS then toRenderConfigLensDistortionSetting[_enumLensDistortionSetting.DEFAULT] = 0
	else toRenderConfigLensDistortionSetting[_enumLensDistortionSetting.DEFAULT] = 2 end
	toRenderConfigLensDistortionSetting[_enumLensDistortionSetting.ON] = 0
	toRenderConfigLensDistortionSetting[_enumLensDistortionSetting.DistortionOnly] = 1

	return toRenderConfigLensDistortionSetting
end
local _toRenderConfigLensDistortionSetting = GetLensDistortionSettingEnumMap()





--5. Var
local function TryGetComponent(classGameObject, classStringFullTypeName)
	local classTypeOf = sdk.typeof(classStringFullTypeName)

	if classTypeOf ~= nil then
		return classGameObject:call("getComponent(System.Type)", classTypeOf)
	end

	return nil
end

local function TryGetComponentFromGameObject(classGameObject, classStringFullTypeName)
	if classGameObject ~= nil then
		local classComponent = TryGetComponent(classGameObject, classStringFullTypeName)
		if classComponent ~= nil then
			return classComponent
		end
	end

	return nil
end

--Renderer
local _classNativeObjectRenderer = nil
local _classTypeDefinitionRenderer = nil

local function TryGetAndCacheRenderer()
	if _classNativeObjectRenderer ~= nil then return true
	else
		local classNativeObject = sdk.get_native_singleton("via.render.Renderer")
		if classNativeObject ~= nil then
			_classNativeObjectRenderer = classNativeObject
			return true
		end
	end

	return false
end

local function TryGetAndCacheRendererTypeDefinition()
	if _classTypeDefinitionRenderer ~= nil then return true
	else
		local classTypeDefinition = sdk.find_type_definition("via.render.Renderer")
		if classTypeDefinition ~= nil then
			_classTypeDefinitionRenderer = classTypeDefinition
			return true
		end
	end

	return false
end

--DisplaySettings
local _classNativeObjectDisplaySettings = nil
local _classTypeDefinitionDisplaySettings = nil

local function TryGetAndCacheDisplaySettings()
	if _classNativeObjectDisplaySettings ~= nil then return true
	else
		local classNativeObject = sdk.get_native_singleton("via.render.DisplaySettings")
		if classNativeObject ~= nil then
			_classNativeObjectDisplaySettings = classNativeObject
			return true
		end
	end

	return false
end

local function TryGetAndCacheDisplaySettingsTypeDefinition()
	if _classTypeDefinitionDisplaySettings ~= nil then return true
	else
		local classTypeDefinition = sdk.find_type_definition("via.render.DisplaySettings")
		if classTypeDefinition ~= nil then
			_classTypeDefinitionDisplaySettings = classTypeDefinition
			return true
		end
	end

	return false
end

--RenderingManager
local _classRenderingManager = nil
local _classTypeDefinitionEnvStageManager = nil

local function TryGetAndCacheRenderingManager_RE9()
	if _classRenderingManager ~= nil then return true
	else
		local classRenderingManager = sdk.get_managed_singleton("app.RenderingManager")
		if classRenderingManager ~= nil then
			_classRenderingManager = classRenderingManager
			return true
		end
	end

	return false
end

local function TryGetAndCacheEnvStageManagerTypeDefinition_RE9()
	if _classTypeDefinitionEnvStageManager ~= nil then return true
	else
		local classTypeDefinition = sdk.find_type_definition("app.EnvStageManager")
		if classTypeDefinition ~= nil then
			_classTypeDefinitionEnvStageManager = classTypeDefinition
			return true
		end
	end

	return false
end

--CameraSystem
local _classCameraSystem = nil
local _classTypeDefinitionEnvironmentSceneManager = nil

local function TryGetAndCacheCameraSystem_PRAGMATA()
	if _classCameraSystem ~= nil then return true
	else
		local classCameraSystem = sdk.get_managed_singleton("app.CameraSystem")
		if classCameraSystem ~= nil then
			_classCameraSystem = classCameraSystem
			return true
		end
	end

	return false
end

local function TryGetAndCacheEnvironmentSceneManagerTypeDefinition_PRAGMATA()
	if _classTypeDefinitionEnvironmentSceneManager ~= nil then return true
	else
		local classTypeDefinition = sdk.find_type_definition("app.EnvironmentSceneManager")
		if classTypeDefinition ~= nil then
			_classTypeDefinitionEnvironmentSceneManager = classTypeDefinition
			return true
		end
	end

	return false
end

--CameraManager
local _classCameraManager = nil
local _classTypeDefinitionCameraManager = nil

local function TryGetAndCacheCameraManager_MHWilds()
	if _classCameraManager ~= nil then return true
	else
		local classCameraManager = sdk.get_managed_singleton("app.CameraManager")
		if classCameraManager ~= nil then
			_classCameraManager = classCameraManager
			return true
		end
	end

	return false
end

local function TryGetAndCacheCameraManagerTypeDefinition_MHWilds()
	if _classTypeDefinitionCameraManager ~= nil then return true
	else
		local classTypeDefinition = sdk.find_type_definition("app.CameraManager")
		if classTypeDefinition ~= nil then
			_classTypeDefinitionCameraManager = classTypeDefinition
			return true
		end
	end

	return false
end

--Camera GameObject
local function TryGetCameraGameObject_PRAGMATA()
	if TryGetAndCacheCameraSystem_PRAGMATA() then
		local classCameraGameObject = _classCameraSystem:call("getCameraObject", 0)
		if classCameraGameObject ~= nil then
			return classCameraGameObject
		end
	end

	return nil
end

local function TryGetCameraGameObject_MHWilds()
	if TryGetAndCacheCameraManager_MHWilds() then
		local classCameraPrimary = _classCameraManager:call("get_PrimaryCamera")
		if classCameraPrimary ~= nil then
			local classCameraGameObject = classCameraPrimary:call("get_GameObject")
			if classCameraGameObject ~= nil then
				return classCameraGameObject
			end
		end
	end

	return nil
end

--GraphicsManager
local _classGraphicsManager = nil

local function TryGetAndCacheGraphicsManager_MHWilds()
	if _classGraphicsManager ~= nil then return true
	else
		local classGraphicsManager = sdk.get_managed_singleton("app.GraphicsManager")
		if classGraphicsManager ~= nil then
			_classGraphicsManager = classGraphicsManager
			return true
		end
	end

	return false
end

--Managed
local _classRenderConfig = nil
local _classToneMapping = nil
local _classLDRPostProcess = nil
local _classLDRColorCorrect = nil
local _classLDRLensDistortion = nil
local _classLDRFilmGrain = nil
local _classVolumetricFogControl = nil

local function TryGetAndCacheRenderConfig()
	if _classRenderConfig ~= nil then return true
	else
		if TryGetAndCacheRenderer() then
			local classRenderConfig = sdk.call_native_func(_classNativeObjectRenderer, _classTypeDefinitionRenderer, "get_RenderConfig")
			if classRenderConfig ~= nil then
				_classRenderConfig = classRenderConfig
				return true
			end
		end
	end

	return false
end

local function TryGetAndCacheToneMapping()
	if _classToneMapping ~= nil then return true
	else
		if RE9 then
			if TryGetAndCacheRenderingManager_RE9() then
				local classToneMapping_RE9 = _classRenderingManager._ToneMapping
				if classToneMapping_RE9 ~= nil then
					_classToneMapping = classToneMapping_RE9
					return true
				end
			end

			return false
		end

		if PRAGMATA then
			local classToneMapping_PRAGMATA = TryGetComponentFromGameObject(TryGetCameraGameObject_PRAGMATA(), "via.render.ToneMapping")
			if classToneMapping_PRAGMATA ~= nil then
				_classToneMapping = classToneMapping_PRAGMATA
				return true
			end

			return false
		end

		if MHWILDS then
			local classToneMapping_MHWilds = TryGetComponentFromGameObject(TryGetCameraGameObject_MHWilds(), "via.render.ToneMapping")
			if classToneMapping_MHWilds ~= nil then
				_classToneMapping = classToneMapping_MHWilds
				return true
			end

			return false
		end
	end

	return false
end
local function TryGetAndCacheLDRPostProcess()
	if _classLDRPostProcess ~= nil then return true
	else
		if RE9 then
			if TryGetAndCacheRenderingManager_RE9() then
				local classLDRPostProcess_RE9 = _classRenderingManager._LDRPostProcess
				if classLDRPostProcess_RE9 ~= nil then
					_classLDRPostProcess = classLDRPostProcess_RE9
					return true
				end
			end

			return false
		end

		if PRAGMATA then
			local classLDRPostProcess_PRAGMATA = TryGetComponentFromGameObject(TryGetCameraGameObject_PRAGMATA(), "via.render.LDRPostProcess")
			if classLDRPostProcess_PRAGMATA ~= nil then
				_classLDRPostProcess = classLDRPostProcess_PRAGMATA
				return true
			end

			return false
		end

		if MHWILDS then
			local classLDRPostProcess_MHWilds = TryGetComponentFromGameObject(TryGetCameraGameObject_MHWilds(), "via.render.LDRPostProcess")
			if classLDRPostProcess_MHWilds ~= nil then
				_classLDRPostProcess = classLDRPostProcess_MHWilds
				return true
			end

			return false
		end
	end

	return false
end
local function TryGetAndCacheVolumetricFogControl()
	if _classVolumetricFogControl ~= nil then return true
	else
		if RE9 then
			if TryGetAndCacheRenderingManager_RE9() then
				local classVolumetricFogControl_RE9 = _classRenderingManager._VolumetricFogControl
				if classVolumetricFogControl_RE9 ~= nil then
					_classVolumetricFogControl = classVolumetricFogControl_RE9
					return true
				end
			end

			return false
		end

		if PRAGMATA then
			local classVolumetricFogControl_PRAGMATA = TryGetComponentFromGameObject(TryGetCameraGameObject_PRAGMATA(), "via.render.VolumetricFogControl")
			if classVolumetricFogControl_PRAGMATA ~= nil then
				_classVolumetricFogControl = classVolumetricFogControl_PRAGMATA
				return true
			end

			return false
		end

		if MHWILDS then
			local classVolumetricFogControl_MHWilds = TryGetComponentFromGameObject(TryGetCameraGameObject_MHWilds(), "via.render.VolumetricFogControl")
			if classVolumetricFogControl_MHWilds ~= nil then
				_classVolumetricFogControl = classVolumetricFogControl_MHWilds
				return true
			end

			return false
		end
	end

	return false
end

--Methods
local _method__RenderingManager__set__IsFilmGrainCustomFilterEnable = nil
local _method__RenderConfig__set_AnitiAliasingSetting = nil
local _method__RenderConfig__set_SharpnessSetting = nil
local _method__LDRColorCorrect__set_Enabled = nil
local _method__ToneMapping__setTemporalAA = nil
local _method__ToneMapping__setTemporalAAAlgorithm = nil
local _method__ToneMapping__set_EchoEnabled = nil
local _method__ToneMapping__set_ExposureEnable = nil
local _method__ToneMapping__set_EV = nil
local _method__ToneMapping__setAutoExposure = nil
local _method__ToneMapping__set_AutoExposureMinEV = nil
local _method__ToneMapping__set_AutoExposureMaxEV = nil
local _method__ToneMapping__set_ReferenceLuminance = nil
local _method__ToneMapping__set_EnableLocalExposure = nil
local _method__ToneMapping__setLocalExposureType = nil
local _method__ToneMapping__setVignetting = nil
local _method__ToneMapping__set_VignettingBrightness = nil
local _method__ToneMapping__set_Sharpness = nil
local _method__VolumetricFogControl__set_Enabled = nil





--6. Apply
--Enum values
--0 = Disable or 0
--1 = Default or 1
--2 = Enable or 2
local _boolInitialized = false

local function ApplyGameSpecificNonPersistentSettings()
	if _boolInitialized == true then
		if RE9 then
			--Film grain
			local intToggleType = _classTableConfig.iFilmGrain
			if intToggleType ~= 1 then
				_method__RenderingManager__set__IsFilmGrainCustomFilterEnable:call(_classRenderingManager, intToggleType == 2)
			end
		end
	end
end

local function ApplyNonPersistentSettings()
	if _boolInitialized == true then
		local classTableConfig = _classTableConfig
		local classToneMapping = _classToneMapping
		local classRenderConfig = _classRenderConfig
		local intToggleType = 1
		local intSharpnessType = 1

		--Anti-aliasing
		local intAntiAliasingType = classTableConfig.iAntiAliasingType
		if intAntiAliasingType ~= 1 then
			_method__RenderConfig__set_AnitiAliasingSetting:call(classRenderConfig, _toRenderConfigAntiAliasingType[intAntiAliasingType])
		end

		--Sharpening
		intSharpnessType = classTableConfig.iSharpnessType
		if intSharpnessType ~= 1 then
			_method__RenderConfig__set_SharpnessSetting:call(classRenderConfig, _toRenderConfigSharpnessType[intSharpnessType])
		end

		--Color correction
		intToggleType = classTableConfig.iColorCorrection
		if intToggleType ~= 1 then
			_method__LDRColorCorrect__set_Enabled:call(_classLDRColorCorrect, intToggleType == 2)
		end

		--TAA
		local intTAA = classTableConfig.iTAA
		if intTAA ~= 1 then
			_method__ToneMapping__setTemporalAA:call(classToneMapping, _toToneMappingTemporalAA[intTAA])
		end

		local intTAAAlgorithm = classTableConfig.iTemporalAAAlgorithm
		if intTAAAlgorithm ~= 1 then
			_method__ToneMapping__setTemporalAAAlgorithm:call(classToneMapping, _toToneMappingTemporalAAAlgorithm[intTAAAlgorithm])
		end

		intToggleType = classTableConfig.iTAAJitter
		if intToggleType ~= 1 then
			_method__ToneMapping__set_EchoEnabled:call(classToneMapping, intToggleType == 2)
		end

		--Exposure
		intToggleType = classTableConfig.iExposure
		if intToggleType ~= 1 then
			_method__ToneMapping__set_ExposureEnable:call(classToneMapping, intToggleType == 2)
			_method__ToneMapping__set_EV:call(classToneMapping, classTableConfig.fEV)
		end

		local intAutoExposure = classTableConfig.iAutoExposure
		if intAutoExposure ~= 1 then
			_method__ToneMapping__setAutoExposure:call(classToneMapping, _toToneMappingAutoExposure[intAutoExposure])
			_method__ToneMapping__set_AutoExposureMinEV:call(classToneMapping, classTableConfig.fAutoExposureMinEV)
			_method__ToneMapping__set_AutoExposureMaxEV:call(classToneMapping, classTableConfig.fAutoExposureMaxEV)
			_method__ToneMapping__set_ReferenceLuminance:call(classToneMapping, classTableConfig.iReferenceLuminanceType)
		end

		intToggleType = classTableConfig.iLocalExposure
		if intToggleType ~= 1 then
			_method__ToneMapping__set_EnableLocalExposure:call(classToneMapping, intToggleType == 2)
			_method__ToneMapping__setLocalExposureType:call(classToneMapping, classTableConfig.iLocalExposureType)
		end

		--Vignette
		local intVignette = classTableConfig.iVignette
		if intVignette ~= 1 then
			_method__ToneMapping__setVignetting:call(classToneMapping, _toToneMappingVignetting[intVignette])
			_method__ToneMapping__set_VignettingBrightness:call(classToneMapping, classTableConfig.fVignetteBrightness)
		end

		intSharpnessType = classTableConfig.iSharpnessType
		if intSharpnessType ~= 1 then
			_method__ToneMapping__set_Sharpness:call(classToneMapping, classTableConfig.fSharpness)
		end

		--Volumetric fog
		intToggleType = classTableConfig.iVolumetricFog
		if intToggleType ~= 1 then
			_method__VolumetricFogControl__set_Enabled:call(_classVolumetricFogControl, intToggleType == 2)
		end

		--Apply game specific settings
		ApplyGameSpecificNonPersistentSettings()
	end
end

local function ApplyGameSpecificPersistentSettings()
	if _boolInitialized == true then
		if MHWILDS then
			local classTableConfig = _classTableConfig
			local classGraphicsManager = _classGraphicsManager
			local classNowGraphicsSetting = classGraphicsManager:call("get_NowGraphicsSetting")
			local boolChanged = false

			local intToggleType = classTableConfig.iFilmGrain
			if (intToggleType ~= 1) then
				classNowGraphicsSetting:call("set_FilmGrain_Enable", intToggleType == 2)
				boolChanged = true
			end

			local intLensDistortionSetting = classTableConfig.iLensDistortion
			if (intLensDistortionSetting ~= 1) then
				classNowGraphicsSetting:call("set_LensDistortionSetting", _toRenderConfigLensDistortionSetting[intLensDistortionSetting])
				boolChanged = true
			end

			if boolChanged == true then
				classGraphicsManager:call("setGraphicsSetting", classNowGraphicsSetting)
			end
		end
	end
end

local function ApplyPersistentSettings()
	if _boolInitialized == true then
		--Var
		local classTableConfig = _classTableConfig
		local classNativeObjectDisplaySettings = _classNativeObjectDisplaySettings
		local classTypeDefinitionDisplaySettings = _classTypeDefinitionDisplaySettings
		local classToneMapping = _classToneMapping

		--Contrast
		if classTableConfig.bCustomContrast then
			classToneMapping:call("set_Contrast", classTableConfig.fContrast)
		end

		--Gamma
		if classTableConfig.bCustomGamma then
			sdk.call_native_func(classNativeObjectDisplaySettings, classTypeDefinitionDisplaySettings, "set_Gamma", classTableConfig.fGamma)
		end

		--Brightness
		if classTableConfig.bCustomBrightness then
			sdk.call_native_func(classNativeObjectDisplaySettings, classTypeDefinitionDisplaySettings, "set_OutputLowerLimit", classTableConfig.fMinBrightness)
			sdk.call_native_func(classNativeObjectDisplaySettings, classTypeDefinitionDisplaySettings, "set_OutputUpperLimit", classTableConfig.fMaxBrightness)
		end

		--UI gamma
		if classTableConfig.bCustomUIGamma then
			sdk.call_native_func(classNativeObjectDisplaySettings, classTypeDefinitionDisplaySettings, "set_GammaForOverlay", classTableConfig.fUIGamma)
		end

		--UI brightness
		local boolCustomUIBrightness = classTableConfig.bCustomUIBrightness
		if boolCustomUIBrightness then
			sdk.call_native_func(classNativeObjectDisplaySettings, classTypeDefinitionDisplaySettings, "set_UseSDRBrightnessOptionForOverlay", boolCustomUIBrightness)
			sdk.call_native_func(classNativeObjectDisplaySettings, classTypeDefinitionDisplaySettings, "set_OutputLowerLimitForOverlay", classTableConfig.fMinUIBrightness)
			sdk.call_native_func(classNativeObjectDisplaySettings, classTypeDefinitionDisplaySettings, "set_OutputUpperLimitForOverlay", classTableConfig.fMaxUIBrightness)
		end

		--Apply game specific settings
		ApplyGameSpecificPersistentSettings()
	end
end





--7. Init
--Native
local function TryInitializeNative()
	if TryGetAndCacheRenderer() == false then return false end
	if TryGetAndCacheRendererTypeDefinition() == false then return false end
	if TryGetAndCacheDisplaySettings() == false then return false end
	if TryGetAndCacheDisplaySettingsTypeDefinition() == false then return false end

	return true
end

--Managed
local function TryInitializeManaged()
	if TryGetAndCacheRenderConfig() == false then return false end
	if TryGetAndCacheToneMapping() == false then return false end
	if TryGetAndCacheLDRPostProcess() == false then return false end
	if TryGetAndCacheVolumetricFogControl() == false then return false end

	local classLDRPostProcess = _classLDRPostProcess
	_classLDRColorCorrect = classLDRPostProcess:call("get_ColorCorrect") if _classLDRColorCorrect == nil then return false end
	_classLDRLensDistortion = classLDRPostProcess:call("get_LensDistortion") if _classLDRLensDistortion == nil then return false end
	_classLDRFilmGrain = classLDRPostProcess:call("get_FilmGrain") if _classLDRFilmGrain == nil then return false end

	return true
end

--Game specific
local function TryInitializeGameSpecific()
	if RE9 then
		--Nothing to initialize
		return true
	end

	if PRAGMATA then
		--Nothing to initialize
		return true
	end

	if MHWILDS then
		if TryGetAndCacheGraphicsManager_MHWilds() == false then return false end
		return true
	end

	return false
end

--Methods
local function TryGetMethod(classStringFullTypeName, classStringMethodName)
	local classTypeDefinition = sdk.find_type_definition(classStringFullTypeName)
	if classTypeDefinition ~= nil then
		local method = classTypeDefinition:get_method(classStringMethodName)
		if method ~= nil then
			return method
		end
	end

	return nil
end

local function TryCacheMethods()
	--Cache the methods used in the per frame hot path
	local classStringRenderConfig = "via.render.RenderConfig"
	_method__RenderConfig__set_AnitiAliasingSetting = TryGetMethod(classStringRenderConfig, "set_AnitiAliasingSetting") if _method__RenderConfig__set_AnitiAliasingSetting == nil then return false end
	_method__RenderConfig__set_SharpnessSetting = TryGetMethod(classStringRenderConfig, "set_SharpnessSetting") if _method__RenderConfig__set_SharpnessSetting == nil then return false end

	local classStringLDRColorCorrect = "via.render.LDRColorCorrect"
	_method__LDRColorCorrect__set_Enabled = TryGetMethod(classStringLDRColorCorrect, "set_Enabled") if _method__LDRColorCorrect__set_Enabled == nil then return false end

	local classStringToneMapping = "via.render.ToneMapping"
	_method__ToneMapping__setTemporalAA = TryGetMethod(classStringToneMapping, "setTemporalAA") if _method__ToneMapping__setTemporalAA == nil then return false end
	_method__ToneMapping__setTemporalAAAlgorithm = TryGetMethod(classStringToneMapping, "setTemporalAAAlgorithm") if _method__ToneMapping__setTemporalAAAlgorithm == nil then return false end
	_method__ToneMapping__set_EchoEnabled = TryGetMethod(classStringToneMapping, "set_EchoEnabled") if _method__ToneMapping__set_EchoEnabled == nil then return false end
	_method__ToneMapping__set_ExposureEnable = TryGetMethod(classStringToneMapping, "set_ExposureEnable") if _method__ToneMapping__set_ExposureEnable == nil then return false end
	_method__ToneMapping__set_EV = TryGetMethod(classStringToneMapping, "set_EV") if _method__ToneMapping__set_EV == nil then return false end
	_method__ToneMapping__setAutoExposure = TryGetMethod(classStringToneMapping, "setAutoExposure") if _method__ToneMapping__setAutoExposure == nil then return false end
	_method__ToneMapping__set_AutoExposureMinEV = TryGetMethod(classStringToneMapping, "set_AutoExposureMinEV") if _method__ToneMapping__set_AutoExposureMinEV == nil then return false end
	_method__ToneMapping__set_AutoExposureMaxEV = TryGetMethod(classStringToneMapping, "set_AutoExposureMaxEV") if _method__ToneMapping__set_AutoExposureMaxEV == nil then return false end
	_method__ToneMapping__set_ReferenceLuminance = TryGetMethod(classStringToneMapping, "set_ReferenceLuminance") if _method__ToneMapping__set_ReferenceLuminance == nil then return false end
	_method__ToneMapping__set_EnableLocalExposure = TryGetMethod(classStringToneMapping, "set_EnableLocalExposure") if _method__ToneMapping__set_EnableLocalExposure == nil then return false end
	_method__ToneMapping__setLocalExposureType = TryGetMethod(classStringToneMapping, "setLocalExposureType") if _method__ToneMapping__setLocalExposureType == nil then return false end
	_method__ToneMapping__setVignetting = TryGetMethod(classStringToneMapping, "setVignetting") if _method__ToneMapping__setVignetting == nil then return false end
	_method__ToneMapping__set_VignettingBrightness = TryGetMethod(classStringToneMapping, "set_VignettingBrightness") if _method__ToneMapping__set_VignettingBrightness == nil then return false end
	_method__ToneMapping__set_Sharpness = TryGetMethod(classStringToneMapping, "set_Sharpness") if _method__ToneMapping__set_Sharpness == nil then return false end

	local classStringVolumetricFogControl = "via.render.VolumetricFogControl"
	_method__VolumetricFogControl__set_Enabled = TryGetMethod(classStringVolumetricFogControl, "set_Enabled") if _method__VolumetricFogControl__set_Enabled == nil then return false end
	return true
end

local function TryCacheGameSpecificMethods()
	--Cache the methods used in the per frame hot path
	if RE9 then
		_method__RenderingManager__set__IsFilmGrainCustomFilterEnable = TryGetMethod("app.RenderingManager", "set__IsFilmGrainCustomFilterEnable") if _method__RenderingManager__set__IsFilmGrainCustomFilterEnable == nil then return false end
		return true
	end

	if PRAGMATA then
		--Nothing to cache
		return true
	end

	if MHWILDS then
		--Nothing to cache
		return true
	end

	return false
end

--Hooks
local function NOP() return end

local function OnShouldApplyPersistentSettings()
	ApplyPersistentSettings()
end

local function TryInitializeHooks()
	if RE9 then
		if TryGetAndCacheEnvStageManagerTypeDefinition_RE9() then
			local methodOnChangeCurrentStage = _classTypeDefinitionEnvStageManager:get_method("OnChangeCurrentStage")
			if methodOnChangeCurrentStage ~= nil then
				sdk.hook(methodOnChangeCurrentStage, NOP, OnShouldApplyPersistentSettings)
				return true
			end
		end

		return false
	end

	if PRAGMATA then
		if TryGetAndCacheEnvironmentSceneManagerTypeDefinition_PRAGMATA() then
			local methodrequestLoad = _classTypeDefinitionEnvironmentSceneManager:get_method("requestLoad")
			if methodrequestLoad ~= nil then
				sdk.hook(methodrequestLoad, NOP, OnShouldApplyPersistentSettings)
				return true
			end
		end

		return false
	end

	if MHWILDS then
		if TryGetAndCacheCameraManagerTypeDefinition_MHWilds() then
			local methodonSceneLoadFadeIn = _classTypeDefinitionCameraManager:get_method("onSceneLoadFadeIn")
			if methodonSceneLoadFadeIn ~= nil then
				sdk.hook(methodonSceneLoadFadeIn, NOP, OnShouldApplyPersistentSettings)
				return true
			end
		end

		return false
	end

	return false
end

--Main
local function Initialize()
	if TryInitializeNative() == false then return end
	if TryInitializeManaged() == false then return end
	if TryInitializeGameSpecific() == false then return end
	if TryCacheMethods() == false then return end
	if TryCacheGameSpecificMethods() == false then return end
	if TryInitializeHooks() == false then return end

	--On success
	_boolInitialized = true
	ApplyPersistentSettings()
	LogInfo("Initialized")
end





--8. Callbacks
local function BeginRendering_Pre()
	if _boolInitialized == false then
		Initialize()
		if (_boolInitialized == false) then return end
	end

	ApplyNonPersistentSettings()
end





--9. Main
LoadFromJson()
re.on_config_save(SaveToJson)
re.on_pre_application_entry("BeginRendering", BeginRendering_Pre)
LogInfo("Loaded")





--10. Script generated UI
local _floatIndent = 24.0

local function Category(classStringText)
	imgui.new_line()
	imgui.text(classStringText)
	imgui.separator()
end

local function SubCategory(classStringText)
	imgui.spacing()
	imgui.indent(_floatIndent)
	imgui.text(classStringText)
	imgui.separator()
end

local function EndSubCategory()
	imgui.unindent(_floatIndent)
end

local function IndentedText(classStringText)
	local floatIndent = _floatIndent
	imgui.indent(floatIndent)
	imgui.text(classStringText)
	imgui.unindent(floatIndent)
end

local function IndentedTextRed(classStringText)
	local floatIndent = _floatIndent
	imgui.indent(floatIndent)
	imgui.text_colored(classStringText, 0xAD0000FF)
	imgui.unindent(floatIndent)
end

local function Combo(classStringLabel, structIntCurrentValue, classTableEnumNames)
	local bChanged, structIntNewValue = imgui.combo(classStringLabel, structIntCurrentValue + 1, classTableEnumNames)
	return bChanged, structIntNewValue - 1
end

re.on_draw_ui(function()
	if reframework:is_drawing_ui() and imgui.tree_node(_stringGUIDVersion) then
		--Var
		local classTableConfig = _classTableConfig
		local enumToggleTypeNames = _enumToggleTypeNames
		local boolChanged = false
		local boolAnyChanged = false --Any persistent setting

		--AntiAliasing
		Category("Anti-aliasing")
		imgui.text("Note: Changing anti-aliasing type to DEFAULT requires changing the in-game anti-aliasing option or game restart to revert the changes.")
		boolChanged, classTableConfig.iAntiAliasingType = Combo("Anti-aliasing type", classTableConfig.iAntiAliasingType, _enumAntiAliasingTypeNames)

		SubCategory("TAA")
		boolChanged, classTableConfig.iTAA = Combo("TAA", classTableConfig.iTAA, _enumTemporalAANames)
		boolChanged, classTableConfig.iTemporalAAAlgorithm = Combo("TAA algorithm", classTableConfig.iTemporalAAAlgorithm, _enumTemporalAAAlgorithmNames)
		boolChanged, classTableConfig.iTAAJitter = Combo("TAA jitter", classTableConfig.iTAAJitter, enumToggleTypeNames)
		EndSubCategory()

		--Vignette
		Category("Vignette")
		imgui.text("Note: Changing vignette to Default requires a game restart to revert the changes.")
		boolChanged, classTableConfig.iVignette = Combo("Vignette", classTableConfig.iVignette, _enumVignetteNames)

		local intVignette = classTableConfig.iVignette
		imgui.begin_disabled(intVignette == 0 or intVignette == 1)
		boolChanged, classTableConfig.fVignetteBrightness = imgui.drag_float("Vignette brightness", classTableConfig.fVignetteBrightness, 0.001, -1.0, 1.0)
		imgui.end_disabled()

		--Color
		Category("Color")
		if MHWILDS then imgui.text("Note: If color correction is disabled it is recommended to enable custom contrast, brightness, and UI brightness and increase the values.") end
		boolChanged, classTableConfig.iColorCorrection = Combo("Color correction", classTableConfig.iColorCorrection, enumToggleTypeNames)

		--Sharpening
		Category("Sharpening")
		if MHWILDS then imgui.text("Note: Changing sharpening type to Default requires a game restart to revert the changes.") end
		imgui.text("Note: The sharpening is often tied to the TAA. If it is not working as intended, try changing the TAA settings.")
		boolChanged, classTableConfig.iSharpnessType = Combo("Sharpness type", classTableConfig.iSharpnessType, _enumSharpnessTypeNames)

		local intSharpnessType = classTableConfig.iSharpnessType
		imgui.begin_disabled(intSharpnessType == 1)
		boolChanged, classTableConfig.fSharpness = imgui.drag_float("Sharpness", classTableConfig.fSharpness, 0.001, 0.0, 10.0)
		imgui.end_disabled()

		--Exposure
		Category("Exposure")
		if PRAGMATA then imgui.text("Note: Changing exposure to Default requires a game restart to revert the changes.") end
		boolChanged, classTableConfig.iExposure = Combo("Exposure", classTableConfig.iExposure, enumToggleTypeNames)

		local intExposure = classTableConfig.iExposure
		imgui.begin_disabled(intExposure == 0 or intExposure == 1)
		boolChanged, classTableConfig.fEV = imgui.drag_float("Exposure value (EV)", classTableConfig.fEV, 0.001, -100.0, 100.0)
		imgui.end_disabled()

		--Local exposure
		Category("Local exposure")
		if PRAGMATA or MHWILDS then imgui.text("Note: Changing local exposure to Default requires a game restart to revert the changes.") end
		boolChanged, classTableConfig.iLocalExposure = Combo("Local exposure", classTableConfig.iLocalExposure, enumToggleTypeNames)

		local intLocalExposure = classTableConfig.iLocalExposure
		imgui.begin_disabled(intLocalExposure == 0 or intLocalExposure == 1)
		boolChanged, classTableConfig.iLocalExposureType = Combo("Local exposure type", classTableConfig.iLocalExposureType, _enumLocalExposureTypeNames)
		imgui.end_disabled()

		--Auto exposure
		Category("Auto exposure")
		boolChanged, classTableConfig.iAutoExposure = Combo("Auto exposure", classTableConfig.iAutoExposure, _enumAutoExposureNames)

		local intAutoExposure = classTableConfig.iAutoExposure
		imgui.begin_disabled(intAutoExposure == 0 or intAutoExposure == 1)
		boolChanged, classTableConfig.iReferenceLuminanceType = Combo("Reference luminance type", classTableConfig.iReferenceLuminanceType, _enumReferenceLuminanceTypeNames)
		boolChanged, classTableConfig.fAutoExposureMinEV = imgui.drag_float("Auto exposure min EV", classTableConfig.fAutoExposureMinEV, 0.01, -100.0, 100.0)
		boolChanged, classTableConfig.fAutoExposureMaxEV = imgui.drag_float("Auto exposure max EV", classTableConfig.fAutoExposureMaxEV, 0.01, -100.0, 100.0)
		imgui.end_disabled()

		--Contrast, gamma, and brightness
		Category("Contrast, gamma, and brightness")
		IndentedText("Note: Disabling custom contrast requires a game restart to revert the changes.")
		IndentedText("Note: Disabling custom gamma, brightness, UI gamma, or UI brightness requires changing the in-game brightness options or game restart to revert the changes.")
		IndentedTextRed("Note: Custom gamma, brightness, UI gamma, and UI brightness are not compatible with HDR. Use the in-game options instead.")

		SubCategory("Contrast")
		boolChanged, classTableConfig.bCustomContrast = imgui.checkbox("Custom contrast", classTableConfig.bCustomContrast)
		boolAnyChanged = boolAnyChanged or boolChanged
		imgui.begin_disabled(classTableConfig.bCustomContrast == false)
		boolChanged, classTableConfig.fContrast = imgui.drag_float("Contrast", classTableConfig.fContrast, 0.001, -10.0, 10.0)
		boolAnyChanged = boolAnyChanged or boolChanged
		imgui.end_disabled()
		EndSubCategory()

		SubCategory("Gamma")
		boolChanged, classTableConfig.bCustomGamma = imgui.checkbox("Custom gamma", classTableConfig.bCustomGamma)
		boolAnyChanged = boolAnyChanged or boolChanged
		imgui.begin_disabled(classTableConfig.bCustomGamma == false)
		boolChanged, classTableConfig.fGamma = imgui.drag_float("Gamma", classTableConfig.fGamma, 0.001, 0.0, 10.0)
		boolAnyChanged = boolAnyChanged or boolChanged
		imgui.end_disabled()
		EndSubCategory()

		SubCategory("Brightness")
		boolChanged, classTableConfig.bCustomBrightness = imgui.checkbox("Custom brightness", classTableConfig.bCustomBrightness)
		boolAnyChanged = boolAnyChanged or boolChanged
		imgui.begin_disabled(classTableConfig.bCustomBrightness == false)
		boolChanged, classTableConfig.fMinBrightness = imgui.drag_float("Min brightness", classTableConfig.fMinBrightness, 0.001, -10.0, 10.0)
		boolAnyChanged = boolAnyChanged or boolChanged
		boolChanged, classTableConfig.fMaxBrightness = imgui.drag_float("Max brightness", classTableConfig.fMaxBrightness, 0.001, -10.0, 10.0)
		boolAnyChanged = boolAnyChanged or boolChanged
		imgui.end_disabled()
		EndSubCategory()

		SubCategory("UI gamma")
		boolChanged, classTableConfig.bCustomUIGamma = imgui.checkbox("Custom UI gamma", classTableConfig.bCustomUIGamma)
		boolAnyChanged = boolAnyChanged or boolChanged
		imgui.begin_disabled(classTableConfig.bCustomUIGamma == false)
		boolChanged, classTableConfig.fUIGamma = imgui.drag_float("UI gamma", classTableConfig.fUIGamma, 0.001, 0.0, 10.0)
		boolAnyChanged = boolAnyChanged or boolChanged
		imgui.end_disabled()
		EndSubCategory()

		SubCategory("UI brightness")
		boolChanged, classTableConfig.bCustomUIBrightness = imgui.checkbox("Custom UI brightness", classTableConfig.bCustomUIBrightness)
		boolAnyChanged = boolAnyChanged or boolChanged
		imgui.begin_disabled(classTableConfig.bCustomUIBrightness == false)
		boolChanged, classTableConfig.fMinUIBrightness = imgui.drag_float("Min UI brightness", classTableConfig.fMinUIBrightness, 0.001, -10.0, 10.0)
		boolAnyChanged = boolAnyChanged or boolChanged
		boolChanged, classTableConfig.fMaxUIBrightness = imgui.drag_float("Max UI brightness", classTableConfig.fMaxUIBrightness, 0.001, -10.0, 10.0)
		boolAnyChanged = boolAnyChanged or boolChanged
		imgui.end_disabled()
		EndSubCategory()

		--Graphics settings
		Category("Graphics settings")
		imgui.text("Note: Changing any of the graphics settings to Default requires a game restart to revert the changes.")
		if RE9 then imgui.text("Note: Changing film grain requires a game restart to apply the changes.") end
		boolChanged, classTableConfig.iVolumetricFog = Combo("Volumetric fog", classTableConfig.iVolumetricFog, enumToggleTypeNames)
		boolAnyChanged = boolAnyChanged or boolChanged
		boolChanged, classTableConfig.iFilmGrain = Combo("Film grain", classTableConfig.iFilmGrain, enumToggleTypeNames)
		boolAnyChanged = boolAnyChanged or boolChanged
		boolChanged, classTableConfig.iLensDistortion = Combo("Lens distortion", classTableConfig.iLensDistortion, _enumLensDistortionSettingNames)
		boolAnyChanged = boolAnyChanged or boolChanged

		--Apply persistent settings if any changed
		if boolAnyChanged then ApplyPersistentSettings() end

		--Pop tree
		imgui.tree_pop()
	end
end)
