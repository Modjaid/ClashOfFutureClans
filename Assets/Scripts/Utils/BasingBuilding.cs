using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasingBuilding : MonoBehaviour, IUnit
{
    public abstract IMoveable Moveable { get; }

    public abstract IAttacker Attacker { get; }

    public abstract IHealth Health { get; }

    public abstract Map Map { set; }

    public abstract Entity Priority {get;}

    public abstract Entity Entity { get; }

    public abstract IUnit Target
    {
        get;
        set;
    }

    public abstract GameObject GO {get;}

    public abstract (int x, int y) GridPos { get; }

    public virtual float CCoeff
    {
        set
        {

        }
    }
}
