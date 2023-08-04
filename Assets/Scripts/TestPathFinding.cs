using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TestPathFinding : MonoBehaviour
{
    private Pathfinding pathfinding;
    
    void Awake()
    {
        pathfinding = new Pathfinding(40, 40);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 muosePosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(muosePosition, out int x, out int y);
            List<PathfindingNode> path = pathfinding.FindPath(1, 1, x, y);
            Debug.Log("PATH");
            Debug.Log(path?.Count);
            if (path != null)
            {
                for(int i=0; i < path.Count-1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 1 + Vector3.one * 1 + new Vector3(-10, -20), new Vector3(path[i + 1].x, path[i + 1].y) * 1 + Vector3.one * 1 + new Vector3(-10, -20), Color.green, 1000);
                    
                }
                
            }
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 muosePosition = UtilsClass.GetMouseWorldPosition();
            PathfindingNode pathNode = pathfinding.GetGrid().GetObject(muosePosition);
            bool isWalkable = pathNode.GetIsWalkable();
            isWalkable = !isWalkable;
            pathNode.SetIsWalkable(isWalkable);
        }
    }
}
