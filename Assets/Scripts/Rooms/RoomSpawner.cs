using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;



public class RoomSpawner : MonoBehaviour
{
    public readonly int maxConnections = 4;
    public bool debug;



    public GameObject StartingRoom;
    public GameObject[] BaseRooms;
    public GameObject[] ZeroRooms = new GameObject[4]; // Spawned when there is no space to spawn a large room
    public GameObject BossRooms;



    void Start()
    {
        Room.Initialize(maxConnections, transform);



        // Setting up all of the base rooms
        foreach (GameObject room in BaseRooms)
        {
            var rm = room.GetComponent<RoomManager>();
            rm.UpdateConnectors();

            int connectionCount = rm.connectors.Sum(s => s.Count);
            Room.BaseRooms[connectionCount].Add(new Room(room, rm));

            if (debug)
                Debug.Log("[Adding Rooms] Room " + room.name + " has "+ connectionCount + " connections");
        }

        // Setting up all of the special rooms
        for (int rotation = 0; rotation < 4; rotation++)
        {
            var zeroScript = ZeroRooms[rotation].GetComponent<RoomManager>();
            zeroScript.UpdateConnectors();

            Room.BaseRooms[0].Add(new Room(ZeroRooms[rotation], zeroScript));
        }

        if (debug)
            Debug.Log("[Total Rooms] " + Room.BaseRooms.Sum(s => s == null ? 0 : s.Count));



        var sw = System.Diagnostics.Stopwatch.StartNew();
        int spawnedRooms = InstantiateRooms();
        sw.Stop();

        Debug.Log("Finished spawning " + (spawnedRooms + 1) + " rooms in " + sw.Elapsed.Milliseconds + " ms total");
    }



    private int InstantiateRooms()
    {
        Room startRoom = new Room(Instantiate(StartingRoom, transform)) { index = 0 };
        Room.spawnedRooms.Add(new Bounds(startRoom.bounds.center, startRoom.bounds.size));


        int remainingRooms = LevelManager.RoomsToSpawn;

        List<Room> newRooms = new List<Room>(16); // Rooms to spawn on in the next iteration
        Queue<Room> queue = new Queue<Room>(16);  // Rooms to spawn on in the current iteration
        queue.Enqueue(startRoom);



        int loop = 0; 
        while (remainingRooms > 0) // Iteration start
        {
            int totalConnections = queue.Sum(r => r.connections.Sum(s => s.Count));

            while (queue.Any())
            {
                Room room = queue.Dequeue();

                foreach (var connection in room.GetConnections())
                {
                    int deltaRooms = remainingRooms - totalConnections;
                    int connections = deltaRooms > 0 ? 
                        UnityEngine.Random.Range(2, Math.Min(deltaRooms + 1, maxConnections) + 1) : 1;



                    var roomToSpawnTuple = room.GetRoom(connections, connection);

                    roomToSpawnTuple.Item1.Instantiate(roomToSpawnTuple.Item2);
                    newRooms.Add(roomToSpawnTuple.Item1);
                    totalConnections += roomToSpawnTuple.Item3;



                    --remainingRooms;
                }
            }

            queue = new Queue<Room>(newRooms);
            newRooms.Clear();

            if (loop++ > LevelManager.RoomsToSpawn) // Infinite loop check
                break;
        }

        return LevelManager.RoomsToSpawn - remainingRooms;
    }
}



public class Room
{
    public GameObject gameObject;
    public Transform transform;

    public List<int>[] connections;
    public Bounds bounds;
    public int index;



    public static List<Room>[] BaseRooms; // [Connections][Room Index]

    public static List<Bounds> spawnedRooms; // Used for checking room collisions



    private static Transform roomSpawner; // Room spawner GameObject

    private static readonly Vector3 roomPadding = new Vector3(2.5f, 2.5f);



    public Room()
    {
    }

    public Room(GameObject go)
    {
        gameObject = go;
        transform = go.transform;

        var rm = go.GetComponent<RoomManager>();
        connections = rm.connectors;
        bounds = rm.bounds;
    }

    public Room(GameObject go, RoomManager rm)
    {
        gameObject = go;
        transform = go.transform;

        connections = rm.connectors;
        bounds = rm.bounds;
    }

    /// <summary>
    /// Used to clone rooms so that the connections can be removed without changing the base rooms
    /// </summary>
    /// <returns>Cloned room</returns>
    public Room Clone()
    {
        Room room = new Room
        {
            connections = new List<int>[]
            {
                new List<int>(connections[0]),
                new List<int>(connections[1]),
                new List<int>(connections[2]),
                new List<int>(connections[3])
            },

            gameObject = gameObject,
            bounds = bounds
        };

        return room;
    }



