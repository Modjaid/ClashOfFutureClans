using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Diagnostics.DebuggerDisplay("entity={entity}")]
public class Building : BasingBuilding
{
    private IUnit target;
    private Map map;

    [SerializeField]
    private Entity entity = 0;
    [SerializeField]
    private Entity priority = 0;

    public IMoveable moveable;
    public IAttacker attacker;
    public IHealth health;

    public override IMoveable Moveable
    {
        get
        {
            if (moveable == null)
            {
                moveable = GetComponent<IMoveable>();
            }
            return moveable;
        }
    }
    public override IAttacker Attacker
    {
        get
        {
            if (attacker == null)
            {
                attacker = GetComponent<IAttacker>();
            }
            return attacker;
        }
    }
    public override IHealth Health
    {
        get
        {
            if (health == null)
            {
                health = GetComponent<IHealth>();
            }
            return health;
        }
    }
    public override Entity Entity => entity;
    public override Entity Priority => priority;
    public override IUnit Target
    {
        get => target;
        set
        {
            if (!value.Equals(target))
            {
                if (target != null)
                {
                    (target.Health as Health).OnDead -= SetTargetToNull;
                }
                target = value;
                Attacker.Target = value;
                (target.Health as Health).OnDead += SetTargetToNull;
            }
        }
    }
    public override GameObject GO => gameObject;
    public override (int x, int y) GridPos => transform.position.ToTuple();

    public override Map Map
    {
        set => map = value;
    }

    [System.Obsolete("TODO: update attacker")]
    private void Update()
    {
        if (Attacker.CanAttack)
        {
            Attacker.Attack();
        }
    }


    private void SetTargetToNull()
    {
        target = null;
        attacker.Target = null;
    }
}
