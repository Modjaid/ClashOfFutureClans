using System;
using UnityEngine;

public static class Extensions
{
    public static int PathDistance(this (int x, int y) vec, (int x, int y) to)
    {
        var x = vec.x - to.x;
        var y = vec.y - to.y;
        return x != 0 && y != 0 ? 14 : 10;
    }

    public static int Manhattan(this (int x, int y) vec, (int x, int y) to)
    {
        var x = vec.x - to.x;
        var y = vec.y - to.y;
        return (Math.Abs(x) + Math.Abs(y)) * 10;
    }

    public static (int x, int y) ToTuple(this Vector2Int vec)
    {
        return (vec.x, vec.y);
    }

    public static (int x, int y) ToTuple(this Vector3 vec)
    {
        return (Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.z));
    }

    public static (int x, int y) Add(this (int x, int y) tuple1, (int x, int y) tuple2)
    {
        return (tuple1.x +tuple2.x, tuple1.y + tuple2.y);
    }

    public static int GridDistance(this (int x, int y) vec, (int x, int y) to)
    {
        var x = Math.Abs(vec.x - to.x);
        var y = Math.Abs(vec.y - to.y);
        var max = Math.Max(x, y);
        var min = Math.Min(x, y);
        return (max - min) * 10 + min * 14;
    }

}
