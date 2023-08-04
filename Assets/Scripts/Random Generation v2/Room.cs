using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Serializable]
    public enum VariationRoomName
    {
        LeftRight,
        LeftDownRight,
        LeftUpRight,
        LeftUpRightDown
    }
    [SerializeField]
    public VariationRoomName type;
    public void RoomDestruct()
    {
        Destroy(gameObject);
    }
}
