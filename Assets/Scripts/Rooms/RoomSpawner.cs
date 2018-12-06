﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;



public class RoomSpawner : MonoBehaviour
{
    [Tooltip("Starting, Shop, Boss, Extra Rooms...")]
    public GameObject[] roomPrefabs;



    public Room startRoom = new Room(Vector2Int.zero, Room.RoomType.StartingRoom);
    public DictonaryGrid pathways = new DictonaryGrid();

    public const int iterations = 4;
    public const int roomsToSpawn = 11;
    public const float teleporterCount = 3;
    public const int scale = 40;



    private const int maxConnections = 3;

    private const float lowerThreshhold = .2f;
    private const float upperThreshhold = .3f;
    private const float decreaseRandomChance = .2f;
    private const float increaseRandomChance = .5f;

    private int spawnedRoomCount = -2; // Offset the inital two spawned rooms



    private void Start()
    {
        if (roomPrefabs.Length == 0)
            throw new System.ArgumentOutOfRangeException("roomPrefabs", "Must have at least two different room prefabs to choose from");

        CreateRoomTree();
    }

    private void OnDrawGizmos()
    {
        InstantiatePathways();
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DeleteAllRooms();
            spawnedRoomCount = -2;

            pathways = new DictonaryGrid();
            Room.takenPositions = new HashSet<Vector2Int>();
            startRoom = new Room(Vector2Int.zero, Room.RoomType.StartingRoom);

            CreateRoomTree();
        }
    }



    public void CreateRoomTree()
    {
        int correctedRoomsToSpawn = roomsToSpawn - 3; // Start, shop, and boss rooms



        Room.Initialize(iterations, roomsToSpawn);

        int startDirection = Random.Range(0, 8);
        Room.rooms[0].Add(new Room(Room.GetDirectionVector(startDirection), GetRandomRoom(-1)));
        pathways.SetPathway(startRoom.Position, (byte)startDirection);

        SpawnRoom(startRoom);
        SpawnRoom(Room.rooms[0][0]);



        int maxIterations = iterations + 1; // Includes one for the initial room
        for (int currentIteration = 0; currentIteration < maxIterations; currentIteration++)
        {
            int spawnedChildCount = Room.rooms[currentIteration].Count;
            if (spawnedChildCount == 0) // Spawn rooms on last iteration rooms if no new rooms were spawned
            {
                --currentIteration;
                spawnedChildCount = Room.rooms[currentIteration].Count;
            }



            float ratio = (correctedRoomsToSpawn - spawnedRoomCount) / ((maxIterations - currentIteration) * spawnedChildCount);
            int nextIteration = currentIteration + 1;

            foreach (Room currentRoom in Room.rooms[currentIteration])
            {
                int numberOfRooms = GetRandom(ratio);
                List<byte> possibleDirections = currentRoom.GetAvailableSpawnDirections();

                for (int roomNumber = 0; roomNumber < numberOfRooms; roomNumber++)
                {
                    foreach (byte direction in RandomHelper.ShuffleList(possibleDirections))
                    {
                        if (pathways.GetPathwayValid(currentRoom.Position, direction))    // Valid position found
                        {
                            possibleDirections.Remove(direction);

                            pathways.SetPathway(currentRoom.Position, direction);
                            Room newRoom = new Room(currentRoom.Position + Room.GetDirectionVector(direction), GetRandomRoom(currentRoom.roomType));
                            Room.rooms[nextIteration].Add(newRoom);

                            SpawnRoom(newRoom);
                            break; // Exit once a valid direction is found
                        }
                    }
                }
            }
        }



        // Spawning the boss room and shop room
        SpawnSpecialRoom(Room.RoomType.ShopRoom);
        SpawnSpecialRoom(Room.RoomType.BossRoom);



        Debug.Log("Finished spawning " + (spawnedRoomCount + 1) + " rooms");
    }

    

    void SpawnRoom(Room room)
    {
        ++spawnedRoomCount;

        Vector3 position = new Vector3(room.Position.x * scale, room.Position.y * scale);
        Instantiate(roomPrefabs[room.roomType + 3], position, transform.rotation, transform);
    }

    void SpawnSpecialRoom(Room.RoomType roomType)
    {
        var iterationLevel = Room.rooms[Room.MaxIterations - 1];
        Room roomToBuildOffOf = iterationLevel[Random.Range(0, iterationLevel.Count)];

        foreach (byte direction in RandomHelper.ShuffleList(roomToBuildOffOf.GetAvailableSpawnDirections()))
        {
            if (pathways.GetPathwayValid(roomToBuildOffOf.Position, direction))
            {
                Room specialRoom = new Room(roomToBuildOffOf.Position + Room.GetDirectionVector(direction), roomType);
                pathways.SetPathway(roomToBuildOffOf.Position, direction);

                SpawnRoom(specialRoom);
                break;
            }
        }
    }

    void SpawnTeleporters()
    {
        var iterationLevel = Room.rooms[Room.MaxIterations - 1];
        iterationLevel = RandomHelper.ShuffleList(iterationLevel).ToList();

        for (int i = 0; i < teleporterCount; i++)
        {
            var rooms = iterationLevel.GetRange(i * 2, 2);
            var directions = rooms.Select(r => r.GetAvailableSpawnDirections());

            if (directions.All(directionList => directionList.Count > 0))
            {

            }
        }
    }



    int GetRandom(float baseNumber)
    {
        float decimalPart = Mathf.Round(baseNumber) - baseNumber;
        int numberToSpawn = 0;

        if (Mathf.Abs(decimalPart) < lowerThreshhold)
            numberToSpawn =  Mathf.FloorToInt(baseNumber) + (Random.value < decreaseRandomChance ? -1 : 0);
        else if (1 - Mathf.Abs(decimalPart) < upperThreshhold)
            numberToSpawn =  Mathf.FloorToInt(baseNumber) + (Random.value < increaseRandomChance ? 1 : 0);
        else
            numberToSpawn = Mathf.FloorToInt(baseNumber) + (Random.value < decimalPart ? 1 : 0);

        return numberToSpawn > maxConnections ? maxConnections : numberToSpawn;
    }

    sbyte GetRandomRoom(sbyte parentRoomType)
    {
        int randomRoom;

        while ((randomRoom = Random.Range(0, roomPrefabs.Length - 3)) == parentRoomType);

        return (sbyte)randomRoom;
    }



    void InstantiatePathways()
    {
        Gizmos.color = Color.green;

        foreach (KeyValuePair<Vector2Int, byte> entry in pathways.grid)
        {
            foreach (Vector2 direction in GetDirectionVectors(entry.Value))
            {
                if (direction == Room.Directions[3])
                    Gizmos.DrawLine((Vector2)(entry.Key + Vector2Int.right) * scale, ((entry.Key + Vector2Int.right) + direction) * scale);
                else
                    Gizmos.DrawLine((Vector2)entry.Key * scale, (entry.Key + direction) * scale);
            }
        }
    }

    void DeleteAllRooms()
    {
        foreach (Transform room in transform)
            Destroy(room.gameObject);
    }



    List<Vector2> GetDirectionVectors(byte direction)
    {
        List<Vector2> directions = new List<Vector2>(4);

        if ((direction & 1) > 0)
            directions.Add(Room.GetDirectionVector(0));
        if ((direction & 2) > 0)
            directions.Add(Room.GetDirectionVector(1));
        if ((direction & 4) > 0)
            directions.Add(Room.GetDirectionVector(2));
        if ((direction & 8) > 0)
            directions.Add(Room.GetDirectionVector(3));

        return directions;
    }
}



