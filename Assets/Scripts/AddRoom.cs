using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    
    void Start()
    {
        Rooms rooms = GameObject.FindGameObjectWithTag("Rooms").GetComponent<Rooms>();
        rooms.spawnedRooms.Add(gameObject);
    }
}
