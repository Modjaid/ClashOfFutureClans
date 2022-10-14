using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Shop))]
public class ShopEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("currentMoney не извлекается из кэша игрока", MessageType.Error);
        DrawDefaultInspector();
    }
}
