using UnityEngine;

public class SingletonScriptableObjectRefferences : MonoBehaviour
{
    // https://forum.unity.com/threads/resources-findobjectsoftypeall-doesnt-find-everything.101054/
    // "Scriptable objects that are not referenced in any scene are not detected by Resources.FindObjectsOfTypeAll."

    [Header("иначе они не будут работать.")]
    [Header("SingletonScriptableObject в список,")]
    [Header("Тут мы должны внести все")]
    public Object[] refferences;
}