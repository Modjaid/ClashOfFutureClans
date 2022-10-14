using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BasingBuilding
{
    [SerializeField] private Entity priority;
    [SerializeField] private Entity entity;
    private IAttacker attacker;
    private IHealth health;
    private IUnit target;
    public override IMoveable Moveable => null;
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
        set => target = value;
    }
    public override GameObject GO => gameObject;
    public override  (int x, int y) GridPos => transform.position.ToTuple();
    private Map map;
    public override Map Map
    {
        set
        {
            map = value;
        }
    }

    private void SetTargetToNull()
    {
        target = null;
        attacker.Target = null;
    }
}
