using System.Collections.Generic;

using UnityEngine;
using UniRx;
using System.Linq;
using System;

public class GameSetting : MonoBehaviour
{
    private CompositeDisposable disposables = new CompositeDisposable();
    public const string HD = "HD", WXGA = "WXGA", FULLHD = "FULLHD", QHD = "QHD", UHD = "UHD";
    public static GameSetting Instance { get; private set; }

    public Dictionary<string, Resolution> Resolution { get; private set; }
    public List<ShadowQualitySetting> shadowQualitySettings;
    public List<string> qualitySettings;
    public Resolution defaultResolution { get; private set; }

    private GameSettingsConfig gameSetting;

    public ReactiveProperty<int> weaponIndex = new ReactiveProperty<int>();
    public ReactiveProperty<string> resolutionID = new ReactiveProperty<string>();
    public ReactiveProperty<int> graphicSettingsIndex = new ReactiveProperty<int>();
    public ReactiveProperty<int> shadowQualityIndex = new ReactiveProperty<int>();
    public ReactiveProperty<int> antiAliasingValue = new ReactiveProperty<int>();
    public ReactiveProperty<float> GeneralSoundValue = new ReactiveProperty<float>();
    public ReactiveProperty<float> EffectsSoundValue = new ReactiveProperty<float>();
    public ReactiveProperty<float> UISoundValue = new ReactiveProperty<float>();
    public ReactiveProperty<float> AmbientSoundValue = new ReactiveProperty<float>();

    public ReactiveProperty<bool> isFullScreen = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> isAntiAliasing = new ReactiveProperty<bool>();
    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadAllSettings()
    {
        LoadResolutions();
        LoadShadowQualities();
        LoadSettings();

        SusSettingsDataValues susSettingsDataValues = GetSettingsValues();
        

        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.CreateSettingsElements(susSettingsDataValues, UIManagerRitualist.GetUIManager.SettingsManagerGameMenu._settingsForm);
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnSettingsApply += SaveSettings;
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnSettingsCancel += UpdateUISettings;
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnGeneralVolumeChanged += PrewUpdateGeneralVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnSettingsChanged += OnUpdateSettings;


        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnUIVolumeChanged += PrewUpdateUIVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnEffectsVolumeChanged += PrewUpdateEffectsVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnAmbientVolumeChanged += PrewUpdateAmbientVolume;

        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.CreateSettingsElements(susSettingsDataValues, UIManagerRitualist.GetUIManager.SettingsManagerMainMenu._settingsForm);
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnSettingsApply += SaveSettings;
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnSettingsCancel += UpdateUISettings;
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnGeneralVolumeChanged += PrewUpdateGeneralVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnSettingsChanged += OnUpdateSettings;

        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnUIVolumeChanged += PrewUpdateUIVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnEffectsVolumeChanged += PrewUpdateEffectsVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnAmbientVolumeChanged += PrewUpdateAmbientVolume;


    }

    private void OnDestroy()
    {

        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnSettingsApply -= SaveSettings;
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnSettingsCancel -= UpdateUISettings;
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnGeneralVolumeChanged -= PrewUpdateGeneralVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnSettingsChanged -= OnUpdateSettings;

        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnUIVolumeChanged -= PrewUpdateUIVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnEffectsVolumeChanged -= PrewUpdateEffectsVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.OnAmbientVolumeChanged -= PrewUpdateAmbientVolume;

        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnSettingsApply -= SaveSettings;
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnSettingsCancel -= UpdateUISettings;
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnGeneralVolumeChanged -= PrewUpdateGeneralVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnSettingsChanged -= OnUpdateSettings;

        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnUIVolumeChanged -= PrewUpdateUIVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnEffectsVolumeChanged -= PrewUpdateEffectsVolume;
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.OnAmbientVolumeChanged -= PrewUpdateAmbientVolume;

        disposables.Dispose();
    }
    void LoadResolutions()
    {
        Resolution = new Dictionary<string, Resolution>();
        foreach (var resolution in Screen.resolutions)
        {
            Resolution.TryAdd(ResolutionToID(resolution), resolution);
        }

        defaultResolution = Screen.currentResolution;

        if (Display.displays.Length > 0)
        {
            Resolution recommendedResolution = new Resolution();
            recommendedResolution.width = Screen.currentResolution.width;
            recommendedResolution.height = Screen.currentResolution.height;
            recommendedResolution.refreshRateRatio = Screen.currentResolution.refreshRateRatio;
            defaultResolution = recommendedResolution;
        }
        else
        {
            
        }
        
        Screen.SetResolution(defaultResolution.width, defaultResolution.height, true);
        Display.main.SetRenderingResolution(defaultResolution.width, defaultResolution.height);
    }
    void LoadShadowQualities()
    {
        shadowQualitySettings = new List<ShadowQualitySetting>
        {
            new ShadowQualitySetting("Low", 1, 25f, 100f, ShadowQuality.Disable),
            new ShadowQualitySetting("Medium", 2, 100f, 250f, ShadowQuality.HardOnly),
            new ShadowQualitySetting("High", 4, 150f, 500f, ShadowQuality.All)
        };
        qualitySettings = new List<string> { "Low", "Middle", "High" };
    }
    private void OnShadowQualityChanged(int index)
    {
        ShadowQualitySetting setting = shadowQualitySettings[index];

        var currentURPAsset = QualitySettings.renderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
        if (currentURPAsset != null)
        {
            currentURPAsset.shadowCascadeCount = setting.CascadeCount;
            currentURPAsset.shadowDistance = setting.ShadowDistance;
            QualitySettings.shadows = setting.Quality;
            QualitySettings.shadowDistance = setting.ShadowDistance;
        }
        QualitySettings.SetQualityLevel(graphicSettingsIndex.Value);
    }

