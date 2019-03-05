using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;



public class RoomSpawner : MonoBehaviour
{
    public int roomsToSpawn = 5;
    public int maxConnections = 4;
    public bool debug;

    public GameObject[] roomPrefabs;
    [Tooltip("Starting, zero, Shop, Boss")]
    public GameObject[] specialRooms = new GameObject[4];



    // Eventually change to spawn a specific number of rooms ~~~
    private void Start()
    {
        Room.Initialize(maxConnections, transform);

        foreach (GameObject room in roomPrefabs)
        {
            var script = room.GetComponent<RoomManager>();
            script.UpdateConnectors();
            int connections = script.connectors.Sum(s => s.Count);

            if (debug)
                Debug.Log("[Adding Rooms] " + connections);
            for (int rotation = 0; rotation < 4; rotation++)
                Room.BaseRooms[connections].Add(new Room(room, script, rotation));
        }

        var zeroScript = specialRooms[1].GetComponent<RoomManager>();
        zeroScript.UpdateConnectors();
        for (int rotation = 0; rotation < 4; rotation++)
            Room.BaseRooms[0].Add(new Room(specialRooms[1], zeroScript, rotation));



        if (debug)
            Debug.Log("[Total Rooms] " + Room.BaseRooms.Sum(s => s == null ? 0 : s.Count));



        var c = System.Diagnostics.Stopwatch.StartNew();
        var ss = System.Diagnostics.Stopwatch.StartNew();

        SpawnRooms();

        c.Stop();
        ss.Stop();
        Debug.Log("Finished spawning " + (roomsToSpawn + 1) + " rooms in " + ss.Elapsed.Milliseconds + " ms total with " + c.Elapsed.Milliseconds + " ms of computing");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
        }
    }



    public void SpawnRooms()
    {
        Room startRoom = new Room(Instantiate(specialRooms[0], transform));
        int remainingRooms = roomsToSpawn;

        List<Room> unconsumedRooms = new List<Room>(8);
        Queue<Room> queue = new Queue<Room>(8);
        queue.Enqueue(startRoom);



        int loop = 0;
        while (remainingRooms > 0) // Iteration
        {
            int totalConnections = queue.Sum(r => r.connectors.Sum(s => s.Count));

            while (queue.Any())
            {
                Room room = queue.Dequeue();

                foreach (var connection in room.GetConnections())
                {
                    int deltaRooms = remainingRooms - totalConnections;
                    int connections = deltaRooms > 0 ? 
                        UnityEngine.Random.Range(2, Math.Min(deltaRooms + 1, maxConnections) + 1) : 1;



                    var roomToSpawnTuple = Room.GetRoom(connections, connection, room);
                    var roomToSpawn = roomToSpawnTuple.Item1.Clone();


                    roomToSpawn.Instantiate(roomToSpawnTuple.Item2);
                    unconsumedRooms.Add(roomToSpawn);
                    totalConnections += roomToSpawnTuple.Item3 - 1;

                    --remainingRooms;
                    if (remainingRooms == 0)
                        return;
                }
            }

            queue = new Queue<Room>(unconsumedRooms);
            unconsumedRooms.Clear();

            if (loop++ > 50)
                break;
        }
    }
}



public class Room
{
    public GameObject gameObject;
    public Transform transform;

