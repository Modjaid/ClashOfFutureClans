using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artromskiy;

public class Unit : MonoBehaviour, IUnit
{
    private IUnit target;
    private IMoveable moveable;
    private IAttacker attacker;
    private IHealth health;
    [SerializeField]
    private Entity entity = 0;
    [SerializeField]
    private Entity priority = 0;
    private List<Vector3> path;
    private Map map;
    private Pathfind pathfind;
    private (int x, int y) gridPos;
    private (int x, int y) lastTargetPos;

    public IUnit Target
    {
        get => target;
        set
        {
            if (!value.Equals(target))
            {
                HandleNewTarget(value);
                GetPath();
                Moveable.Move(path);
            }
        }
    }
    public Map Map
    {
        set
        {
            GridPos = transform.position.ToTuple();
            map = value;
            pathfind = new Pathfind(map, (1f / 20), 1f);
            map.OnChanged += (pos) =>
            {
                if (target != null && target.GridPos != pos && pos.GridDistance(GridPos) < 40)
                {
                    GetPath();
                    Moveable.Move(path);
                }
            };
        }
    }
    public IMoveable Moveable
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
    public IAttacker Attacker
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
    public IHealth Health {
        get
        {
            if (health == null)
            {
                health = GetComponent<IHealth>();
            }
            return health;
        }
    }
    public Entity Entity => entity;
    public Entity Priority => priority;
    public GameObject GO => gameObject;
    public (int x, int y) GridPos
    {
        get => gridPos;
        private set
        {
            gridPos = value;
        }
    }
    private float ccoef = 0;
    public float CCoeff
    {
        set
        {
            pathfind = new Pathfind(map, (1f / 50) * value, 1f);
            ccoef = value;
        }
        get
        {
            return ccoef;
        }
    }

    private void Start()
    {
        (Health as Health).OnDamage += SolveGettingDamage;
    }

    private void GetPath()
    {
        if(target == null)
        {
            path = new List<Vector3>() { transform.position };
            return;
        }
        List <Vector3> l = pathfind.FindPath(GridPos, target.GridPos);
        int enemy = l.Count - 1;
        for (int i = 0; i < l.Count; i++)
        {
            if(CCoeff != 100)
            {
                if (map.GetUnit(l[i]) != null)
                {
                    enemy = i;
                    HandleNewTarget(map.GetUnit(l[enemy]));
                    break;
                }
            }
            else
            {
                if (map.GetUnit(l[i]) != null)
                {
                    enemy = i - 1;
                    HandleNewTarget(target);
                    break;
                }
            }
        }
        l.RemoveRange(enemy + 1, l.Count - enemy - 1);
        if(l.Count == 0)
            path = new List<Vector3>() { transform.position };
        path = l;
    }

    private void SolveGettingDamage(int damage, IUnit unit)
    {
        if(target == null || unit.GridPos.GridDistance(GridPos) < target.GridPos.GridDistance(GridPos))
        {
            Target = unit;
        }
    }

    private void Update()
    {
        if (target == null)
            return;
        if(transform.position.ToTuple() != GridPos)
        {
            GridPos = transform.position.ToTuple();
            if (lastTargetPos != target.GridPos)
            {
                //Debug.Log("updated last target pos");
                GetPath();
                Moveable.Move(path);
                lastTargetPos = target.GridPos;
            }
        }
        if (Attacker.CanAttack)
        {
            path = new List<Vector3> { transform.position };
            Moveable.Move(path);
            Attacker.Attack();
        }
    }

    private void HandleNewTarget(IUnit value)
    {
        if (target != null)
        {
            (target.Health as Health).OnDead -= SetTargetToNull;
        }
        if(value is BasingBuilding b && CCoeff == 100)
        {
            return;
        }
        target = value;
        lastTargetPos = target.GridPos;
        Attacker.Target = value;
        (target.Health as Health).OnDead += SetTargetToNull;
    }

    private void SetTargetToNull()
    {
        target = null;
        attacker.Target = null;
    }
    
}