    void LoadSettings()
    {
        var settingsData = SaveModule.GetData<SettingsData>();

        resolutionID.Value = settingsData.ResolutionSettings;
        graphicSettingsIndex.Value = settingsData.GraphicSettingsIndex;
        shadowQualityIndex.Value = settingsData.ShadowQualityIndex;
        GeneralSoundValue.Value = settingsData.GeneralSoundValue;
        UISoundValue.Value = settingsData.UISoundValue;
        EffectsSoundValue.Value = settingsData.EffectsSoundValue;
        AmbientSoundValue.Value = settingsData.AmbientSoundValue;
        isFullScreen.Value = settingsData.IsFullScreen;
        antiAliasingValue.Value = settingsData.AntiAliasingValue;
        Resolution resolution = new();
        resolution = defaultResolution;
        if (!String.IsNullOrEmpty(resolutionID.Value))
        {
            if (Resolution.ContainsKey(resolutionID.Value))
            {
                resolution = Resolution[resolutionID.Value];
            }
        }
        resolutionID.Value = ResolutionToID(resolution);
        Screen.SetResolution(resolution.width, resolution.height, isFullScreen.Value);
        SoundEntity.Instance.SetMasterVolume(settingsData.GeneralSoundValue);
        SoundEntity.Instance.SetAmbientVolume(settingsData.AmbientSoundValue);
        SoundEntity.Instance.SetUIVolume(settingsData.UISoundValue);
        SoundEntity.Instance.SetEffectsVolume(settingsData.EffectsSoundValue);


        UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset currentURPAsset = QualitySettings.renderPipeline as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
        QualitySettings.SetQualityLevel(graphicSettingsIndex.Value, true);
        /*if (shadowQualityIndex.Value >= 0 && shadowQualityIndex.Value < shadowQualitySettings.Count)
        {
            ShadowQualitySetting setting = shadowQualitySettings[shadowQualityIndex.Value];

            if (currentURPAsset != null)
            {
                currentURPAsset.shadowCascadeCount = setting.CascadeCount;
                currentURPAsset.shadowDistance = setting.currentURPAssetShadowDistance;
                QualitySettings.shadows = setting.Quality;
                QualitySettings.shadowDistance = setting.ShadowDistance; 
            }
        }*/

        Camera.main.allowMSAA = isAntiAliasing.Value;
        QualitySettings.antiAliasing = isAntiAliasing.Value ? antiAliasingValue.Value : 0;
    }

    public void ApplySettings(SusSettingsData data)
    {
        QualitySettings.SetQualityLevel(graphicSettingsIndex.Value, true);
        Screen.SetResolution(Resolution[resolutionID.Value].width, Resolution[resolutionID.Value].height, isFullScreen.Value);
        SoundEntity.Instance.SetMasterVolume(GeneralSoundValue.Value);
        SoundEntity.Instance.SetEffectsVolume(EffectsSoundValue.Value);
        SoundEntity.Instance.SetUIVolume(UISoundValue.Value);
        SoundEntity.Instance.SetAmbientVolume(AmbientSoundValue.Value);
        Camera.main.allowMSAA = isAntiAliasing.Value;
        OnShadowQualityChanged(shadowQualityIndex.Value); 
        QualitySettings.antiAliasing = isAntiAliasing.Value ? antiAliasingValue.Value : 0;
    }

