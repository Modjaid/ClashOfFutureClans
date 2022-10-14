using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTestGenerator
{
    private float Xdistance = 1.05f;
    private float Ydistance = 1.03f;

    private int mapScale;
    private Transform mapParent;

    public Dictionary<Vector2Int, GameObject> nodes;


    public MapTestGenerator(GameObject mapMeshPrefab, int mapScale)
    {

        mapParent = new GameObject().transform;
        mapParent.name = "MapTest";
        this.mapScale = mapScale;
        GenerationMap(mapMeshPrefab);
    }

    public void ClearMap()
    {
        for (int x = 0; x < mapScale; x++)
        {
            for (int y = 0; y < mapScale; y++)
            {
                GameObject targetMesh;
                if (nodes.TryGetValue(new Vector2Int(x, y), out targetMesh))
                {
                    targetMesh.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
    }

    public void ActiveSectionColors(bool[,] grid)
    {
        for(int x = 0; x < mapScale; x++)
        {
            for(int y = 0; y < mapScale; y++)
            {
                if (grid[x, y])
                {
                    GameObject targetMesh;
                    if (nodes.TryGetValue(new Vector2Int(x, y), out targetMesh))
                    {
                        targetMesh.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
        }
    }

    private void GenerationMap(GameObject mapMeshPrefab)
    {
        nodes = new Dictionary<Vector2Int, GameObject>();
        Vector3 currentPos = Vector3.zero;
        for (int x = 0; x < mapScale; x++)
        {
            for (int y = 0; y < mapScale; y++)
            {
                var newMesh = GameObject.Instantiate(mapMeshPrefab, mapParent);
                newMesh.transform.position = currentPos;
                nodes[new Vector2Int(x, y)] = newMesh.gameObject;
                currentPos.x += Xdistance;
            }
            currentPos.z += Ydistance;
            currentPos.x = 0;
        }
    }

}