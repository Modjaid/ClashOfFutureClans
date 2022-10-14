using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideBar
{
    public Coroutine sliding;
    public float slideBarDiapason;
    private float speedUp;
    private float currentPoints;
    public float targetPoints;
    private Image image;
    public SlideBar(float slideBarDiapason, Image image,float speedUp = 4)
    {
        targetPoints = slideBarDiapason;
        this.slideBarDiapason = slideBarDiapason;
        currentPoints = slideBarDiapason;
        this.speedUp = speedUp;
        this.image = image;
    }
    public float GetBarNormalized()
    {
        return currentPoints / slideBarDiapason;
    }
 
    public IEnumerator Sliding(float currentHealth)
    {
        if (currentHealth < 0)
        {
            this.targetPoints = 0;
        }
        else if(currentHealth > slideBarDiapason)
        {
            slideBarDiapason = currentHealth;
            this.targetPoints = currentHealth;
            this.currentPoints = currentHealth;
        }
        else
        {
          //  Debug.Log($"Diapason:{slideBarDiapason}, targetPoints:{currentHealth}");
            this.targetPoints = currentHealth;
        }

        if (currentPoints < targetPoints)
        {
            return Increase();
        }
        return Decrease();

        IEnumerator Increase()
        {
            while (currentPoints < targetPoints)
            {
                currentPoints += Time.deltaTime * speedUp;
                image.fillAmount = GetBarNormalized();
                yield return null;
            }
            
        }
        IEnumerator Decrease()
        {
            while (currentPoints > targetPoints)
            {
                currentPoints -= Time.deltaTime * speedUp;
                image.fillAmount = GetBarNormalized();
                yield return null;
            }
            currentPoints = targetPoints;
        }
    }
}