    private void SaveSettings(SusSettingsData data, UIManagerRitualist.UISettingsManager uISettingsManager)
    {
            resolutionID.Value = data.Resolution;
            shadowQualityIndex.Value = shadowQualitySettings
                .Where(x => x.Name == data.Shadow)
                .Select(x => shadowQualitySettings.IndexOf(x))
                .FirstOrDefault();
            graphicSettingsIndex.Value = qualitySettings.IndexOf(data.Quality);
            GeneralSoundValue.Value = data.GeneralVolume;
            EffectsSoundValue.Value = data.EffectsVolume;
            UISoundValue.Value = data.UIVolume;
            AmbientSoundValue.Value = data.AmbientVolume;
            antiAliasingValue.Value = data.Antialiasing;
            isFullScreen.Value = data.Fullscreen;

            SaveModule.SaveSingleData<SettingsData>();
        ApplySettings(data);
        UpdateUISetting(uISettingsManager);
    }

    private void PrewUpdateGeneralVolume(float value)
    {
        SoundEntity.Instance.SetMasterVolume(value);
    }
    private void OnUpdateSettings(bool value)
    {
        LoadSettings();
    }
    private void PrewUpdateAmbientVolume(float value)
    {
        SoundEntity.Instance.SetAmbientVolume(value);
    }
    private void PrewUpdateUIVolume(float value)
    {
        SoundEntity.Instance.SetUIVolume(value);
    }
    private void PrewUpdateEffectsVolume(float value)
    {
        SoundEntity.Instance.SetEffectsVolume(value);
    }

    private void UpdateUISetting(UIManagerRitualist.UISettingsManager uISettingsManager)
    {
        uISettingsManager.UpdateSettings(GetSettingsValues(), uISettingsManager._settingsForm);

        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.UpdateSettingsForm(GetSettingsValues(), UIManagerRitualist.GetUIManager.SettingsManagerGameMenu._settingsForm);
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.UpdateSettingsForm(GetSettingsValues(), UIManagerRitualist.GetUIManager.SettingsManagerMainMenu._settingsForm);
    }

    private void UpdateUISettings()
    {
        UIManagerRitualist.GetUIManager.SettingsManagerGameMenu.UpdateSettings(GetSettingsValues(), UIManagerRitualist.GetUIManager.SettingsManagerGameMenu._settingsForm);
        UIManagerRitualist.GetUIManager.SettingsManagerMainMenu.UpdateSettings(GetSettingsValues(), UIManagerRitualist.GetUIManager.SettingsManagerMainMenu._settingsForm);
    }

    private string ResolutionToID(Resolution resolution)
    {
        return $"{resolution.width}x{resolution.height}@{resolution.refreshRateRatio}";
    }
    private SusSettingsDataValues GetSettingsValues()
    {
        return new()
        {
            Antialiasing = antiAliasingValue.Value,
            Fullscreen = isFullScreen.Value,
            Resolution = resolutionID.Value,
            Shadow = shadowQualitySettings
                .Where(x => shadowQualitySettings.IndexOf(x) == shadowQualityIndex.Value)
                .Select(x => x.Name)
                .FirstOrDefault(),
            Quality =qualitySettings[ graphicSettingsIndex.Value],
            GeneralVolume = GeneralSoundValue.Value,
            AmbientVolume = AmbientSoundValue.Value,
            EffectsVolume = EffectsSoundValue.Value,
            UIVolume = UISoundValue.Value,
            ShadowQualitySettings = shadowQualitySettings,
            QualitySettings = qualitySettings,
            Resolutions = Resolution,
            ShadowSelectedIndex = shadowQualityIndex.Value,
            ResolutionSelectedIndex = Resolution.ToList().FindIndex(x => x.Key == resolutionID.Value),
            QualitySelectedIndex = graphicSettingsIndex.Value
        };
    }
}

public class ShadowQualitySetting
{
    public string Name;
    public int CascadeCount;
    public float currentURPAssetShadowDistance;
    public float ShadowDistance;
    public ShadowQuality Quality;

    public ShadowQualitySetting(string Name, int CascadeCount, float currentURPAssetShadowDistance, float ShadowDistance, ShadowQuality Quality)
    {
        this.Name = Name;
        this.CascadeCount = CascadeCount;
        this.currentURPAssetShadowDistance = currentURPAssetShadowDistance;
        this.ShadowDistance = ShadowDistance;
        this.Quality = Quality;
    }
}
