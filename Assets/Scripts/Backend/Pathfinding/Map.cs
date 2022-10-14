using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVO;

public class Map
{
    private readonly Dictionary<(int x, int y), IUnit> posToUnit = new Dictionary<(int x, int y), IUnit>();
    private readonly Dictionary<IUnit, List<(int x, int y)>> unitToPos = new Dictionary<IUnit, List<(int x, int y)>>();
    public Map()
    {

    }

    public event System.Action<(int x, int y)> OnChanged;


    public IUnit GetUnit((int x, int y) point)
    {
        posToUnit.TryGetValue(point, out IUnit unit);
        return unit;
    }

    public IUnit GetUnit(Vector3 point)
    {
        var vec = point.ToTuple();
        posToUnit.TryGetValue(vec, out IUnit unit);
        return unit;
    }

    public List<(int x, int y)> GetPos(IUnit unit)
    {
        if(unitToPos.ContainsKey(unit))
            return unitToPos[unit];
        return null;
    }

    private void SetUnit(IUnit unit, (int x, int y) pos)
    {
        //Debug.Log("Keys count " + buildingList.Keys.Count.ToString());
        if (unit is BasingBuilding b)
            if (FractionData.TryGetBuildingDataFromAllFractions(b.name, out FractionData.BuildingData data))
            {
                List<(int x, int y)> positions = new List<(int x, int y)>();
                for (int i = 0 - data.scale.x / 2; i < data.scale.x - data.scale.x / 2; i++)
                {
                    for (int j = 0 - data.scale.y / 2; j < data.scale.y - data.scale.y / 2; j++)
                    {
                        var position = pos;
                        position.x += i;
                        position.y += j;
                        positions.Add(position);
                        // Adding to buildings;
                        try
                        {
                            posToUnit.Add(position, unit);
                        }
                        catch
                        {
                            Debug.Log("Try to add " + b.name);
                            if(posToUnit.TryGetValue(position, out IUnit u))
                                Debug.Log("On the place of" + u.GO.name + " at " + position.ToString());
                        }
                    }
                }
                // Inform that map is updated;
                OnChanged?.Invoke(pos);
                unitToPos.Add(unit, positions);
                (unit.Health as Health).OnDead += () =>
                {
                    for (int i = 0; i < positions.Count; i++)
                    {
                        posToUnit.Remove(positions[i]);
                    }
                    unitToPos.Remove(unit);
                    OnChanged?.Invoke(pos);
                };
            }
    }


    public void Visualize()
    {
        foreach (var item in posToUnit)
        {
            var pos = new Vector3(item.Key.x, 0, item.Key.y);
            Debug.DrawRay(pos + (Vector3.right * 0.5f), Vector3.up, Color.blue);
            Debug.DrawRay(pos - (Vector3.right * 0.5f), Vector3.up, Color.blue);
            Debug.DrawRay(pos + (Vector3.forward * 0.5f), Vector3.up, Color.blue);
            Debug.DrawRay(pos - (Vector3.forward * 0.5f), Vector3.up, Color.blue);
            Debug.DrawLine(pos, pos + Vector3.right * 0.5f, Color.blue);
            Debug.DrawLine(pos, pos - Vector3.right * 0.5f, Color.blue);
            Debug.DrawLine(pos, pos + Vector3.forward * 0.5f, Color.blue);
            Debug.DrawLine(pos, pos - Vector3.forward * 0.5f, Color.blue);
        }
    }

    public void SetUnit(IUnit unit)
    {
        SetUnit(unit, unit.GO.transform.position.ToTuple());
    }

}
