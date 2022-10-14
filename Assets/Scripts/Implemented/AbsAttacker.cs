using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsAttacker : MonoBehaviour, IAttacker
{
    public event System.Action OnAttack;
    public abstract IUnit Target { set; }

    public abstract bool CanAttack { get; }

    public abstract void Attack();

    protected void RiseOnAttack()
    {
        OnAttack?.Invoke();
    }
}
