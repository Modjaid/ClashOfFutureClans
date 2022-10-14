using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraController))]
public class CameraEditor : Editor
{
   public override void OnInspectorGUI()
    {
        var cameraManager = target as CameraController;

        DrawDefaultInspector();
        EditorGUILayout.HelpBox("If EDITOR \nRotation - Press left Mouse Button\nZoom - scroll\nMove - Press scroll", MessageType.Info,true);
        EditorGUILayout.HelpBox("If Mobile \nRotation - Press one Touch\nZoom - distance of two Touch\nMove - move of Two Touch", MessageType.Info, true);
    }
}
