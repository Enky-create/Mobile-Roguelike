using System;
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
    Heap<PathfindingNode> openHeap;
    HashSet<PathfindingNode> closedList;
    public Pathfinding(int width, int height)
    {
        if (Instance == null)
        {
            Instance = this;
            grid = new GenericGrid<PathfindingNode>(
            width,
            height,
            1,
            new Vector3(-10, -20),// start position
            (GenericGrid<PathfindingNode> g, int x, int y) => new PathfindingNode(x, y, g));
            Debug.Log("Pathfinding instance  exists");
        }
        else
        {
            Debug.Log("Pathfinding instance already exists");
        }
    }

    public IEnumerator FindPath(Vector3 start, Vector3 end)
    {
        
        grid.GetXY(start,out int startX,out int startY);
        grid.GetXY(end, out int endX, out int endY);
        
        
        
        Vector3[] path = FromPathfindingNodeToVector3Simplify(FindPath(startX, startY, endX, endY));

        Debug.Log("HERE IS path" + path == null);
        yield return null;
        
        PathRequestManager.Instance.FinishedProcessingath(path, path != null ? true : false);
        
    }
    public Vector3[] FromPathfindingNodeToVector3Simplify(List<PathfindingNode> path)
    {
        if (path != null && path.Count > 0)
        {
            List<Vector3> vector3Path = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i].x - path[i - 1].x, path[i].y - path[i - 1].y);
                if (directionNew != directionOld)
                {
                    vector3Path.Add(grid.GetWorldPoition(path[i].x, path[i].y));
                }
                directionOld = directionNew;
            }
            vector3Path.Add(grid.GetWorldPoition(path[path.Count - 1].x, path[path.Count - 1].y));
            return vector3Path.ToArray();
        }
        return null;
    }
    public List<PathfindingNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathfindingNode startNode = grid.GetObject(startX, startY);
        PathfindingNode endNode = grid.GetObject(endX, endY);
        if (endNode == null) return null;
        if (startNode == endNode) return null;
        openHeap = new Heap<PathfindingNode>(grid.GetWidth()*grid.GetHeight());
        openHeap.Add(startNode);
        closedList = new HashSet<PathfindingNode>();

        for(int x=0; x<grid.GetWidth(); x++)
        {
            for(int y = 0; y<grid.GetHeight(); y++)
            {
                PathfindingNode node = grid.GetObject(x, y);
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanse(startNode, endNode);
        startNode.CalculateFCost();
        while (openHeap.Count > 0)
        {
            PathfindingNode currentNode = openHeap.RemoveFirstItem();
            if(currentNode == endNode)
            {
                // reached endNode
                return CalculatePath(endNode);
            }
            
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
                    if (!openHeap.Contains(neighbourNode))
                    {
                        openHeap.Add(neighbourNode);
                    }
                    else
                    {
                        openHeap.UpdateItem(neighbourNode);
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
    //private PathfindingNode GetTheLowestFCostNode()
    //{
    //    PathfindingNode lowestFCostNode = openHeap[0];
    //    for(int i=1; i<openHeap.Count; i++)
    //    {
    //        if(openHeap[i].fCost< lowestFCostNode.fCost)
    //        {
    //            lowestFCostNode = openHeap[i];
    //        }
    //    }
    //    return lowestFCostNode;
    //}
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
