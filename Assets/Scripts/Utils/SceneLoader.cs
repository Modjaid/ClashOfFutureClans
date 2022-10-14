using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator background;
    public void LoadSceneAcync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void Start()
    {
        background.gameObject.SetActive(true);
        background.SetTrigger("Play");
    }
}
