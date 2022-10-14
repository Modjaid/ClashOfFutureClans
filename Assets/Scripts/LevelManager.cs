using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVO;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private BattleWarUI warUI;

    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private UnitCreator unitCreator;
    [SerializeField] private AutoUnitCreator autoUnitCreator;
    [SerializeField] private Collider ground;
    [SerializeField] private GameObject winLose;
    [SerializeField] private int baseScale;
    [SerializeField] private HealthCanvas healthCanvas;

    [SerializeField] private TeamData leftTeamData;
    [SerializeField] private TeamData rightTeamData;
    [SerializeField] private Vector3 move;

    private TeamManager leftTeam;
    private TeamManager rightTeam;

    private Map map;
    private BattleWarCondition matchCondition;
    private AutoBuildingCreator autoBuildingCreator;
    public Bounds BaseCollider
    {
        get
        {
            return new Bounds(new Vector3(move.x + baseScale, 0, move.z + baseScale),new Vector3(baseScale*2, 0.5f, baseScale*2));
        }
    }

    private void Start()
    {
        map = new Map();
        InitTeams();

        rightTeamData.RemoveAllBuildings(); 
        rightTeamData.AddBuildings(InitRandomBuildingsData_BSP());

        matchCondition = new BattleWarCondition(leftTeamData, rightTeamData);
        matchCondition.Init(leftTeam, rightTeam, gameTimer);

        Simulator.Instance.Clear();
        Simulator.Instance.setAgentDefaults(15.0f, 10, 5.0f, 5.0f, 2.0f, 2.0f, new RVO.Vector2(0.0f, 0.0f));
        healthCanvas.Init(leftTeam, rightTeam);

        var rewarder = new RewardCalculator(matchCondition, winLose);
        if (autoUnitCreator)
        {
            autoUnitCreator.Init(rightTeamData,map);
            autoUnitCreator.OnCreateNewUnit += (go) => rightTeam.AddUnit(go.GetComponent<IUnit>());
        }
        unitCreator.OnCreateNewUnit += (go) => leftTeam.AddUnit(go.GetComponent<IUnit>());
        InstanceTeamBuildings(rightTeamData,rightTeam);
        warUI.Init(leftTeamData, rightTeamData, leftTeam, rightTeam, gameTimer, unitCreator, matchCondition);
        autoUnitCreator.StartRandomInstantiate();
        gameTimer.StartTimer(300);
    }

    private void InitTeams()
    {
        if (leftTeam == null)
        {
            leftTeam = new TeamManager(map, false, this);
        }
        if (rightTeam == null)
        {
            rightTeam = new TeamManager(map, true, this);
        }

        leftTeam.enemies = rightTeam;
        rightTeam.enemies = leftTeam;
    }


    /// <summary>
    /// генерирует данные о зданиях с рандомным позиционированием, сначала "main Station" затем остальные
    /// </summary>
    private TeamData.BuildingData[] InitRandomBuildingsData_Old()
    {
        if (autoBuildingCreator)
        {
            var fractionData = FractionData.GetBuildingDataFromFraction("Jiro", "Main Station", "Casern", "TowerCP", "Wall");
            var newBuildingsData = autoBuildingCreator.GenerateBuildingsWithRandomPos(new TeamData.BuildingData[0], fractionData);
            var newWalsData = autoBuildingCreator.GenerateWallsForTeam(newBuildingsData.ToArray());
            newBuildingsData.AddRange(newWalsData);
            return newBuildingsData.ToArray();
        }
        else
            return null;
    }

    private TeamData.BuildingData[] InitRandomBuildingsData_BSP()
    {
        var fractionData = FractionData.GetBuildingDataFromFraction("Jiro", "Main Station", "Casern", "TowerCP");
        var wall = FractionData.GetBuildingDataFromFraction("Jiro", "Wall")[0];
        var cornerWall = FractionData.GetBuildingDataFromFraction("Jiro", "Corner Wall")[0];
        var teamBuildings = new List<TeamData.BuildingData>();
        Room baseGrid = new Room(baseScale, baseScale);

        var rooms = baseGrid.BSP();

        for (int i = 0; i < fractionData.Length; i++)
        {
            Vector2Int buildingPos = new Vector2Int(rooms[i].x + 4, rooms[i].z + 4);
            teamBuildings.Add(new TeamData.BuildingData() { prefab = fractionData[i].prefab, position = buildingPos });
        }

        ////////////// WALLS /////////////////////////////////////////////////////////////////
        for (int i = 0; i < baseGrid.width; i++)
        {
            Vector2Int pos = new Vector2Int();
            pos.x = 0;
            pos.y = i;
            var rotation = 0;
            if (i == 0)
            {
                //  rotation = -45;
                teamBuildings.Add(new TeamData.BuildingData() { prefab = cornerWall.prefab, position = pos, rotation = -90 });
            }
            else if (i == baseGrid.width - 1)
            {
                // rotation = 45;
                teamBuildings.Add(new TeamData.BuildingData() { prefab = cornerWall.prefab, position = pos, rotation = 0 });
            }
            else
            {
                teamBuildings.Add(new TeamData.BuildingData() { prefab = wall.prefab, position = pos, rotation = rotation });
            }
        }
        for (int i = 0; i < baseGrid.width; i++)
        {
            Vector2Int pos = new Vector2Int();
            pos.x = baseGrid.width;
            pos.y = i;
            var rotation = 0;
            if (i == 0)
            {
                //  rotation = -45;
                teamBuildings.Add(new TeamData.BuildingData() { prefab = cornerWall.prefab, position = pos, rotation = -180 });
            }
            else if (i == baseGrid.width - 1)
            {
                // rotation = 45;
                teamBuildings.Add(new TeamData.BuildingData() { prefab = cornerWall.prefab, position = pos, rotation = -270 });
            }
            else
            {
                teamBuildings.Add(new TeamData.BuildingData() { prefab = wall.prefab, position = pos, rotation = rotation });
            }
        }
        for (int i = 1; i < baseGrid.length; i++)
        {
            Vector2Int pos = new Vector2Int();
            pos.x = i;
            pos.y = 0;
            var rotation = 90;
            teamBuildings.Add(new TeamData.BuildingData() { prefab = wall.prefab, position = pos, rotation = rotation });
        }
        for (int i = 1; i < baseGrid.length; i++)
        {
            Vector2Int pos = new Vector2Int();
            pos.x = i;
            pos.y = baseGrid.length - 1;
            var rotation = 90;
            teamBuildings.Add(new TeamData.BuildingData() { prefab = wall.prefab, position = pos, rotation = rotation });
        }
        //////////////////////////////////////////////////////////////////////////////////////

        return teamBuildings.ToArray();
    }
    private void InstanceTeamBuildings(TeamData teamData,TeamManager teamManager)
    {
        var fractiondData = new List<FractionData.BuildingData>(FractionData.GetFractionBuildingsData(teamData));
        foreach (TeamData.BuildingData building in teamData.GetAllBuildings())
        {
            var newGo = Instantiate(building.prefab);
            newGo.transform.position = new Vector3(building.position.x, 0, building.position.y);
            newGo.transform.rotation = Quaternion.Euler(0, building.rotation, 0);
            var scaledPos = newGo.transform.position;
            scaledPos.Scale(newGo.transform.localScale);
            scaledPos += move;
            newGo.transform.position = scaledPos;
            newGo.name = fractiondData.Find((data) => data.prefab == building.prefab).name;
            teamManager.AddUnit(newGo);
        }
    }


    
    public void Update()
    {
        Simulator.Instance.setTimeStep(Time.deltaTime * 60 * 0.25f);
        Simulator.Instance.doStep();
        //map.Visualize();
    }
    

}
