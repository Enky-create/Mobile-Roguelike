using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class SpawnRooms : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnPoints;
    [SerializeField]
    private LayerMask roomMask;
    private void Start()
    {
        LevelGeneration.Instance.OnAllPathCreated += Instance_OnAllPathCreated;
    }

    private void Instance_OnAllPathCreated(object sender, System.EventArgs e)
    {
        SpawnRandomRooms();
    }

    private void SpawnRandomRooms()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, roomMask);
        if (roomDetection==null)
        {
            int randomRoom = Random.Range(0,LevelGeneration.Instance.rooms.Count);
            int randomVariation = Random.Range(0, LevelGeneration.Instance.rooms[randomRoom].variations.Length);
            Instantiate(LevelGeneration.Instance.rooms[randomRoom].variations[randomVariation], transform.position,Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
