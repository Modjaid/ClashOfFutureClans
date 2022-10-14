using UnityEngine;

[System.Diagnostics.DebuggerDisplay("FCost={FCost}, gCost={gCost}, hCost={hCost}, position={position}")]
public class Node
{
    public Node previous;
    public (int x, int y) position;

    public int gCost = 0;
    public int hCost = 0;
    public int cCost = 0;
    public int FCost => gCost + hCost;

}