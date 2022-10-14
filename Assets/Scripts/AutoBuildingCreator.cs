using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class AutoBuildingCreator : MonoBehaviour
{
    public event Action<GameObject> OnCreateNewBuilding;


    [SerializeField] public GameObject mapMeshPrefabForTest;
    [SerializeField] public int virtualGridScale = 30;

    [HideInInspector] public bool[,] virtualGrid;

    private int Normalize { get { return virtualGridScale / 2; } }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(virtualGridScale, 0f, virtualGridScale));
    }

    public List<TeamData.BuildingData> GenerateBuildingsWithRandomPos(TeamData.BuildingData[] existBuildings, params FractionData.BuildingData[] buildings)
    {
        var newData = new List<TeamData.BuildingData>();
        virtualGrid = InitGrid();
        ReserveExistBuildings(existBuildings);
        

        foreach (var data in buildings)
        {
            var buildingPos = GetRandomPos(data.scale);
            ReserveSections(data.scale, buildingPos);
            newData.Add(new TeamData.BuildingData() { prefab = data.prefab, position = buildingPos });
        }
        return newData;
    }

    public List<TeamData.BuildingData> GenerateWallsForTeam(TeamData.BuildingData[] existBuildings)
    {
        var wallData = FractionData.GetBuildingDataFromFraction("Jiro", "Wall")[0];
        var newData = new List<TeamData.BuildingData>();
        virtualGrid = InitGrid();
        ReserveExistBuildings(existBuildings);
        var nextPos = Vector2Int.zero;
        var boundGrid = InitBoundGrid();
        


        for(int x = 0; x < virtualGridScale; x++)
        {
            for(int y = 0; y < virtualGridScale; y++)
            {
                if (boundGrid[x, y].IsBound)
                {
                    int rotation = (boundGrid[x, y].IsHorizontal) ? 90 : 0;
                    newData.Add(new TeamData.BuildingData() { prefab = wallData.prefab, position = new Vector2Int(x, y), rotation = rotation });
                    ReserveSections(wallData.scale, new Vector2Int(x, y));
                }
            }
        }
        return newData;

    }



    private Vector2Int GetRandomPos(Vector2Int buildingScale)
    {
        int collisionCount = 0;

        Vector2Int pos = new Vector2Int(Normalize, Normalize);
        
        Vector2Int randDirection = new Vector2Int(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2));
        while (randDirection == Vector2Int.zero)
        {
            randDirection = new Vector2Int(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2));
        }

        while (IsCollision(buildingScale, pos) && collisionCount< 50)
        {
            collisionCount++;
            pos += randDirection;
        }
        return pos;
    }

    private bool IsCollision(Vector2Int buildingScale, Vector2Int buildingPos)
    {
        for (int x = buildingPos.x; x < buildingPos.x + buildingScale.x; x++)
        {
            for (int y = buildingPos.y; y <buildingPos.y + buildingScale.y; y++)
            {
                if (x >= virtualGridScale -1 || y >= virtualGridScale - 1) return true;

                if (virtualGrid[x,y])
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void ReserveSections(Vector2Int scale, Vector2Int pos)
    {

        for (int x = pos.x - 3; x< pos.x+ scale.x ; x++)
        {
            for(int y = pos.y - 3; y< pos.y+ scale.y; y++)
            {
                if (x >= virtualGridScale || y >= virtualGridScale) return;
                virtualGrid[x, y] = true;
            }
        }
    }



    private bool[,] InitGrid()
    {
        bool[,] arr= new bool[virtualGridScale, virtualGridScale];
        for(int x = 0; x < virtualGridScale; x++)
        {
            for(int y = 0; y < virtualGridScale; y++)
            {
                arr[x, y] = false;
            }
        }
        return arr;
    }
    private void ReserveExistBuildings(TeamData.BuildingData[] buildings)
    {

        foreach (var building in buildings)
        {
            FractionData.BuildingData buildingData;
            if (FractionData.TryGetBuildingDataFromAllFractions(building.prefab, out buildingData))
            {
                ReserveSections(buildingData.scale, building.position);
            }
        }
    }

    private Node[,] InitBoundGrid()
    {
        Node[,] boundGrid = new Node[virtualGridScale, virtualGridScale];



        for (int x = 0; x < virtualGridScale; x++)
        {
            bool isXInside = false;

            for (int y = 0; y < virtualGridScale; y++)
            {

                if (virtualGrid[x, y] && !isXInside)
                {
                    isXInside = true;
                    boundGrid[x, y].IsBound = true;
                    boundGrid[x, y].IsHorizontal = true;
                }
                else if(virtualGrid[x, y] && isXInside)
                {
                    boundGrid[x, y].IsBound = false;
                    boundGrid[x, y].IsHorizontal = true;
                }else if(!virtualGrid[x, y] && isXInside)
                {
                    boundGrid[x, y].IsBound = true;
                    boundGrid[x, y].IsHorizontal = true;
                    boundGrid[x, y].IsBound = true;
                    isXInside = false;
                }
            }
        }

        for (int y = 0; y < virtualGridScale; y++)
        {
            bool isYInside = false;

            for (int x = 0; x < virtualGridScale; x++)
            {

                if (virtualGrid[x, y] && !isYInside)
                {
                    isYInside = true;
                    boundGrid[x, y].IsBound = true;
                    boundGrid[x, y].IsHorizontal = false;
                }
                else if (virtualGrid[x, y] && isYInside)
                {
                    boundGrid[x, y].IsBound = false;
                    boundGrid[x, y].IsHorizontal = true;
                }
                else if (!virtualGrid[x, y] && isYInside)
                {
                    boundGrid[x, y].IsBound = true;
                    boundGrid[x, y].IsHorizontal = false;
                    isYInside = false;
                }
            }
        }

        return boundGrid;
    }

    private FractionData.BuildingData GetMostBigBuilding(FractionData.BuildingData[] buildings)
    {
        int maxSpace = 0;
        var bigBuilding = new FractionData.BuildingData();
        foreach (var data in buildings)
        {
            int sum = data.scale.x + data.scale.y;
            if (sum > maxSpace)
            {
                maxSpace = sum;
                bigBuilding = data;
            }
        }
        return bigBuilding;
    }

    internal struct Node {
        public bool IsBound;
        public bool IsHorizontal;
    }


}
