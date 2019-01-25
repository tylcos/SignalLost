using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;
using System;

public class RoomSpawner : MonoBehaviour
{
    public int roomsToSpawn = 5;

    public GameObject[] roomPrefabs;
    [Tooltip("Starting, Shop, Boss")]
    public GameObject[] specialRooms = new GameObject[3];



    private void Start()
    {
        Room.Initialize(4, transform);

        foreach (GameObject room in roomPrefabs)
        {
            var script = room.GetComponent<RoomManager>();
            script.UpdateConnectors();
            int connections = script.connectors.Sum(s => s.Count);

            Debug.Log("[Adding Rooms] " + connections);
            for (int rotation = 0; rotation < 4; rotation++)
                Room.BaseRooms[connections].Add(new Room(room, script, rotation));
        }

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



        while (remainingRooms > 0)
        {
            while (queue.Any())
            {
                Room room = queue.Dequeue();

                foreach (var connection in room.GetConnections())
                {
                    int connections = new int[] { 1, 2, 4 }.Shuffle().First(); // TODO
                    Room roomToSpawn = Room.GetRoom(connections, connection.FlippedDirection).Clone();

                    Debug.Log("Chosen rot " + roomToSpawn.rotation);
                    var pos = room.GetSpawnPosition(connection, roomToSpawn);



                    roomToSpawn.Instantiate(pos);
                    unconsumedRooms.Add(roomToSpawn);
                    --remainingRooms;

                    if (remainingRooms == 0)
                        return;
                }
            }

            queue = new Queue<Room>(unconsumedRooms);
            unconsumedRooms.Clear();
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
        foreach (int s in new int[] { 0,2,1,3})
        {
            var side = connectors[s];
            for (int c = 0; c < side.Count; c++)
                yield return new Connection((byte)s, side[c]);
        }
    }

    public Vector3 GetConnectionOffset(Connection connection)
    {
        Vector3 offset = new Vector3();
        float multiplier = (connection.Direction & 1) > 0 ? 1 : -1;

        if ((connection.Direction & 2) > 0) // Horizonal connection
        {
            offset.x = (connection.Direction & 1) > 0 ? bounds.max.x : bounds.min.x;
            offset.y = connection.Position;
        }
        else
        {
            offset.x = connection.Position;
            offset.y = (connection.Direction & 1) > 0 ? bounds.max.y : bounds.min.y;
        }

        Debug.Log("-----");
        Debug.Log(rotation);
        Debug.Log(bounds.center.x + "   " + bounds.center.y + "   " + bounds.extents.x + "   " + bounds.extents.y);
        Debug.Log(bounds.min.x + "   " + bounds.min.y + "   " + bounds.max.x + "   " + bounds.max.y);
        Debug.Log(offset.x + "   " + offset.y);
        return offset;
    }



    public Vector3 GetSpawnPosition(Connection connection, Room roomToSpawn)
    {
        var flippedDirection = connection.Flip();

        Debug.Log(roomToSpawn.GetConnections().Count());
        var spawnConnector = roomToSpawn.GetConnections().First(c => c.Direction == flippedDirection.Direction);
        roomToSpawn.connectors[flippedDirection.Direction].Remove(connection.Position);

        Debug.Log(spawnConnector.Direction + "    " + spawnConnector.Position);

        return transform.position + GetConnectionOffset(connection) - roomToSpawn.GetConnectionOffset(spawnConnector);
    }



    public void Instantiate(Vector3 position)
    {
        var quaternion = new Quaternion
        {
            eulerAngles = new Vector3(0, 0, 90f * rotation)
        };

        gameObject = UnityEngine.Object.Instantiate(gameObject, position, quaternion, roomSpawner);
        transform = gameObject.transform;
    }



    public static void Initialize(int maxConnections, Transform parent)
    {
        roomSpawner = parent;

        BaseRooms = new List<Room>[++maxConnections];
        for (int i = 1; i < maxConnections; i++)
            BaseRooms[i] = new List<Room>(2);
    }

    public void Rotate()
    {
        bounds = new Bounds(new Vector3(0, 0), new Vector3(bounds.size.x, bounds.size.y));
        if (rotation == 0)
            return;



        List<int>[] newConnectors = null;

        switch (rotation)
        {
            case 1:
                bounds = new Bounds(new Vector3(0, 0), new Vector3(bounds.size.y, bounds.size.x));

                newConnectors = new List<int>[] {
                    new List<int>(connectors[3]),
                    new List<int>(connectors[2]),
                    new List<int>(connectors[0]),
                    new List<int>(connectors[1])
                };
                break;

            case 2:

                newConnectors = new List<int>[] {
                    new List<int>(connectors[1]),
                    new List<int>(connectors[0]),
                    new List<int>(connectors[3]),
                    new List<int>(connectors[2])
                };
                break;

            case 3:
                bounds = new Bounds(new Vector3(0, 0), new Vector3(bounds.size.y, bounds.size.x));

                newConnectors = new List<int>[] {
                    new List<int>(connectors[2]),
                    new List<int>(connectors[3]),
                    new List<int>(connectors[1]),
                    new List<int>(connectors[0])
                };
                break;
        }

        connectors = newConnectors;
    }



    public static Room GetRoom(int connections, byte direction)
    {
        foreach (Room room in RandomHelper.Shuffle(BaseRooms[connections]))
        {
            Debug.Log("Rot " + room.rotation);
            if (room.connectors[direction].Count > 0)
                return room;
        }

        throw new Exception("No room with " + connections + " connections and a connection at " + direction);
    }



    public static void Print(List<int>[] a, String b)
    {
        Debug.Log("[Connetors] -- " + b + " -- " + ((a[0].Count > 0) ? 1 : 0) + ((a[1].Count > 0) ? 1 : 0)
        + ((a[2].Count > 0) ? 1 : 0) + ((a[3].Count > 0) ? 1 : 0));
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
