using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class GenerateDelHereScripts : EditorWindow
{
    private string componentName = "";

    [MenuItem("Tools/Generate DelHere Scripts")]
    public static void ShowWindow()
    {
        GetWindow<GenerateDelHereScripts>("Generate DelHere Script");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate DelHere Script", EditorStyles.boldLabel);
        componentName = EditorGUILayout.TextField("Component Name", componentName);

        if (GUILayout.Button("Generate Script"))
        {
            if (!string.IsNullOrEmpty(componentName))
            {
                GenerateScript(componentName);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please enter a valid component name.", "OK");
            }
        }
    }

    private static void GenerateScript(string name)
    {
        string folderPath = "Assets/Scripts/DelSystem";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fileName = $"DelHere{name}.cs";
        string filePath = Path.Combine(folderPath, fileName);

        if (File.Exists(filePath))
        {
            
            EditorUtility.DisplayDialog("Warning", $"File {fileName} already exists.", "OK");
            return;
        }

        string fileContent = GetScriptContent(name);
        File.WriteAllText(filePath, fileContent);

        
        EditorUtility.DisplayDialog("Success", $"Generated script: {fileName}", "OK");

        AssetDatabase.Refresh();
    }

    private static string GetScriptContent(string name)
    {
        return $@"using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{{
    public class DelHere{name} : MainEcsSystem
    {{
        readonly EcsFilterInject<Inc<{name}>> _filter = default;
        
        public override MainEcsSystem Clone()
        {{
            return new DelHere{name}();
        }}

        public override void Run(IEcsSystems systems)
        {{
            foreach (var entity in _filter.Value)
            {{
                _filter.Pools.Inc1.Del(entity);
            }}
        }}
    }}
}}";
    }
}
#endif