using UnityEngine;
using System.Collections.Generic;



public class RoomSpawner : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject pathwayPrefab;



    public Room startRoom = new Room(Vector2Int.zero);
    public DictonaryGrid pathways = new DictonaryGrid();

    public int iterations = 3;
    public int rooms = 6;
    public int scale = 40;
    public float spawnSpeed = 1f;



    private const float lowerThreshhold = .2f;
    private const float upperThreshhold = .3f;
    private const float decreaseRandomChance = .2f;
    private const float increaseRandomChance = .3f;

    private int spawnedRoomCount = -1; // Offset the inital spawned room



    private void Start()
    {
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
            spawnedRoomCount = -1;

            startRoom = new Room(Vector2Int.zero);
            pathways = new DictonaryGrid();
            Room.takenPositions = new HashSet<Vector2Int>();

            CreateRoomTree();
        }
    }

    public void DeleteAllRooms()
    {
        foreach (Transform room in transform)
            Destroy(room.gameObject);
    }



    public void CreateRoomTree()
    {
        int startDirection = Random.Range(0, 8);
        startRoom.Children.Add(new Room(Room.GetDirectionVector(startDirection)));
        pathways.SetPathway(startRoom.Position, (byte)startDirection);
        SpawnRoom(startRoom.Children[0]);



        for (int currentIteration = 0; currentIteration < iterations; currentIteration++)
        {
            int spawnedChildCount = startRoom.GetChildrenCountAtLevel(currentIteration + 1);
            if (spawnedChildCount == 0) // Spawn rooms on last iteration rooms if no new rooms were spawned
            {
                --currentIteration;
                spawnedChildCount = startRoom.GetChildrenCountAtLevel(currentIteration + 1);
            }



            float ratio = (rooms - spawnedRoomCount) / ((iterations - currentIteration) * spawnedChildCount);

            foreach (Room child in startRoom.GetChildrenAtLevel(currentIteration + 1))
            {
                int numberOfRooms = GetRandom(ratio);
                List<byte> possibleDirections = child.GetAvailableSpawnDirections();

                for (int roomNumber = 0; roomNumber < numberOfRooms; roomNumber++)
                {
                    foreach (byte direction in RandomHelper.ShuffleList(possibleDirections))
                    {
                        if (pathways.GetPathwayValid(child.Position, direction))    // Valid position found
                        {
                            Vector2Int resultPosition = child.Position + Room.GetDirectionVector(direction);

                            possibleDirections.Remove(direction);
                            pathways.SetPathway(child.Position, direction);
                            Room newRoom = new Room(resultPosition);
                            child.Children.Add(newRoom);

                            SpawnRoom(newRoom);
                            pathwayPrefab.transform.position = (Vector3)((Vector2)child.Position) * scale + new Vector3(0, 0, -20);
                            break; // Exit once a valid direction is found
                        }
                    }
                }
            }
        }

        Debug.Log("Finished spawning " + (spawnedRoomCount + 1) + " rooms");
    }

    public void SpawnRoom(Room room)
    {
        Debug.Log("Spawning room at (" + room.Position.x + ", " + room.Position.y + ")");

        ++spawnedRoomCount;

        Vector3 position = new Vector3(room.Position.x * scale, room.Position.y * scale);
        Instantiate(roomPrefab, position, transform.rotation, transform);
    }

    public int GetRandom(float baseNumber)
    {
        float decimalPart = Mathf.Round(baseNumber) - baseNumber;

        if (Mathf.Abs(decimalPart) < lowerThreshhold)
            return Mathf.FloorToInt(baseNumber) + (Random.value < decreaseRandomChance ? -1 : 0);
        else if (1 - Mathf.Abs(decimalPart) < upperThreshhold)
            return Mathf.FloorToInt(baseNumber) + (Random.value < increaseRandomChance ? 1 : 0);

        return Mathf.FloorToInt(baseNumber) + (Random.value < decimalPart ? 1 : 0);
    }



    public void InstantiatePathways()
    {
        foreach (KeyValuePair<Vector2Int, byte> entry in pathways.grid)
        {
            foreach (Vector2 direction in GetDirectionVectors(entry.Value))
            {
                
                if (direction.x == -1 && direction.y == 1)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine((Vector2)(entry.Key + Vector2Int.right) * scale, ((entry.Key + Vector2Int.right) + direction) * scale);
                }
                else
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine((Vector2)entry.Key * scale, (entry.Key + direction) * scale);
                }
            }
        }
    }

    public List<Vector2> GetDirectionVectors(byte direction)
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
    public List<Room> Children = new List<Room>();
    public Vector2Int Position;

    public static HashSet<Vector2Int> takenPositions = new HashSet<Vector2Int>();

    public static Vector2Int[] Directions =
        { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 1), new Vector2Int(-1, 1),
            new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1) };



    public Room(Vector2Int position)
    {
        Position = position;
        takenPositions.Add(position);
    }



    public List<byte> GetAvailableSpawnDirections()
    {
        List<byte> validDirection = new List<byte>(8);

        for (byte i = 0; i < 8; i++)
            if (!takenPositions.Contains(Position + Directions[i]))
                validDirection.Add(i);

        return validDirection;
    }




    public List<Room> GetChildrenAtLevel(int level)
    {
        List<Room> currentChildren = new List<Room>();

        if (level > 1)
        {
            foreach (Room room in Children)
                currentChildren.AddRange(room.GetChildrenAtLevel(level - 1));

            return currentChildren;
        }
        else
            return Children;
    }

    public List<Room> GetLowestChildren()
    {
        List<Room> children = new List<Room>();

        foreach (Room child in Children)
            children.AddRange(GetLowestChildren());

        return children;
    }



    public int GetChildrenCountAtLevel(int level)
    {
        int count = 0;

        if (level > 1)
        {
            foreach (Room room in Children)
                count += room.GetChildrenCountAtLevel(level - 1);

            return count;
        }
        else
            return Children.Count;
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
