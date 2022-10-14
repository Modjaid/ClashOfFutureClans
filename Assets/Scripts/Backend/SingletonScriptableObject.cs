using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Resources.LoadAll<T>("Resources");
                _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

                if(_instance == null) Debug.LogError("[SingletonScriptableObject] не был найден файл. Возможно забыли внести его в SingletonScriptableObjectRefferences?");
            }
            return _instance;
        }
    }
}
