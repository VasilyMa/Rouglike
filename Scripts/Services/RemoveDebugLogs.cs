using System.IO;
#if UNITY_EDITOR
using UnityEditor;

public class RemoveDebugLogs
{
    [MenuItem("Tools/Remove Debug Logs")]
    public static void RemoveDebugLogsFromScripts()
    {
        string scriptsPath = "Assets/Scripts";
        string[] files = Directory.GetFiles(scriptsPath, "*.cs", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            // ���������� SaveModule.cs � ConfigModule.cs
            if (file.EndsWith("SaveModule.cs") || file.EndsWith("ConfigModule.cs"))
                continue;

            string content = File.ReadAllText(file);

            // ������� Debug.Log, Debug.LogWarning, Debug.LogError
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\bDebug\.Log(Warning|Error)?\s*\(.*?\)\s*;", "");

            File.WriteAllText(file, content);
        }

        AssetDatabase.Refresh();
    }
}
#endif
