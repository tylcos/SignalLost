using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;



public class RoomSpawner : MonoBehaviour
{
    public readonly int maxConnections = 4;
    public bool debug;



    public GameObject[] BaseRooms;
    public GameObject StartingRoom;
    public GameObject[] ZeroRooms = new GameObject[4]; // Spawned when there is no space to spawn a large room
    public GameObject[] ShopRooms = new GameObject[4];
    public GameObject[] BossRooms = new GameObject[4];

    public Transform SpawnTransform = GameObject.Find("/Scene/Rooms").transform;
    


    public void SpawnRooms(int roomsToSpawn)
    {
        Assert.IsNotNull(SpawnTransform, "No game object '/Scene/Rooms' found");



        Room.Initialize(maxConnections, transform);

        foreach (GameObject room in BaseRooms)
        {
            var rm = room.GetComponent<RoomManager>();
            rm.UpdateConnectors();

            int connectionCount = rm.connectors.Sum(s => s.Count);
            Room.BaseRooms[connectionCount].Add(new Room(room, rm));

            if (debug)
                Debug.Log("[Adding Rooms] " + connectionCount);
        }

        for (int rotation = 0; rotation < 4; rotation++)
        {
            var zeroScript = ZeroRooms[rotation].GetComponent<RoomManager>();
            zeroScript.UpdateConnectors();

            Room.BaseRooms[0].Add(new Room(ZeroRooms[rotation], zeroScript));
        }



        if (debug)
            Debug.Log("[Total Rooms] " + Room.BaseRooms.Sum(s => s == null ? 0 : s.Count));



        var sw = System.Diagnostics.Stopwatch.StartNew();
        InstantiateRooms(roomsToSpawn);
        sw.Stop();

        Debug.Log("Finished spawning " + (roomsToSpawn + 1) + " rooms in " + sw.Elapsed.Milliseconds + " ms total");
    }



    private void InstantiateRooms(int roomsToSpawn)
    {
        Room startRoom = new Room(Instantiate(StartingRoom, transform));
        int remainingRooms = roomsToSpawn;

        List<Room> newRooms = new List<Room>(16); // Rooms to spawn on in the next iteration
        Queue<Room> queue = new Queue<Room>(16);  // Rooms to spawn on in the current iteration
        queue.Enqueue(startRoom);



        int loop = 0; 
        while (remainingRooms > 0) // Iteration
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



                    var roomToSpawnTuple = Room.GetRoom(connections, connection, room);

                    roomToSpawnTuple.Item1.Instantiate(roomToSpawnTuple.Item2);
                    newRooms.Add(roomToSpawnTuple.Item1);
                    totalConnections += roomToSpawnTuple.Item3 - 1;



                    --remainingRooms;
                    if (remainingRooms == 0)
                        return;
                }
            }

            queue = new Queue<Room>(newRooms);
            newRooms.Clear();

            if (loop++ > 50) // Infinite loop check
                break;
        }
    }
}



public class Room
{
    public GameObject gameObject;
    public Transform transform;

    public List<int>[] connections;
    public Bounds bounds;



    public static List<Room>[] BaseRooms; // [Connections][Room Index]

    public static List<Bounds> spawnedRooms;



    private static Transform roomSpawner;



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

        if ((connection.Direction & 2) > 0) // Horizonal connection
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
    public Vector3 GetSpawnPosition(Connection connection, Room roomToSpawn)
    {
        byte flippedDirection = connection.FlippedDirection;
        var oppositeConnection = new Connection(flippedDirection, roomToSpawn.connections[flippedDirection].Shuffle().First());
        roomToSpawn.connections[flippedDirection].Remove(connection.Position);

        return transform.position + GetConnectionOffset(connection) - roomToSpawn.GetConnectionOffset(oppositeConnection);
    }

    public bool ValidSpawnPosition(Vector3 position)
    {/*
        var spawnedBounds = new Bounds(bounds.center + position, bounds.size);

        for (int r = 0; r < spawnedRooms.Count; r++)
            if (spawnedRooms[r].Intersects(spawnedBounds))
                return false;
                */
        return true;
    }

    /// <summary>
    /// Returns a valid room that can be spawned from the connection of the current room
    /// </summary>
    /// <param name="connections">The number of connections the returned room needs to have</param>
    /// <param name="connection">The connection the new room is being spawned from</param>
    /// <param name="currentRoom">The current room that is spawning the new room</param>
    /// <returns>(Room to spawn, Position to spawn the room, Connections the returned room has) </returns>
    public static (Room, Vector3, int) GetRoom(int connections, Connection connection, Room currentRoom)
    {
        foreach (Room room in RandomHelper.Shuffle(BaseRooms[connections]))
        {
            if (room.connections[connection.FlippedDirection].Count > 0)/* &&
                room.ValidSpawnPosition((spawnPosition = currentRoom.GetSpawnPosition(connection, room))))*/
                return (room.Clone(), currentRoom.GetSpawnPosition(connection, room), connections);
        }
            
        Debug.Log("No room found with " + connections + " connections!");
        Room zeroRoom = BaseRooms[0].First(r => r.connections[connection.FlippedDirection].Count > 0);
        return (zeroRoom, currentRoom.GetSpawnPosition(connection, zeroRoom), 1);
    }



    public void Instantiate(Vector3 position)
    {
        gameObject = UnityEngine.Object.Instantiate(gameObject, position, Quaternion.identity, roomSpawner);

        var spawnedBounds = new Bounds(bounds.center + position, bounds.size);
        spawnedRooms.Add(spawnedBounds);
    }



    public static void Initialize(int maxConnections, Transform parent)
    {
        roomSpawner = parent;

        BaseRooms = new List<Room>[++maxConnections];
        for (int i = 0; i < maxConnections; i++)
            BaseRooms[i] = new List<Room>(8);

        spawnedRooms = new List<Bounds>(maxConnections);
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
