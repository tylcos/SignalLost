using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

public class RoomSpawner : MonoBehaviour
{
    [Tooltip("Starting, Shop, Boss, Extra Rooms...")]
    public GameObject[] roomPrefabs;



    public Room startRoom = new Room(Vector2Int.zero, Room.RoomType.StartingRoom);
    public DictonaryGrid pathways = new DictonaryGrid();
        
    public int iterations = 4;
    public int roomsToSpawn = 11;
    public float teleporterCount = 3;
    public int scale = 40;



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
        var s = Stopwatch.StartNew();
        var ss = Stopwatch.StartNew();



        int correctedRoomsToSpawn = roomsToSpawn - 3; // Start, shop, and boss rooms

        Room.Initialize(iterations, roomsToSpawn);



        // Spawn the start room in a random direction 
        int startDirection = Random.Range(0, 8);
        Room.levels[0].Add(new Room(Room.Directions[startDirection], GetRandomRoom(-1)));
        pathways.SetPathway(startRoom.Position, (byte)startDirection);

        SpawnRoom(startRoom);
        SpawnRoom(Room.levels[0][0]);



        int maxIterations = iterations + 1; // Includes one for the initial room
        for (int currentIteration = 0; currentIteration < maxIterations; currentIteration++)
        {
            // Spawn rooms on last iteration rooms if no new rooms were spawned
            int spawnedChildCount = Room.levels[currentIteration].Count;
            if (spawnedChildCount == 0) 
            {
                --currentIteration;
                spawnedChildCount = Room.levels[currentIteration].Count;
            }



            // A value representing about how many rooms should be spawned per room 
            float ratio = (correctedRoomsToSpawn - spawnedRoomCount) / ((maxIterations - currentIteration) * spawnedChildCount);
            int nextIteration = currentIteration + 1;
            
            // Spawn new rooms on every room at the current iteration level
            foreach (Room currentRoom in Room.levels[currentIteration])
            {
                int numberOfRooms = GetRandom(ratio); // Number of rooms to spawn

                // Get a list of directions where a room will be spawned at
                var directionsToSpawn = currentRoom.GetAvailableSpawnDirectionsShuffled()
                    .Where(d => pathways.GetPathwayValid(currentRoom.Position, d)).Take(numberOfRooms);

                foreach (byte direction in directionsToSpawn)
                {
                    pathways.SetPathway(currentRoom.Position, direction);
                    Room newRoom = new Room(currentRoom.Position + Room.Directions[direction], GetRandomRoom(currentRoom.roomType));
                    Room.levels[nextIteration].Add(newRoom);

                    s.Stop();
                    SpawnRoom(newRoom);
                    s.Start();
                }
            }
        }



        // Spawning the boss room and shop room
        SpawnSpecialRoom(Room.RoomType.ShopRoom);
        SpawnSpecialRoom(Room.RoomType.BossRoom);



        s.Stop();
        ss.Stop();
        UnityEngine.Debug.Log("Finished spawning " + (spawnedRoomCount + 1) + " rooms in " + ss.Elapsed.Milliseconds + " ms total with " + s.Elapsed.Milliseconds + " ms of computing");
    }

    

    void SpawnRoom(Room room)
    {
        ++spawnedRoomCount;

        Vector3 position = new Vector3(room.Position.x * scale, room.Position.y * scale);
        Instantiate(roomPrefabs[room.roomType + 3], position, transform.rotation, transform);
    }

    void SpawnSpecialRoom(Room.RoomType roomType)
    {
        var iterationLevel = Room.levels[Room.MaxIterations - 1];

        // Find a vaild direction
        Room roomToBuildOffOf = null;
        List<byte> directionsToSpawn = null;
        do
        {
            roomToBuildOffOf = iterationLevel[Random.Range(0, iterationLevel.Count)];
        }
        while ((directionsToSpawn = roomToBuildOffOf.GetAvailableSpawnDirectionsShuffled()
            .Where(d => pathways.GetPathwayValid(roomToBuildOffOf.Position, d)).ToList()).Count == 0);



        Room specialRoom = new Room(roomToBuildOffOf.Position + Room.Directions[directionsToSpawn[0]], (sbyte)roomType);
        pathways.SetPathway(roomToBuildOffOf.Position, directionsToSpawn[0]);

        SpawnRoom(specialRoom);
    }





    int GetRandom(float baseNumber)
    {
        float decimalPart = Mathf.Round(baseNumber) - baseNumber;
        int numberToSpawn = 0;
        double absoluteDecimalPart = Mathf.Abs(decimalPart);

        if (absoluteDecimalPart < lowerThreshhold)
            numberToSpawn =  Mathf.FloorToInt(baseNumber) + (Random.value < decreaseRandomChance ? -1 : 0);
        else if (1 - absoluteDecimalPart < upperThreshhold)
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

        for (int i = 0; i < 4; i++)
            if ((direction & 1 << i) > 0)
                directions.Add(Room.Directions[i]);

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

    public static List<Room>[] levels;

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

        levels = new List<Room>[MaxIterations];

        levels[0] = new List<Room>(1);
        for (int i = 1; i < MaxIterations; i++)
            levels[i] = new List<Room>(averageMaxRooms);
    }



    public IEnumerable<byte> GetAvailableSpawnDirectionsShuffled()
    {
        foreach (byte i in RandomHelper.RandomRangeNoRepeat(8, 8))
            if (!takenPositions.Contains(Position + Directions[i]))
                yield return i;

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
