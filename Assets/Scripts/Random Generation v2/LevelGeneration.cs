using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]
    private int maxRoomCount;
    public int currentRoomCount=0;
    public static LevelGeneration Instance
    {
        get;
        private set;
    }
    public bool isPathCreated = false;
    public event EventHandler OnAllPathCreated;
    public event EventHandler OnAllRoomsCreated;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Player >1");
        }
        Instance = this;
    }
    [Serializable]
    public enum VariationRoomName
    {
        LeftRight,
        LeftDownRight,
        LeftUpRight,
        LeftUpRightDown
    }
    [Serializable]
    public struct VariationOfRooms
    {
        public VariationRoomName name;
        public GameObject[] variations;
    }

        // 0 - LR
        // 1 - LDR
        // 2 - LUR
        // 3 - LDRU

    [SerializeField]
    public List<VariationOfRooms> rooms;
    [SerializeField]
    private Transform [] startPositions;
    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float minY;
    private bool stopGeneration = false;
    [SerializeField]
    private float startroomTime;
    private float spawnroomTime;
    // from 1 to 5
    //1,2 - right direction
    //3,4 - left direction
    //5   - down direction
    private int direction; 
    [SerializeField]
    private float moveAmount;
    [SerializeField]
    private LayerMask roomLayer;
    private int downDirectionCount;
    private Room currentRoom;
    //For pathfinding check
    private bool areAllRoomsCreated = false;
    void Start()
    {
        downDirectionCount = 0;
        int randPosition = Random.Range(0, startPositions.Length);
        transform.position = startPositions[randPosition].position;
        int randRoom = Random.Range(0, rooms.Count);
        int randVar = Random.Range(0, rooms[0].variations.Length);
        currentRoom= Instantiate(rooms[randRoom].variations[randVar], transform.position, Quaternion.identity).GetComponent<Room>();
        direction = Random.Range(1, 6);
        spawnroomTime = startroomTime;
        currentRoomCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (/*spawnroomTime < 0 && */!stopGeneration)
        {
            SpawnRoom();
            spawnroomTime = startroomTime;
        }
        if (currentRoomCount == maxRoomCount && !areAllRoomsCreated)
        {
            areAllRoomsCreated = true;
            OnAllRoomsCreated?.Invoke(this, EventArgs.Empty);
        }
        //else
        //{
        //    spawnroomTime -= Time.deltaTime;
        //}
    }
    private void SpawnRoom()
    {
        Debug.Log(direction);//DEBUG!!!!!!!!!!!!!!!!!
        if(direction ==1 || direction == 2) // Right direction of spawning a room
        {
            
            if (transform.position.x < maxX)
            {
                downDirectionCount = 0;
                transform.position = new Vector2(transform.position.x + moveAmount, transform.position.y);
                int randRoom = Random.Range(0, rooms.Count);
                int randVar = Random.Range(0, rooms[0].variations.Length);
                

                currentRoom = Instantiate(rooms[randRoom].variations[randVar], transform.position, Quaternion.identity).GetComponent<Room>();
                direction = Random.Range(1, 6);
                if (direction==3)
                {
                    direction = 1;
                }
                if (direction == 4)
                {
                    direction = 5;
                }
                currentRoomCount++;
            }
            else
            {
                direction = 5;
            }
            
        }
        else if(direction == 3 || direction == 4)
        {
            
            if (transform.position.x > minX)
            {
               
                downDirectionCount = 0;
                transform.position = new Vector2(transform.position.x - moveAmount, transform.position.y);
                int randRoom = Random.Range(0, rooms.Count);
                int randVar = Random.Range(0, rooms[0].variations.Length);
                
                currentRoom = Instantiate(rooms[randRoom].variations[randVar], transform.position, Quaternion.identity).GetComponent<Room>();
                currentRoomCount++;
                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 5)
        {
            
            if (transform.position.y>minY)
            {
                downDirectionCount++;
                //Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, roomLayer);
                //if(roomDetection.TryGetComponent(out Room room))
                //{
                //    if (room.type != VariationRoomName.LeftDownRight && room.type != VariationRoomName.LeftUpRightDown)
                //    {
                //        if (downDirectionCount > 1)
                //        {
                //            room.RoomDestruct();
                //            Instantiate(rooms[3].variations[0], transform.position, Quaternion.identity);
                //        }
                //        else
                //        {
                //            room.RoomDestruct();
                //            int randroom = Random.Range(1, 4);
                //            if (randroom == 2)
                //            {
                //                randroom = 1;
                //            }
                //            int randvar = Random.Range(0, rooms[randroom].variations.Length);
                //            Instantiate(rooms[randroom].variations[randvar], transform.position, Quaternion.identity);
                //        }

                //    }
                //}
                
                if (currentRoom.type != VariationRoomName.LeftDownRight && currentRoom.type != VariationRoomName.LeftUpRightDown)
                {
                    if (downDirectionCount > 1)
                    {
                        currentRoom.RoomDestruct();
                        currentRoom=Instantiate(rooms[3].variations[0], transform.position, Quaternion.identity).GetComponent<Room>();
                    }
                    else
                    {
                        currentRoom.RoomDestruct();
                        int randroom = Random.Range(1, 4);
                        if (randroom == 2)
                        {
                            randroom = 1;
                        }
                        int randvar = Random.Range(0, rooms[randroom].variations.Length);
                        currentRoom=Instantiate(rooms[randroom].variations[randvar], transform.position, Quaternion.identity).GetComponent<Room>();
                    }

                }
                

                transform.position = new Vector2(transform.position.x, transform.position.y - moveAmount);
                int randRoom = Random.Range(2, rooms.Count);
                int randVar = Random.Range(0, rooms[randRoom].variations.Length);
                currentRoom = Instantiate(rooms[randRoom].variations[randVar], transform.position, Quaternion.identity).GetComponent<Room>();
                currentRoomCount++;
                direction = Random.Range(1, 6);
            }
            else
            {
                stopGeneration = true;
                isPathCreated = true;
                OnAllPathCreated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
