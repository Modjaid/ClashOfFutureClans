using System.Collections.Generic;
using UnityEngine;

public class Pathfind
{
    private readonly PathGrid grid;
    private readonly float cCoeff;
    private readonly float hCoeff;

    public Pathfind(Map map, float cCoeff, float hCoeff)
    {
        grid = new PathGrid(map);
        this.cCoeff = cCoeff;
        this.hCoeff = hCoeff;
    }


    public List<Vector3> FindPath((int x, int y) start, (int x, int y) end)
    {
        if (grid.Count < 100 && grid.HasNode(end))
            grid.Reset();
        else
            grid.Clear();
        var a = grid.GetNode(start);
        var e = grid.GetNode(end);
        //grid.SetNullToUnit(end);
        HashSet<Node> open = new HashSet<Node>() { a };
        HashSet<Node> closed = new HashSet<Node>();
        a.hCost = a.position.Manhattan(e.position);
        while (open.Count != 0)
        {
            Node current = null;
            int min = int.MaxValue;
            foreach (var item in open)
            {
                if (item.FCost < min)
                {
                    min = item.FCost;
                    current = item;
                }
            }
            if (current == e)
            {
                List<Vector3> path = new List<Vector3>();
                while (current.previous != null)
                {
                    path.Add(new Vector3(current.position.x, 0, current.position.y));
                    current = current.previous;
                }
                path.Reverse();
                return path;
            }
            open.Remove(current);
            closed.Add(current);
            var neighbours = GetNeighbourList(current);
            for (int i = 0; i < neighbours.Length; i++)
            {
                if (closed.Contains(neighbours[i]))
                    continue;
                var gCostFromCurrent = current.gCost + (int)(current.position.PathDistance(neighbours[i].position) * hCoeff) + (int)(neighbours[i].cCost * cCoeff);
                if (!open.Contains(neighbours[i]))
                {
                    open.Add(neighbours[i]);
                    neighbours[i].previous = current;
                    neighbours[i].gCost = gCostFromCurrent;
                    neighbours[i].hCost = neighbours[i].position.Manhattan(e.position);
                }
                else
                {
                    if (gCostFromCurrent < neighbours[i].gCost)
                    {
                        neighbours[i].previous = current;
                        neighbours[i].gCost = gCostFromCurrent;
                    }
                }
            }
        }
        return null;
    }

    private Node[] GetNeighbourList(Node currentNode)
    {
        Node[] neighbourList = new Node[8];
        for (int i = 0; i < Near.Length; i++)
        {
            neighbourList[i] = grid.GetNode(currentNode.position.Add(Near[i]));
        }
        return neighbourList;
    }

    private static readonly (int x, int y)[] Near = new (int x, int y)[]
    {
        (-1,-1),
        (-1,0),
        (-1,1),
        (0,-1),
        (0,1),
        (1,-1),
        (1,0),
        (1,1),
    };
}
