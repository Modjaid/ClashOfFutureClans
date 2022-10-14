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
        EditorGUILayout.HelpBox("��� ���������� ����� �� ����� GO <Ground>", MessageType.Warning,true);
        EditorGUILayout.HelpBox("������ � ������� ������ ���������� �� ������� <BasicBuilding>", MessageType.Warning, true);
    }
}
