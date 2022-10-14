using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    IMoveable Moveable
    {
        get;
    }
    IAttacker Attacker
    {
        get;
    }
    IHealth Health
    {
        get;
    }
    public Map Map
    {
        set;
    }
    public Entity Priority
    {
        get;
    }

    public Entity Entity
    {
        get;
    }

    public IUnit Target
    {
        get;
        set;
    }
    public GameObject GO
    {
        get;
    }

    public (int x, int y) GridPos
    {
        get;
    }

    public float CCoeff
    {
        set;
    }
}
