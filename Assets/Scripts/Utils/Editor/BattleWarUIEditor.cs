using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleWarUI))]
public class BattleWarUIEditor : Editor
{
    BattleWarUI battleUI;
    public override void OnInspectorGUI()
    {
        battleUI = target as BattleWarUI;
        DrawDefaultInspector();

    }

    void ButtonDestroyAllCards()
    {
        if (GUILayout.Button("Destroy All Cards", GUILayout.Height(50)))
        {
            var list = battleUI.GetComponentsInChildren<BattleWarCardUI>();
            for (int i = 0; i < list.Length; i++)
            {
                DestroyImmediate(list[i].gameObject);
            }
        }
    }


}
