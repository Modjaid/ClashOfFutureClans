using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TeamManager
{
    public event Action<IUnit> OnAddUnit;

    private readonly List<IUnit> units = new List<IUnit>();
    public TeamManager enemies;
    public Map map;
    public bool hasBuildings;

    public void AddUnit(IUnit unit)
    {
        OnAddUnit?.Invoke(unit);
        units.Add(unit);
        unit.Map = map;
        if(hasBuildings)
            unit.CCoeff = 100;
        map.SetUnit(unit);
        StartHandle(unit);
    }

    private void StartHandle(IUnit unit)
    {
        var healthImpl = unit.Health as Health;
        if (healthImpl)
        {
            healthImpl.OnDead += () =>
            {
                units.Remove(unit);
            };
        }
    }

    public TeamManager(Map map, bool hasBuildings, MonoBehaviour handler)
    {
        this.map = map;
        this.hasBuildings = hasBuildings;
        handler.StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        while (true)
        {
            SetTargets();
            yield return null;
        }
    }

    private void SetTargets()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].Target != null && units[i].Attacker != null && units[i].Attacker.CanAttack)
                continue;
            IUnit nearest = units[i].Target;
            int nearestDistance = nearest == null? int.MaxValue: nearest.GridPos.GridDistance(units[i].GridPos);
            bool priorityFound = nearest == null? false: nearest.Entity == units[i].Priority;
            for (int j = 0; j < enemies.units.Count; j++)
            {
                int distance = units[i].GridPos.GridDistance(enemies.units[j].GridPos);
                if(!priorityFound)
                {
                    if(enemies.units[j].Entity == units[i].Priority)
                    {
                        priorityFound = true;
                        nearest = enemies.units[j];
                        nearestDistance = distance;
                    }
                    else if(distance < nearestDistance)
                    {
                        nearest = enemies.units[j];
                        nearestDistance = distance;
                    }
                }
                else
                {
                    if (enemies.units[j].Entity == units[i].Priority && distance < nearestDistance)
                    {
                        nearest = enemies.units[j];
                        nearestDistance = distance;
                    }
                }
            }
            if(nearest != null)
                units[i].Target = nearest;
        }
    }
}