    public IEnumerable<Connection> GetConnections()
    {
        foreach (int s in new[] { 0,2,1,3 })
        {
            var side = connections[s];
            for (int c = 0; c < side.Count; c++)
                yield return new Connection((byte)s, side[c]);
        }
    }

    /// <summary>
    /// Returns the offset of where the connection is located at
    /// </summary>
    public Vector3 GetConnectionOffset(Connection connection)
    {
        Vector3 offset = new Vector3(bounds.extents.x + bounds.min.x, bounds.extents.y + bounds.min.y);

        if ((connection.Direction & 0b10) > 0) // Horizonal connection
        {
            offset.x += (connection.Direction & 1) > 0 ? bounds.extents.x : -bounds.extents.x;
            offset.y += connection.Position;
        }
        else // Vertical connection
        {
            offset.x += connection.Position;
            offset.y += (connection.Direction & 1) > 0 ? bounds.extents.y : -bounds.extents.y;
        }

        return offset;
    }

    /// <summary>
    /// Returns the position of where to spawn the room in world space and removes the connection of the room to spawn
    /// </summary>
    public (Vector3, Connection) GetSpawnTuple(Connection parentConnection, Room roomToSpawn)
    {
        byte newDirection = parentConnection.FlippedDirection;

        int positionOfNewConnection = roomToSpawn.connections[newDirection].Shuffle().First();
        Connection newConnection = new Connection(newDirection, positionOfNewConnection);

        return (transform.position + GetConnectionOffset(parentConnection) - roomToSpawn.GetConnectionOffset(newConnection)
            , newConnection);
    }

    public bool ValidSpawnPosition(Vector3 position, int parentIndex)
    {
        Bounds spawnedBounds = new Bounds(bounds.center + position, bounds.size + roomPadding);

        for (int r = 0; r < spawnedRooms.Count; r++)
            if (r != parentIndex && spawnedRooms[r].Intersects(spawnedBounds))
                return false;
                
        return true;
    }

    /// <summary>
    /// Returns a valid room that can be spawned from the connection of the current room
    /// </summary>
    /// <param name="connections">The number of connections the returned room needs to have</param>
    /// <param name="connection">The connection the new room is being spawned from</param>
    /// <returns>(Room to spawn, Position to spawn the room, Connections the returned room has) </returns>
    public (Room, Vector3, int) GetRoom(int connections, Connection connection)
    {
        (Vector3, Connection) spawnTuple;

        foreach (Room room in BaseRooms[connections].Shuffle())
        {
            if (room.connections[connection.FlippedDirection].Count > 0)
            {
                spawnTuple = GetSpawnTuple(connection, room);

                if (room.ValidSpawnPosition(spawnTuple.Item1, index))
                {
                    Room newRoom = room.Clone();
                    newRoom.connections[spawnTuple.Item2.Direction].Remove(spawnTuple.Item2.Position);
                    return (newRoom, spawnTuple.Item1, connections - 1);
                }
            }
        }
            
        Room zeroRoom = BaseRooms[0].First(r => r.connections[connection.FlippedDirection].Count > 0).Clone();
        Vector3 newPosition = GetSpawnTuple(connection, zeroRoom).Item1;
        zeroRoom.connections[connection.FlippedDirection].Clear();
        return (zeroRoom, newPosition, 0);
    }



    public void Instantiate(Vector3 position)
    {
        gameObject = UnityEngine.Object.Instantiate(gameObject, position, Quaternion.identity, roomSpawner);
        transform = gameObject.transform;

        index = spawnedRooms.Count;
        spawnedRooms.Add(new Bounds(bounds.center + position, bounds.size));
    }



    public static void Initialize(int maxConnections, Transform parent)
    {
        maxConnections++;

        roomSpawner = parent;

        BaseRooms = new List<Room>[maxConnections];
        for (int i = 0; i < maxConnections; i++)
            BaseRooms[i] = new List<Room>(8);

        spawnedRooms = new List<Bounds>(16);
    }
}



public struct Connection
{
    public readonly byte Direction;
    public readonly int Position;



    public byte FlippedDirection { get { return (byte)(Direction ^ 1); } }



    public Connection(byte dir, int pos)
    {
        Direction = dir;
        Position = pos;
    }
}
