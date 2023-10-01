using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnRandomWall : MonoBehaviour
{
    [SerializeField]
    GameObject[] walls;
    void Awake()
    {
        //LevelGeneration.Instance.OnAllRoomsCreated += Instance_OnAllRoomsCreated;
        int randomWall = Random.Range(0, walls.Length);
        GameObject instance = Instantiate(walls[randomWall], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
        PathfindingNode node = Pathfinding.Instance.GetGrid().GetObject(transform.position);
        node.SetIsWalkable(false);
    }

    //private void Instance_OnAllRoomsCreated(object sender, System.EventArgs e)
    //{
    //    SetIsWalkable();
    //}
    private void OnDestroy()
    {
        //LevelGeneration.Instance.OnAllRoomsCreated -= Instance_OnAllRoomsCreated;
        PathfindingNode node = Pathfinding.Instance.GetGrid().GetObject(transform.position);
        node.SetIsWalkable(true);
    }
    public void SetIsWalkable()
    {
        PathfindingNode node = Pathfinding.Instance.GetGrid().GetObject(transform.position);
        node.SetIsWalkable(false);
    }
}
