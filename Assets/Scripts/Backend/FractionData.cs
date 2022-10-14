using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Общие данные о всех юнитах текущей фракции, главный метод TryGetUnitData(Unit unitPrefab, out UnitData unitData)
/// </summary>
[CreateAssetMenu(fileName = "NewFractionData", menuName = "ScriptableObjects/Add New Fraction Data", order = 3)]
public class FractionData : ScriptableObject
{
    [SerializeField] private string fractionName;
    [SerializeField] private Image fractionAvatar;
    [SerializeField] private List<UnitData> units;
    [SerializeField] private List<BuildingData> buildings;

    public string GetFractionName()
    {
        return fractionName;
    }
    public Image GetFractionAvatar()
    {
        return fractionAvatar;
    }

    /// <summary>
    /// Получает данные об отряде фракции через сам отряд
    /// </summary>
    /// <returns>True - если текущий отряд существует в фракции</returns>
    public bool TryGetUnitData(Unit unitPrefab, out UnitData targetData)
    {
        targetData = units.Find((x) => x.prefab == unitPrefab);
        if (targetData.prefab)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Получает данные об отряде фракции через сам отряд
    /// </summary>
    /// <returns>True - если текущий отряд существует в фракции</returns>
    public bool TryGetBuildingData(BasingBuilding buildingPrefab, out BuildingData targetData)
    {
        targetData = buildings.Find((x) => x.prefab == buildingPrefab);
        if (targetData.prefab)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Получает данные об отряде фракции через сам отряд
    /// </summary>
    /// <returns>True - если текущий отряд существует в фракции</returns>
    public bool TryGetBuildingData(string buildingName, out BuildingData targetData)
    {
        targetData = buildings.Find((x) => x.name == buildingName);
        if (targetData.prefab)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public BuildingData[] GetAllBuildings()
    {
        return buildings.ToArray();
    }

    /// <summary>
    /// Автоматический поиск данных юнита из всех фракций в АССЕТЕ
    /// </summary>
    public static bool TryGetUnitDataFromAllFractions(Unit unitPrefab, out UnitData targetData)
    {
        FractionData[] fractions = Resources.LoadAll<FractionData>("Fractions");

        foreach (FractionData fraction in fractions)
        {
            if(fraction.TryGetUnitData(unitPrefab,out targetData))
            {
                return true;
            }
        }
        targetData = new UnitData();
        return false;
    }

    /// <summary>
    /// Автоматический поиск данных здания из всех фракций в АССЕТЕ
    /// </summary>
    public static bool TryGetBuildingDataFromAllFractions(BasingBuilding buildingPrefab, out BuildingData targetData)
    {
        FractionData[] fractions = Resources.LoadAll<FractionData>("Fractions");

        foreach (FractionData fraction in fractions)
        {
            if (fraction.TryGetBuildingData(buildingPrefab, out targetData))
            {
                return true;
            }
        }
        targetData = new BuildingData();
        return false;
    }
    /// <summary>
    /// Автоматический поиск данных здания из всех фракций в АССЕТЕ
    /// </summary>
    public static bool TryGetBuildingDataFromAllFractions(string buildingName, out BuildingData targetData)
    {
        FractionData[] fractions = Resources.LoadAll<FractionData>("Fractions");

        foreach (FractionData fraction in fractions)
        {
            if (fraction.TryGetBuildingData(buildingName, out targetData))
            {
                return true;
            }
        }
        targetData = new BuildingData();
        return false;
    }

    public static BuildingData[] GetFractionBuildingsData(TeamData team)
    {
        FractionData[] fractions = Resources.LoadAll<FractionData>("Fractions");
        var availableBuildings = team.GetAllBuildings();
        List<BuildingData> list = new List<BuildingData>();


        foreach (FractionData fraction in fractions)
        {
            foreach(TeamData.BuildingData item in availableBuildings)
            {
                BuildingData targetBuild;
                if (fraction.TryGetBuildingData(item.prefab, out targetBuild))
                {
                    list.Add(targetBuild);
                }
            }
        }

        return list.ToArray();
    }

    /// <summary>
    /// Возвращает необходимые данные согласно списку, если buildingNames пустой возвращает все данные указанной фракции
    /// </summary>
    public static BuildingData[] GetBuildingDataFromFraction(string fractionName,params string[] buildingNames)
    {
        List<BuildingData> dataList;
        if (buildingNames.Length == 0)
        {
            return GetFraction(fractionName).GetAllBuildings();
        }
        else
        {
            dataList = new List<BuildingData>();
        }

        foreach (string name in buildingNames)
        {
            foreach (var building in GetFraction(fractionName).GetAllBuildings())
            {
                if (building.name == name)
                {
                    dataList.Add(building);
                }
            }
        }
        return dataList.ToArray();
    }

    /// <summary>
    /// Возвращает необходимые данные согласно списку, если список unitNames пустой возвращает все данные указанной фракции
    /// </summary>
    public static UnitData[] GetUnitsDataFromFraction(string fractionName, params string[] unitNames)
    {
        List<UnitData> dataList;
        if(unitNames.Length == 0)
        {
            return GetFraction(fractionName).units.ToArray();
        }
        else
        {
            dataList = new List<UnitData>();
        }

        foreach (string name in unitNames)
        {
            foreach (var unit in GetFraction(fractionName).units)
            {
                if (unit.name == name)
                {
                    dataList.Add(unit);
                }
            }
        }
        return dataList.ToArray();


    }

    public static FractionData[] GetAllFractions()
    {
       return Resources.LoadAll<FractionData>("Fractions");
    }
    private static FractionData GetFraction(string fractionName)
    {
        foreach(var fraction in GetAllFractions())
        {
            if(fraction.name == fractionName)
            {
                return fraction;
            }
        }
        return null;
    }


    [Serializable]
    public struct UnitData
    {
        [Header("For BattleWarCardUI - UnitName_Text")]
        [SerializeField] public string name;
        [Header("Index")]
        [SerializeField] public Unit prefab;
        [Header("For BattleWarCardUI - cardAvatar_Image")]
        [SerializeField] public Sprite avatar;
        [Header("For BattleWarCardUI - Mode_Image")]
        [SerializeField] public Sprite mode;
        [SerializeField] public int cost;
        [SerializeField] public int count;
    }

    [Serializable]
    public struct BuildingData
    {
        [SerializeField] public string name;
        [Header("Index")]
        [SerializeField] public BasingBuilding prefab;
        [SerializeField] public int armor;
        [SerializeField] public Vector2Int scale;
        [SerializeField] public int cost;
    }
}
