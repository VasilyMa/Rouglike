using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.SceneManagement;


public class EntryPoint : MonoBehaviour
{
    public bool EnableUI;

    public static EntryPoint Instance;

#if UNITY_EDITOR    
    public UnityEditor.SceneAsset SceneAsset;
#endif

    [ReadOnlyInspector] public string DestinationTargetScene;

    private void Awake()
    {
        Instance = this;

        SaveModule.Initialize();

        ConfigModule.Initialize(this, onConfigLoaded);
    }

    void onConfigLoaded()
    {
        StartCoroutine(StartLoadedScene());
    }

    IEnumerator StartLoadedScene()
    {
        EntityModule.Initialize();

        yield return new WaitUntil(() => EntityModule.IsInit);

        var uiConfig = ConfigModule.GetConfig<UIConfig>();

        uiConfig.Initialize();

        //TODO need to put this somewhere else sorry
        if (DestinationTargetScene != "TestScene")
        {
            GameSetting settings = Object.FindObjectOfType<GameSetting>();
            settings.LoadAllSettings();
        }


        SceneManager.LoadScene(DestinationTargetScene);
    }


#if UNITY_EDITOR    
    private void OnValidate()
    {
        if (SceneAsset != null)
        {
            DestinationTargetScene = SceneAsset.name;
        }
    }
#endif
}