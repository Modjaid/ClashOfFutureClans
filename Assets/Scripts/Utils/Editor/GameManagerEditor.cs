using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var battleUI = target as GameManager;
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("------------\nDontDestroyOnLoad\n------------\nSingleton\n------------", MessageType.Info, true);
    }
}
