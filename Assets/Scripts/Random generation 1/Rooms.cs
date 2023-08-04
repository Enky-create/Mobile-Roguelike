using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    public GameObject[] LeftRooms;
    public GameObject[] RightRooms;
    public GameObject[] TopRooms;
    public GameObject[] BottomRooms;
    public GameObject ClosedRoom;
    public List<GameObject> spawnedRooms;
    [SerializeField] private float spawnTime;
    [SerializeField] private GameObject boss;
    private bool bossSpawned = false;
    private void Update()
    {
        if(spawnTime < 0 && !bossSpawned)
        {
            Instantiate(boss, spawnedRooms[spawnedRooms.Count-1].transform.position, Quaternion.identity);
            bossSpawned = true;
        }
        else
        {
            spawnTime -= Time.deltaTime;
        }
    }
}
