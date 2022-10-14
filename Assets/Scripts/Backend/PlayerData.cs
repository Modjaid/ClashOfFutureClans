using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : SingletonScriptableObject<PlayerData>
{
    public event Action<int> OnChangeNox;
    public event Action<int> OnChangeSoftCost;
    public event Action<float> OnChangeLevelPoints;

    [SerializeField] private int softCost;
    [SerializeField] private int nox;
    [SerializeField] private float levelPoints;

    public int SoftCost
    {
        get
        {
            if (PlayerPrefs.HasKey("SoftCost"))
            {
                return PlayerPrefs.GetInt("SoftCost");
            }
            else
            {
                PlayerPrefs.SetInt("SoftCost", softCost);
                return softCost;
            }
        }
        set
        {
            OnChangeSoftCost?.Invoke(value);
            PlayerPrefs.SetInt("SoftCost", value);
	    softCost = value;
        }
    }

    public int Nox
    {
        get
        {
            if (PlayerPrefs.HasKey("Nox"))
            {
                return PlayerPrefs.GetInt("Nox");
            }
            else
            {
                PlayerPrefs.SetInt("Nox", nox);
                return nox;
            }
        }
        set
        {
            OnChangeNox?.Invoke(value);
            PlayerPrefs.SetInt("Nox", value);
	    nox = value;
        }
    }
    public float LevelPoints
    {
        get
        {
            if (PlayerPrefs.HasKey("LevelPoints"))
            {
                return PlayerPrefs.GetFloat("LevelPoints");
            }
            else
            {
                PlayerPrefs.SetFloat("LevelPoints", levelPoints);
                return levelPoints;
            }
        }
        set
        {
            OnChangeLevelPoints?.Invoke(value);
            PlayerPrefs.SetFloat("LevelPoints", value);
	    levelPoints = value;
        }
    }
}

public struct Cash
{
    public int softCost;
    public int nox;


    public static bool operator >(Cash c1,Cash c2)
    {
       return (c1.softCost > c2.softCost && c1.nox > c2.nox);
    }
    public static bool operator <(Cash c1, Cash c2)
    {
        return (c1.softCost < c2.softCost && c1.nox < c2.nox);
    }
    public static Cash operator +(Cash c1, Cash c2)
    {
        return new Cash { nox = c1.nox + c2.nox , softCost = c1.softCost + c2.softCost};
    }
    public static Cash operator -(Cash c1, Cash c2)
    {
        return new Cash { nox = c1.nox + c2.nox, softCost = c1.softCost + c2.softCost };
    }

}