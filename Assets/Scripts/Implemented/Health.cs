using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour , IHealth
{
    [Header("Базовое здоровье компонента")]
    [SerializeField]
    private int points = 1;
    /// <summary>
    /// Called on getting damage returning current points and attacker
    /// </summary>
    public event Action<int, IUnit> OnDamage;
    public event Action OnDead;
    public int Points => points;

    public void Damage(int damage, IUnit attacker)
    {
        damage = Mathf.Max(damage, 0);
        points -= damage;
        OnDamage?.Invoke(points, attacker);
        if(points <= 0)
        {
            OnDead?.Invoke();
            Destroy(gameObject);
        }
    }

}
