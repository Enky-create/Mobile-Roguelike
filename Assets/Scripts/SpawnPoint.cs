using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private OpeningDirection openingDirection;
    private Rooms roomsTemplates;
    [SerializeField] private bool isSpawned=false;
    [Serializable]enum OpeningDirection
    {
        Top,
        Bottom,
        Left,
        Right,
        NoOpening,
    }
    [SerializeField]
    private float waitTime = 5f;
    void Start()
    {
        Destroy(gameObject, waitTime);
        roomsTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<Rooms>();
        Invoke("SpawnRoom", 0.1f);
    }

    // Update is called once per frame
    void SpawnRoom()
    {
        int rand;
        if (!isSpawned)
        {
            switch (openingDirection)
            {
                default:
                case OpeningDirection.Top:
                    rand = Random.Range(0, roomsTemplates.TopRooms.Length);
                    Instantiate(roomsTemplates.TopRooms[rand], transform.position, Quaternion.identity);
                    break;
                case OpeningDirection.Bottom:
                    rand = Random.Range(0, roomsTemplates.BottomRooms.Length);
                    Instantiate(roomsTemplates.BottomRooms[rand], transform.position, Quaternion.identity);
                    break;
                case OpeningDirection.Left:
                    rand = Random.Range(0, roomsTemplates.LeftRooms.Length);
                    Instantiate(roomsTemplates.LeftRooms[rand], transform.position, Quaternion.identity);
                    break;
                case OpeningDirection.Right:
                    rand = Random.Range(0, roomsTemplates.RightRooms.Length);
                    Instantiate(roomsTemplates.RightRooms[rand], transform.position, Quaternion.identity);
                    break;
            }
            isSpawned = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("RoomSpawner"))
        {
            if (collision.GetComponent<SpawnPoint>().isSpawned == false && isSpawned==false)
            {
                Instantiate(roomsTemplates.ClosedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            isSpawned = true;
        }
    }
}
