using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public static Pathfinding Instance
    {
        private set; get;
    }
    private const int FORWARD_COST = 10;
    private const int DIAGONAL_COST = 14;

    private GenericGrid<PathfindingNode> grid;
    List<PathfindingNode> openList;
    List<PathfindingNode> closedList;
    public Pathfinding(int width, int height)
    {
        if (Instance == null)
        {
            Instance = this;
            grid = new GenericGrid<PathfindingNode>(width,
            height,
            1,
            new Vector3(-10, -20),
            (GenericGrid<PathfindingNode> g, int x, int y) => new PathfindingNode(x, y, g));
        }
        else
        {
            Debug.Log("Pathfinding instance already exists");
        }
    }
    public List<PathfindingNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathfindingNode startNode = grid.GetObject(startX, startY);
        PathfindingNode endNode = grid.GetObject(endX, endY);
        openList = new List<PathfindingNode> { startNode };
        closedList = new List<PathfindingNode>();

        for(int x=0; x<grid.GetWidth(); x++)
        {
            for(int y = 0; y<grid.GetHeight(); y++)
            {
                PathfindingNode node = grid.GetObject(x, y);
                node.gCost = int.MaxValue;
                //node.SetIsWalkable(true);
                node.CalculateFCost();
                node.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanse(startNode, endNode);
        startNode.CalculateFCost();
        while (openList.Count > 0)
        {
            PathfindingNode currentNode = GetTheLowestFCostNode();
            if(currentNode == endNode)
            {
                // reached endNode
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            
            foreach(PathfindingNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if(!neighbourNode.GetIsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = neighbourNode.gCost + CalculateDistanse(currentNode, neighbourNode);
                if(tentativeGCost< neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanse(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();
                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        return null;
    }
    private int CalculateDistanse (PathfindingNode start, PathfindingNode end)
    {
        int xDiff = Mathf.Abs(start.x - end.x);
        int yDiff = Mathf.Abs(start.y - end.y);
        int diagonal = Mathf.Min(xDiff, yDiff);
        int forward = Mathf.Abs(xDiff - yDiff);
        return diagonal * DIAGONAL_COST + forward * FORWARD_COST;
    }
    private PathfindingNode GetTheLowestFCostNode()
    {
        PathfindingNode lowestFCostNode = openList[0];
        for(int i=1; i<openList.Count; i++)
        {
            if(openList[i].fCost< lowestFCostNode.fCost)
            {
                lowestFCostNode = openList[i];
            }
        }
        return lowestFCostNode;
    }
    private List<PathfindingNode> CalculatePath(PathfindingNode endNode)
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        path.Add(endNode);
        PathfindingNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }
    private List<PathfindingNode> GetNeighbourList(PathfindingNode currentNode)
    {
        List<PathfindingNode> neighbourList = new List<PathfindingNode>();

        if (currentNode.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            if(currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x-1, currentNode.y-1));
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x-1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        return neighbourList;
    }
    private PathfindingNode GetNode(int x, int y)
    {
        return grid.GetObject(x, y);
    }
    public GenericGrid<PathfindingNode> GetGrid()
    {
        return grid;
    }
}
