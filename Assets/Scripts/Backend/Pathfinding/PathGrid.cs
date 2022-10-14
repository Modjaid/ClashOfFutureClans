using UnityEngine;
using System.Collections.Generic;

public class PathGrid
{
    private readonly Dictionary<(int x, int y), Node> nodes = new Dictionary<(int x, int y), Node>();
    private readonly Map map;
    public int Count => nodes.Count;

    public void Clear()
    {
        nodes.Clear();
    }

    public void Reset()
    {
        foreach (var item in nodes)
        {
            var value = item.Value;
            value.previous = null;
            value.gCost = 0;
            value.hCost = 0;

            var u = map.GetUnit(item.Key);
            if (u != null)
                value.cCost = u.Health.Points;
        }
    }

    public Node this[(int x, int y) pos]
    {
        get
        {
            if (nodes.TryGetValue(pos, out Node n))
                return n;
            else
            {
                var node = new Node()
                {
                    position = pos
                };
                if (map.GetUnit(pos) != null)
                    node.cCost = map.GetUnit(pos).Health.Points;
                nodes.Add(pos, node);
                return node;
            }
        }
    }


    public PathGrid(Map map)
    {
       // width = map.width;
       // height = map.height;
        this.map = map;
    }


    public Node GetNode((int x, int y) coords)
    {
        return this[coords];
    }

    public void SetNullToUnit((int x, int y) coords)
    {
        var u = map.GetUnit(coords);
        if (u == null)
            return;
        var l = map.GetPos(u);
        for (int i = 0; l != null && i < l.Count; i++)
        {
            this[l[i]].cCost = 0;
        }
    }

    public Node GetNode(Vector3 coords3)
    {
        var coords = (Mathf.RoundToInt(coords3.x), Mathf.RoundToInt(coords3.z));
        return GetNode(coords);
    }

    public bool HasNode((int x, int y) coords)
    {
        return nodes.ContainsKey(coords);
    }

    public bool HasNode(Vector3 coords3)
    {
        var coords = (Mathf.RoundToInt(coords3.x), Mathf.RoundToInt(coords3.z));
        return HasNode(coords);
    }
}
