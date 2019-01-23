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

            Room.BaseRooms[connections].Add(new Room(room, script));
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
                    int connections = new int[] { 1, 2, 4, 6 }[UnityEngine.Random.Range(0, 4)]; // TODO
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



    public static List<Room>[] BaseRooms;



    private static Transform roomSpawner;



    public Room(GameObject go, RoomManager rm)
    {
        gameObject = go;
        transform = go.transform;

        connectors = rm.connectors;
        bounds = rm.bounds;
    }

    public Room(GameObject go)
    {
        gameObject = go;
        transform = go.transform;

        var rm = go.GetComponent<RoomManager>();
        connectors = rm.connectors;
        bounds = rm.bounds;
    }

    public Room()
    {
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

    public void EnsureDirection(int direction)
    {
        if (!connectors[direction].Any())
        {
            int rotation;
            int[] indexes = { 0, 2, 1, 3 };
            for (rotation = 1; rotation < 4; rotation++)
            {
                int index = (Array.IndexOf(indexes, direction) + rotation) % 4;
                if (connectors[indexes[index]].Any())
                    break;
            }

            switch (rotation)
            {
                case 1:

                    break;
            }
        }
    }



    public Vector3 GetSpawnPosition(Connection connection, Room roomToSpawn)
    {
        var flippedDirection = connection.Flip();

        roomToSpawn.EnsureDirection(flippedDirection.Direction);
        var spawnConnector = roomToSpawn.GetConnections().First(c => c.Direction == flippedDirection.Direction);
        roomToSpawn.connectors[flippedDirection.Direction].Remove(connection.Position);

        return transform.position + GetConnectionOffset(connection) - roomToSpawn.GetConnectionOffset(spawnConnector);
    }



    public void Instantiate(Vector3 position)
    {
        Instantiate(position, roomSpawner.transform.rotation);
    }

    public void Instantiate(Vector3 position, Quaternion rotation)
    {
        gameObject = UnityEngine.Object.Instantiate(gameObject, position, rotation, roomSpawner);
        transform = gameObject.transform;
    }



    public static void Initialize(int maxConnections, Transform parent)
    {
        roomSpawner = parent;

        BaseRooms = new List<Room>[maxConnections];
        for (int i = 0; i < maxConnections; i++)
            BaseRooms[i] = new List<Room>(2);
    }



    public static Room GetRoom(int connections, byte direction)
    {
        foreach (Room room in RandomHelper.Shuffle(BaseRooms[connections]))
            if (room.connectors[direction].Any())
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
