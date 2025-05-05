using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEditor;
public static class EditorCoroutine
{
#if UNITY_EDITOR
    public static void Start(IEnumerator routine)
    {
        EditorApplication.update += () => { EditorUpdate(routine); };
    }
    private static void EditorUpdate(IEnumerator routine)
    {
        if (routine.MoveNext() == false)
        {
            EditorApplication.update -= () => { EditorUpdate(routine); };
        }
    }
#endif
}