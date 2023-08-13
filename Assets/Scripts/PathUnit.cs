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

    private void Start()
    {
        LevelGeneration.Instance.OnAllRoomsCreated += Instance_OnAllRoomsCreated;
    }

    private void Instance_OnAllRoomsCreated(object sender, System.EventArgs e)
    {
        PathRequestManager.Instance.RequestPath(transform.position, target.position, onPathFound);
    }

    private void Update()
    {
        
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
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed*Time.deltaTime);
            yield return null;
        }
    }
    private void OnDestroy()
    {
        LevelGeneration.Instance.OnAllRoomsCreated -= Instance_OnAllRoomsCreated;
    }
}
