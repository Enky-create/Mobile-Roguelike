using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUnit : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float speed;
    private Vector3[] path;
    int targetIndex = 0;

    private void Update()
    {
        if (LevelGeneration.Instance.isPathCreated)
        {
            PathRequestManager.Instance.RequestPath(transform.position, target.position, onPathFound);
            
        }
    }
    public void onPathFound(Vector3[] newPath, bool IsPathSuccessful)
    {
        Debug.Log("PAth++ " + newPath==null +" "+ IsPathSuccessful);
        if (IsPathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    IEnumerator FollowPath()
    {
       
        Vector3 currentWayPoint = path[0];
        while (true)
        {
            if(transform.position== currentWayPoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWayPoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed);
            yield return null;
        }
    }
}
