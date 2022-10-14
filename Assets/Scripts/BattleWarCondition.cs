using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ќбработчик общих очков комманд и победы
/// </summary>
public class BattleWarCondition
{
    public event Action<int> OnDecreaseLeftPoints;
    public event Action<int> OnDecreaseRightPoints;
    private readonly Dictionary<string, int> healthPointsBuffer;

    private readonly Dictionary<string, int> needToDestroy = new Dictionary<string, int>();

    /// <summary>
    /// True - VICTORY
    /// False - FAIL
    /// </summary>
    public event Action<bool> OnEndGame; 

    private int leftCommonPoints;
    private int rightCommonPoints;

    public int LeftCommonPoints
    {
        get => leftCommonPoints;
        private set
        {
            OnDecreaseLeftPoints?.Invoke(value);
            leftCommonPoints = value;
            if (value <= 0)
                WinAndDestroy();
        }
    }

    public int RightCommonPoints
    {
        get => rightCommonPoints;
        private set
        {
            OnDecreaseRightPoints?.Invoke(value);
            rightCommonPoints = value;
            if (value <= 0)
                WinAndDestroy();
        }
    }

    public BattleWarCondition(TeamData leftTeam,TeamData rightTeam)
    {
        healthPointsBuffer = new Dictionary<string, int>();
        leftCommonPoints = CalcCommonHealth(leftTeam.GetAllAvailableUnits(), leftTeam.GetAllBuildings());
        rightCommonPoints = CalcCommonHealth(rightTeam.GetAllAvailableUnits(), rightTeam.GetAllBuildings());

        needToDestroy.Add("Main Station", 1);
        needToDestroy.Add("Casern", 1);
    }

    /// <summary>
    /// »нициализаци€ должна быть после инциализации тимы, и до инстацировани€ юнитов и зданий
    /// </summary>
    public void Init(TeamManager leftTeam,TeamManager rightTeam,GameTimer gameTimer)
    {
        leftTeam.OnAddUnit += (unit) => 
        {
            healthPointsBuffer[unit.GO.name] = ControlTeamPoints(unit);
            (unit.Health as Health).OnDead += () =>
            {
                LeftCommonPoints -= healthPointsBuffer[unit.GO.name];
            };
        };
        rightTeam.OnAddUnit += (unit) =>
        {
            healthPointsBuffer[unit.GO.name] = ControlTeamPoints(unit);
            (unit.Health as Health).OnDead += () =>
            {
                IsAllDestroyed(unit);
                RightCommonPoints -= healthPointsBuffer[unit.GO.name];
            };
        };
        gameTimer.OnEverySecond += (time) =>
        { 
            if (time.TotalSeconds <= 0)
            {
                WinAndDestroy();
            }
        };
    }

    /// <summary>
    /// подсчитывает и преобразовывает здоровье в очки комманды, стены прибовл€ют значительно меньше очков
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
   private int CalcCommonHealth(Dictionary<Unit,int> units,TeamData.BuildingData[] buildings)
    {
        int comonHealth = 0;
        foreach (var keyValue in units)
        {
            healthPointsBuffer[keyValue.Key.GO.name] = ControlTeamPoints(keyValue.Key);
            comonHealth += ControlTeamPoints(keyValue.Key) * keyValue.Value;
        }
        foreach (var build in buildings)
        {
            healthPointsBuffer[build.prefab.GO.name] = ControlTeamPoints(build.prefab);
            comonHealth += ControlTeamPoints(build.prefab.GetComponent<IUnit>());
          //  Debug.Log($" commonHealth +={ControlTeamPoints(build.prefab.GetComponent<IUnit>())} Name:{build.prefab.name}");
        }
       // Debug.Log($"Buildings:{buildings.Length} Units:{units.Count} commonHealth:{comonHealth}");
        return comonHealth;
    }

    private void WinAndDestroy()
    {
        OnEndGame?.Invoke(rightCommonPoints <= 0);
        OnEndGame = null;
        OnDecreaseLeftPoints = null;
        OnDecreaseRightPoints = null;
    }

    private int ControlTeamPoints(IUnit unit)
    {
        switch (unit.Entity)
        {
            case Entity.Wall:
                return 0;
            case Entity.SimpleUnit:
                break;
            case Entity.EarnerUnit:
                break;
            case Entity.DefenceBuilding:
                break;
            case Entity.EarnerBuilding:
                break;
        }
        return unit.Health.Points;
    }

    private void IsAllDestroyed(IUnit deadUnit)
    {

        if (needToDestroy.ContainsKey(deadUnit.GO.name))
        {
            needToDestroy[deadUnit.GO.name]--;
            bool allDestroyed = true;
            foreach (var item in needToDestroy)
            {
                if (item.Value > 0)
                {
                    allDestroyed = false;
                    break;
                }
            }
            if (allDestroyed)
            {
                OnEndGame?.Invoke(true);
                OnEndGame = null;
                OnDecreaseLeftPoints = null;
                OnDecreaseRightPoints = null;
            }
        }
    }

}
