using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RoomsrScriptableObject", order = 1)]
public class RoomsSO : ScriptableObject
{
    public GameObject[] LeftRooms;
    public GameObject[] RightRooms;
    public GameObject[] TopRooms;
    public GameObject[] BottomRooms;
}
