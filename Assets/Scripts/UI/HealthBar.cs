using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private RectTransform bounds;
    [SerializeField] private RectTransform fill;

    private int unitHealth;
    private Vector2 startSize;

    public Transform unit;

    public void Init(IUnit unit)
    {
        this.unit = unit.GO.transform;
        unitHealth = unit.Health.Points;
        startSize = fill.sizeDelta;
    }

    public void UpdateInfo(int damage, IUnit attacker)
    {
        fill.sizeDelta = new Vector2(((float)(damage) / unitHealth) * startSize.x, startSize.y);
    }    
}
