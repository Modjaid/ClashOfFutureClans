using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(LoadSomeScene);
    }

    private void LoadSomeScene()
    {
	SceneManager.LoadSceneAsync("BattleWar");
    }
}
