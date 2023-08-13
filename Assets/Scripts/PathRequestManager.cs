using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    Queue<PathRequest> pathRequestQueue=new Queue<PathRequest>();
    PathRequest currentPathRequest;
    bool isProcessingPath;
    Pathfinding pathfinding;
    public static PathRequestManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            new Pathfinding(width,height);
        }
        else
        {
            Debug.Log("PathRequestManager alreaady exists");
        }
        
    }
    public void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        
        PathRequest pathRequest = new PathRequest(pathStart, pathEnd, callback);
        
        Instance.pathRequestQueue.Enqueue(pathRequest);
        Instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath&& pathRequestQueue.Count>0)
        {
            
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            StartPathfinding(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }
    private void StartPathfinding(Vector3 start, Vector3 end)
    {
       
        
        StartCoroutine(Pathfinding.Instance.FindPath(start, end));
        
    }
    
    public void FinishedProcessingath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;
        public  PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            this.pathStart = pathStart;
            this.pathEnd = pathEnd;
            this.callback = callback;
        }
    }
}
