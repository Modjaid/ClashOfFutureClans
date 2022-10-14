using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelGameScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelGameScene;
    }

    private void OnLevelGameScene(Scene scene, LoadSceneMode sceneMode)
    {

    }

}
