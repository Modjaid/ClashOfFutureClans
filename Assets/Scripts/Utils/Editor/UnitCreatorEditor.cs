using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitCreator))]
public class UnitCreatorEditor : Editor
{
   public override void OnInspectorGUI()
    {
        var cameraManager = target as UnitCreator;

        DrawDefaultInspector();
        EditorGUILayout.HelpBox("Ћуч определ€ют землю по имени GO <Ground>", MessageType.Warning,true);
        EditorGUILayout.HelpBox("«дани€ в радиусе спавна определ€ет по скрипту <BasicBuilding>", MessageType.Warning, true);
    }
}