    public List<int>[] connectors;
    public Bounds bounds;
    public byte rotation;



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
        connectors = rm.connectors;
        bounds = rm.bounds;
    }

    public Room(GameObject go, RoomManager rm)
    {
        gameObject = go;
        transform = go.transform;

        connectors = rm.connectors;
        bounds = rm.bounds;
    }

    public Room(GameObject go, RoomManager rm, int rotation)
    {
        gameObject = go;
        transform = go.transform;

        connectors = rm.connectors;
        bounds = rm.bounds;

        this.rotation = (byte)rotation;
        Rotate();
    }

    public Room Clone()
    {
        Room room = new Room
        {
            connectors = new List<int>[]
            {
                new List<int>(connectors[0]),
                new List<int>(connectors[1]),
                new List<int>(connectors[2]),
                new List<int>(connectors[3])
            },

            gameObject = gameObject,
            bounds = bounds,
            rotation = rotation
        };

        return room;
    }



    public IEnumerable<Connection> GetConnections()
    {
        foreach (int s in new [] { 0,2,1,3})
        {
            var side = connectors[s];
            for (int c = 0; c < side.Count; c++)
                yield return new Connection((byte)s, side[c]);
        }
    }

    public Vector3 GetConnectionOffset(Connection connection)
    {
        Vector3 offset = new Vector3(bounds.extents.x + bounds.min.x, bounds.extents.y + bounds.min.y);

        if ((connection.Direction & 2) > 0) // Horizonal connection
        {
            offset.x += (connection.Direction & 1) > 0 ? bounds.extents.x : -bounds.extents.x;
            offset.y += connection.Position;
        }
        else
        {
            offset.x += connection.Position;
            offset.y += (connection.Direction & 1) > 0 ? bounds.extents.y : -bounds.extents.y;
        }

        if (roomSpawner.GetComponent<RoomSpawner>().debug)
        {
            Debug.Log("-----[Connection Offset]-----");
            Debug.Log(rotation);
            Debug.Log(bounds.center.x + "   " + bounds.center.y + "   " + bounds.extents.x + "   " + bounds.extents.y);
            Debug.Log(bounds.min.x + "   " + bounds.min.y + "   " + bounds.max.x + "   " + bounds.max.y);
            Debug.Log(offset.x + "   " + offset.y);
        }
        
        return offset;
    }



    public Vector3 GetSpawnPosition(Connection connection, Room roomToSpawn)
    {
        var flippedDirection = connection.FlippedDirection;

        var c = roomToSpawn.connectors[connection.FlippedDirection];
        var spawnConnector = new Connection(flippedDirection, roomToSpawn.connectors[flippedDirection].Shuffle().First());
        roomToSpawn.connectors[flippedDirection].Remove(connection.Position);

        return transform.position + GetConnectionOffset(connection) - roomToSpawn.GetConnectionOffset(spawnConnector);
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

    public static (Room, Vector3, int) GetRoom(int connections, Connection connection, Room currentRoom)
    {
        foreach (Room room in RandomHelper.Shuffle(BaseRooms[connections]))
        {
            if (room.connectors[connection.FlippedDirection].Count > 0)/* &&
                room.ValidSpawnPosition((spawnPosition = currentRoom.GetSpawnPosition(connection, room))))*/
                return (room, currentRoom.GetSpawnPosition(connection, room), connections);
        }
            
        Debug.Log("bad" + connections);
        Room capRoom = BaseRooms[0].First(r => r.connectors[connection.FlippedDirection].Count > 0);
        return (capRoom, currentRoom.GetSpawnPosition(connection, capRoom), 1);
    }



    public void Instantiate(Vector3 position)
    {
        var quaternion = new Quaternion
        {
            eulerAngles = new Vector3(0, 0, 90f * rotation)
        };

        gameObject = UnityEngine.Object.Instantiate(gameObject, position, quaternion, roomSpawner);
        transform = gameObject.transform;

        var spawnedBounds = new Bounds(bounds.center + position, bounds.size);
        spawnedRooms.Add(spawnedBounds);
    }



    public static void Initialize(int maxConnections, Transform parent)
    {
        roomSpawner = parent;

        BaseRooms = new List<Room>[++maxConnections];
        for (int i = 0; i < maxConnections; i++)
            BaseRooms[i] = new List<Room>(2);

        spawnedRooms = new List<Bounds>(maxConnections);
    }

    public void Rotate()
    {
        if (rotation == 0)
            return;
        


        List<int>[] newConnectors = null;
        var newBounds = new Bounds();

        switch (rotation)
        {
            case 1:
                newBounds.SetMinMax(new Vector3(bounds.min.y, -bounds.max.x), new Vector3(bounds.max.y, -bounds.min.x));
                newBounds.center = new Vector3(bounds.center.y, -bounds.center.x);

                newConnectors = new List<int>[] {
                    new List<int>(connectors[3]),
                    new List<int>(connectors[2]),
                    new List<int>(connectors[0]),
                    new List<int>(connectors[1])
                };
                break;

            case 2:
                newBounds.SetMinMax(-bounds.max, -bounds.min);
                newBounds.center = new Vector3(-bounds.center.x, -bounds.center.y);

                newConnectors = new List<int>[] {
                    new List<int>(connectors[1]),
                    new List<int>(connectors[0]),
                    new List<int>(connectors[3]),
                    new List<int>(connectors[2])
                };
                break;

            case 3:
                newBounds.SetMinMax(new Vector3(-bounds.max.y, bounds.min.x), new Vector3(-bounds.min.y, -bounds.max.x));
                newBounds.center = new Vector3(bounds.center.y, -bounds.center.x);

                newConnectors = new List<int>[] {
                    new List<int>(connectors[2]),
                    new List<int>(connectors[3]),
                    new List<int>(connectors[1]),
                    new List<int>(connectors[0])
                };
                break;
        }

        bounds = newBounds;
        connectors = newConnectors;
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



    public Connection Flip()
    {
        return new Connection(FlippedDirection, Position);
    }
}