public class Room
{
    public Vector2Int Position;
    public sbyte roomType;



    public static int MaxIterations { get; private set; }

    public static HashSet<Vector2Int> takenPositions = new HashSet<Vector2Int>();

    public static readonly Vector2Int[] Directions =
        { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 1), new Vector2Int(-1, 1),
            new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1) };

    public static List<Room>[] rooms;

    public enum RoomType : sbyte
    {
        StartingRoom = -3,
        ShopRoom = -2,
        BossRoom = -1
    }



    public Room(Vector2Int position, sbyte roomType)
    {
        Position = position;
        takenPositions.Add(position);

        this.roomType = roomType;
    }

    public Room(Vector2Int position, RoomType roomType)
    {
        Position = position;
        takenPositions.Add(position);

        this.roomType = (sbyte)roomType;
    }



    public static void Initialize(int iterations, int roomsToSpawn)
    {
        MaxIterations = iterations + 2; // One for the initial room and last spawned rooms
        int averageMaxRooms = (roomsToSpawn / iterations) * 2;

        rooms = new List<Room>[MaxIterations];

        rooms[0] = new List<Room>(1);
        for (int i = 1; i < MaxIterations; i++)
            rooms[i] = new List<Room>(averageMaxRooms);
    }



    public List<byte> GetAvailableSpawnDirections()
    {
        List<byte> validDirection = new List<byte>(8);

        for (byte i = 0; i < 8; i++)
            if (!takenPositions.Contains(Position + Directions[i]))
                validDirection.Add(i);

        return validDirection;
    }



    public static Vector2Int GetDirectionVector(int value)
    {
        return Directions[value];
    }
}



public class DictonaryGrid
{
    public Dictionary<Vector2Int, byte> grid = new Dictionary<Vector2Int, byte>(32);



    public byte GetPosition(Vector2Int pos)
    {
        byte outValue = 0;

        grid.TryGetValue(pos, out outValue);

        return outValue;
    }



    public bool GetPathwayValid(Vector2Int position, byte direction)
    {
        CorrectPosition(ref position, ref direction);
        byte value = GetPosition(position);

        return (direction % 2 == 0) ? (value & (1 << direction)) == 0 : (value & 0xA) == 0; 
    }



    public void SetPathway(Vector2Int position, byte direction)
    {
        CorrectPosition(ref position, ref direction);

        if (grid.ContainsKey(position))
            grid[position] |= (byte)(1 << direction);
        else
            grid.Add(position, (byte)(1 << direction));
    }




    private static void CorrectPosition(ref Vector2Int position, ref byte direction)
    {
        switch (direction)
        {
            case 3:
            case 4:
                position += Vector2Int.left;
                break;
            case 5:
                position += new Vector2Int(-1, -1);
                break;
            case 6:
            case 7:
                position += Vector2Int.down;
                break;
        }

        direction %= 4;
    }
}
