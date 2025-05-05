using System.Collections;
using System.Linq;

using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "Settings", menuName = "Configs/Settings")]
public class GameSettingsConfig : Config
{
    public UniversalRenderPipelineAsset[] GraphicPressets;

    public override IEnumerator Init()
    {
        isLoaded = false;

        // ��������� ��� ������� �� ����� Resources
        var allGameSettings = Resources.LoadAll<GameSetting>("");

        if (allGameSettings.Length == 0)
        {
            
            yield return null;
        }

        // �������� ������ ��������� (��� ����� ������ �� ��������)
        var selectedGameSetting = allGameSettings.FirstOrDefault();

        if (selectedGameSetting != null)
        {
            var instance = Object.Instantiate(selectedGameSetting);
            instance.Init(); 
            isLoaded = true;
            
        }
        else
        {
            
        }

        yield return new WaitUntil(() => isLoaded);
    }
}
