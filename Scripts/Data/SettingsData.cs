[System.Serializable]
internal class SettingsData : IDatable
{
    public string ResolutionSettings;
    public int GraphicSettingsIndex;
    public int ShadowQualityIndex;
    public float GeneralSoundValue;
    public float UISoundValue;
    public float EffectsSoundValue;
    public float AmbientSoundValue;
    public bool IsFullScreen;
    public int AntiAliasingValue;

    public SettingsData()
    {
        ResolutionSettings = GameSetting.HD;
        GeneralSoundValue = 0.75f;
        UISoundValue = 0.75f;
        EffectsSoundValue = 0.75f;
        AmbientSoundValue = 0.75f;
        GraphicSettingsIndex = 0;
    }

    public string DATA_ID => "SettingsData_ID";

    public void ProcessUpdataData()
    {
        var gameSetings = GameSetting.Instance;

        if (gameSetings == null) return;

        ResolutionSettings = gameSetings.resolutionID.Value;
        GraphicSettingsIndex = gameSetings.graphicSettingsIndex.Value;
        ShadowQualityIndex = gameSetings.shadowQualityIndex.Value;
        GeneralSoundValue = gameSetings.GeneralSoundValue.Value;
        UISoundValue = gameSetings.UISoundValue.Value;
        EffectsSoundValue = gameSetings.EffectsSoundValue.Value;
        AmbientSoundValue = gameSetings.AmbientSoundValue.Value;
        IsFullScreen = gameSetings.isFullScreen.Value;
        AntiAliasingValue = gameSetings.antiAliasingValue.Value;
    }
    public void Dispose()
    {

    }
}