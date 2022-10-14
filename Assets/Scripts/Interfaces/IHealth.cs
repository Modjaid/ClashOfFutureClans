using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHealth
{
    int Points
    {
        get;
    }
    void Damage(int damage, IUnit attacker);
}
