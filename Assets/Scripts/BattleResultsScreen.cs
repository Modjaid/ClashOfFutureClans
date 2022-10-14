using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleResultsScreen : MonoBehaviour
{   
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text noxText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Animator barsAnimator;
    [SerializeField] private GameObject redLogo;
    [SerializeField] private GameObject blueLogo;
    [SerializeField] private RandomName randomName;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="won">True - Game won. False - Game lost.</param>
    public void Show(bool won, int money)
    {
        gameObject.SetActive(true);

        StartCoroutine(ShowRoutine(won, money));
    }

    private IEnumerator ShowRoutine(bool won, int money)
    {
        animator.Play("Show");

        moneyText.text = $"+{money} HP";
        noxText.text = "+0 NOX";

        if(won) 
        {
            blueLogo.SetActive(true);
            redLogo.SetActive(false);
            
            if(PlayerPrefs.HasKey("Name") && PlayerPrefs.GetString("Name") != "")
            {
                title.text = $"{PlayerPrefs.GetString("Name")} won";
            }
            else
            {
                title.text = "Hammer won";
            }
        }
        else
        {
            redLogo.SetActive(true);
            blueLogo.SetActive(false);

            title.text = $"{randomName.SelectedName} won";
        }

        yield return new WaitForSecondsRealtime(1.7f);

        if(won) 
        {
            barsAnimator.Play("Won");
        }
        else
        {
            barsAnimator.Play("Lost");
        }
    }
}
