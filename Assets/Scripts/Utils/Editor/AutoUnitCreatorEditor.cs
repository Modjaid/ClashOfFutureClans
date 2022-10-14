using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AutoUnitCreator))]
public class AutoUnitCreatorEditor : Editor
{
    AutoUnitCreator autoCreator;
    public override void OnInspectorGUI()
    {
        autoCreator = target as AutoUnitCreator;
        if (Application.isPlaying)
        {
            Message();
            DrawButtons();
        }
        else
        {
            DrawDefaultInspector();
        }
    }
    void Message()
    {
       int count = autoCreator.GetAllUnitsCount();
        if(count > 0)
        {
            EditorGUILayout.HelpBox($"Осталось юнитов {count}", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox($"Юнитов не осталось", MessageType.Error);
        }

    }
    void DrawButtons()
    {
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Stop Random",GUILayout.Height(50)))
        {
            autoCreator.StopRandomInstantiate();
        }
        if(GUILayout.Button("Start Random", GUILayout.Height(50)))
        {
            autoCreator.StartRandomInstantiate();
        }
        GUILayout.EndHorizontal();
    }
}
