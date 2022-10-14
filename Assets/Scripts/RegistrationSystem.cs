using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class RegistrationSystem : MonoBehaviour
{
  //  [SerializeField] private VideoPlayer player;
    [SerializeField] private UnityEngine.UI.Text text;
    void Start()
    {
   //     player.Play();
        if(PlayerPrefs.HasKey("Name"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("BattleWar");
        }
    }

    public void SaveName()
    {
        var name = text.text;
        //var bytes = System.Text.Encoding.UTF32.GetBytes(name);
        //var strings = System.BitConverter.ToString(bytes);
        PlayerPrefs.SetString("Name", name);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("BattleWar");
    }

    public static string GetName()
    {
        var buildfrom = PlayerPrefs.GetString("Name");
        //string[] strArray = buildfrom.Split('-');
        //byte[] bytes = new byte[strArray.Length];
        //for (int i = 0; i < strArray.Length; i++)
        //{
        //    bytes[i] = System.Convert.ToByte(strArray[i], 16);
        //}
        //var name = System.Text.Encoding.UTF32.GetString(bytes);
        return buildfrom;
    }
}
