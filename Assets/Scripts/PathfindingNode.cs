using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode: IHeapItem<PathfindingNode>
{
    public int x;
    public int y;
    private GenericGrid<PathfindingNode> grid;
    private bool isWalkable=true;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathfindingNode cameFromNode;

    int indexInHeap;
    public int heapIndex { 
        get { return indexInHeap; } 
        set {
            indexInHeap = value;
        }
    }

    public PathfindingNode(int x, int y, GenericGrid<PathfindingNode> grid)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public override string ToString()
    {
        return isWalkable.ToString();
    }
    public void SetIsWalkable(bool value)
    {
        isWalkable = value;
        grid.TriggerGridObjectChange(x, y);
    }
    public bool GetIsWalkable()
    {
        return isWalkable;
    }

    public int CompareTo(PathfindingNode other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }
        return -compare;
    }
}
