using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������� ����� ��� �������������� � ����
/// </summary>
[CreateAssetMenu(fileName = "NewTeamData", menuName = "ScriptableObjects/Team Data", order = 2)]
public class TeamData : ScriptableObject
{
    [Header("��������� ����� ��� �������������� � ����")]
    [SerializeField] private List<UnitData> allTeamUnits;

    [Header("��������� ������ � ����")]
    [SerializeField] private List<BuildingData> allTeamBuildings;


    public TeamData()
    {
        allTeamUnits = new List<UnitData>();
        allTeamBuildings = new List<BuildingData>();
    }

    /// <summary>
    /// ���������� ������ �� ������� � �� ���������� ���-��� ��� ��������������
    /// </summary>
    public Dictionary<Unit,int> GetAllAvailableUnits()
    {
        Dictionary<Unit, int> unitDict = new Dictionary<Unit, int>();
        foreach(UnitData item in allTeamUnits)
        {
            unitDict[item.prefab] = item.count;
        }
        return unitDict;
    }

    public BuildingData[] GetAllBuildings()
    {
        return allTeamBuildings.ToArray();
    }

    public void RemoveBuilding(BuildingData building)
    {
        allTeamBuildings.Remove(building);
    }

    public void RemoveAllBuildings()
    {
        allTeamBuildings = new List<BuildingData>();
    }

    public void AddBuilding(BasingBuilding prefab, Vector2Int pos, int rotation = 0)
    {
        allTeamBuildings.Add(new BuildingData() { prefab = prefab, position = pos, rotation = rotation});
    }
    public void AddBuildings(BuildingData[] buildings)
    {
        if(buildings != null)
            allTeamBuildings.AddRange(buildings);
    }
    public void AddUnit(Unit prefab, int count = 1)
    {
        int index = allTeamUnits.FindIndex((x) => x.prefab == prefab);
        if (index > -1)
        {
            var data = allTeamUnits[index];
            data.count += count;
            allTeamUnits[index] = data;
        }
        else
        {
            allTeamUnits.Add(new UnitData() { prefab = prefab, count = count });
        }
    }

    [Serializable]
    public struct UnitData
    {
        [Header("������ �����")]
        [SerializeField] public Unit prefab;
        [Header("���������� ��������� ��� ��������������")]
        [SerializeField] public int count;
    }

    [Serializable]
    public struct BuildingData
    {
        public BasingBuilding prefab;
        [Header("������� ��� ���� �������������� ������")]
        public Vector2Int position;
        public int rotation;
    }
}
