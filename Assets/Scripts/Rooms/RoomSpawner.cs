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
            int connections = script.connectors.Count(s => s.Any());

            for (int rotation = 0; rotation < 4; rotation++)
                Room.BaseRooms[connections].Add(new Room(room, script, rotation));
        }

        SpawnRooms();
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
                    int connections = new int[] { 1, 2, 4 }[UnityEngine.Random.Range(0, 3)]; // TODO
                    Room roomToSpawn = Room.GetRoom(connections, connection.Direction).Clone();

                    var pos = room.GetSpawnPosition(connection, roomToSpawn);



                    roomToSpawn.Instantiate(pos);
                    unconsumedRooms.Add(roomToSpawn);
                    --remainingRooms;
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
    public byte rotation = 0;



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

    public Room(GameObject go, RoomManager rm, int rotation) : this(go, rm)
    {
        Rotate(rotation);
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
            bounds = bounds
        };

        return room;
    }



    public IEnumerable<Connection> GetConnections()
    {
        for (int s = 0; s < 4; s++)
        {
            var side = connectors[s];
            for (int c = 0; c < side.Count; c++)
                yield return new Connection((byte)s, side[c]);
        }
    }

    public Vector3 GetConnectionOffset(Connection connection)
    {
        float xOffset;
        float yOffset;

        if ((connection.Direction & 2) > 0)
        {
            xOffset = (connection.Direction & 1) > 0 ? bounds.max.x : bounds.min.x;
            yOffset = bounds.min.y + connection.Position;
        }
        else
        {
            xOffset = bounds.min.x + connection.Position;
            yOffset = (connection.Direction & 1) > 0 ? bounds.max.y : bounds.min.y;
        }

        return new Vector3(xOffset, yOffset);
    }



    public Vector3 GetSpawnPosition(Connection connection, Room roomToSpawn)
    {
        var flippedDirection = connection.Flip();

        var spawnConnector = roomToSpawn.GetConnections().First(c => c.Direction == flippedDirection.Direction);
        roomToSpawn.connectors[flippedDirection.Direction].Remove(connection.Position);

        return transform.position + GetConnectionOffset(connection) - roomToSpawn.GetConnectionOffset(spawnConnector);
    }



    public void Instantiate(Vector3 position)
    {
        var quaternion = new Quaternion();
        quaternion.eulerAngles = new Vector3(0, 0, 90f * rotation);
        Debug.Log(rotation);

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

    public void Rotate(int rotation)
    {
        Debug.Log("ROtate");
        if (rotation == 0)
            return;

        List<int>[] newConnectors;
        this.rotation = (byte)rotation;

        var newBounds = new Bounds();
        switch (rotation)
        {
            case 1:
                newBounds.SetMinMax(new Vector3(bounds.min.y, -bounds.max.x), new Vector3(bounds.max.y, -bounds.min.x));
                bounds = newBounds;

                newConnectors = new List<int>[] {
                    new List<int>(connectors[3]),
                    new List<int>(connectors[0]),
                    new List<int>(connectors[1]),
                    new List<int>(connectors[2])
                };
                break;

            case 2:
                newBounds.SetMinMax(new Vector3(-bounds.max.x, -bounds.max.y), new Vector3(-bounds.min.x, -bounds.min.y));
                bounds = newBounds;

                newConnectors = new List<int>[] {
                    new List<int>(connectors[2]),
                    new List<int>(connectors[3]),
                    new List<int>(connectors[0]),
                    new List<int>(connectors[1])
                };
                break;

            case 3:
                newBounds.SetMinMax(new Vector3(-bounds.max.y, bounds.min.x), new Vector3(-bounds.min.y, bounds.max.x));
                bounds = newBounds;

                newConnectors = new List<int>[] {
                    new List<int>(connectors[1]),
                    new List<int>(connectors[2]),
                    new List<int>(connectors[3]),
                    new List<int>(connectors[0])
                };
                break;
        }
    }



    public static Room GetRoom(int connections, byte direction)
    {
        Debug.Log("Conn" + connections);
        foreach (Room room in RandomHelper.Shuffle(BaseRooms[connections]))
            if (room.connectors[direction].Count > 0)
                return room;

        throw new Exception("No room with " + connections + " connections and connections on each side!");
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
