using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnRandomWall : MonoBehaviour
{
    [SerializeField]
    GameObject[] walls;
    void Start()
    {
        int randomWall = Random.Range(0, walls.Length);
        GameObject instance = Instantiate(walls[randomWall], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
        PathfindingNode node = Pathfinding.Instance.GetGrid().GetObject(transform.position);
        node.SetIsWalkable(false);
    }
}
