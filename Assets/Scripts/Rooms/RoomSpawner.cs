using UnityEngine;
using System.Collections.Generic;
using System.Linq;



public class RoomSpawner : MonoBehaviour
{
    public GameObject roomPrefab;



    public Room startRoom = new Room(Vector2Int.zero);
    public DictonaryGrid pathways = new DictonaryGrid();

    public int iterations = 3;
    public int rooms = 6;
    public int scale = 40;



    private const float randomThreshold = .2f;
    private const float randomRoomChance = .5f;



    private void Start()
    {
        CreateRoomTree();
        SpawnRooms();
    }



    public void CreateRoomTree()
    {
        if (rooms / iterations < 1 + randomThreshold)
        {
            Debug.LogError("Unsafe number of iterations for the specified number of rooms.");
            return;
        }


        startRoom.Children.Add(new Room(Room.GetDirectionVector()));
        
        for (int currentIteration = 0; currentIteration < iterations; currentIteration++)
        {
            float ratio = rooms / ((iterations - currentIteration) * startRoom.GetChildCountAtLevel(currentIteration + 1));

            foreach (Room child in startRoom.GetChildrenAtLevel(currentIteration + 1))
            {
                int numberOfRooms = GetRandom(ratio);

                for (int roomNumber = 0; roomNumber < numberOfRooms; roomNumber++)
                {
                    byte direction;
                    Vector2Int resultVector;
                    byte i = 0;

                    do
                    {
                        direction = (byte)Random.Range(0, 8);
                        resultVector = child.Position + Room.GetDirectionVector(direction);

                        if (Room.takenPositions.Contains(resultVector))
                        {
                            var possibleDirections = child.GetAvailableSpawnDirections();
                            direction = possibleDirections[Random.Range(0, possibleDirections.Count)];
                        }
                    }
                    while (pathways.GetPathway(child.Position, direction) && i++ < 8);

                    if (i < 8) // Valid room position
                    {
                        pathways.SetPathway(resultVector, direction);
                        Room.takenPositions.Add(resultVector);



                        child.Children.Add(new Room(resultVector));
                    }
                }
                

                Debug.Log(currentIteration + "    " + child.Position);
            }
        } 
    }



    public void SpawnRooms()
    {
        for (int currentIteration = 0; currentIteration < iterations; currentIteration++)
        {
            foreach (Room child in startRoom.GetChildrenAtLevel(currentIteration + 1))
            {
                Vector3 position = new Vector3(child.Position.x * scale, child.Position.y * scale);
                Instantiate(roomPrefab, position, transform.rotation, transform);
            }
        }
    }



    public int GetRandom(float baseNumber)
    {
        float decimalPart = Mathf.Round(baseNumber) - baseNumber;

        if (Mathf.Abs(decimalPart) > randomThreshold && 1 - Mathf.Abs(decimalPart) > randomThreshold)
            return Mathf.FloorToInt(baseNumber) + (Random.value < decimalPart ? 1 : 0);
        else
            return Mathf.FloorToInt(baseNumber) + (Random.value < randomRoomChance ? Random.Range(0, 2) - 1 : 0);
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

    

    public int GetChildCountAtLevel(int level)
    {
        int count = 0;

        if (level > 1)
        {
            foreach (Room room in Children)
                count += room.GetChildCountAtLevel(level - 1);

            return count;
        }
        else
            return Children.Count;
    }



    public static Vector2Int GetDirectionVector(byte value)
    {
        return Directions[value];
    }

    public static Vector2Int GetDirectionVector()
    {
        return GetDirectionVector((byte)Random.Range(0, 8));
    }
}



public class DictonaryGrid
{
    private Dictionary<Vector2Int, byte> grid = new Dictionary<Vector2Int, byte>();

    

    public byte GetPosition(Vector2Int pos)
    {
        byte outValue = 0;

        grid.TryGetValue(pos, out outValue);

        return outValue;
    }



    public bool GetPathway(Vector2Int position, int direction)
    {
        switch (direction)
        {
            case 4:
            case 5:
                position += Vector2Int.left;
                break;
            case 6:
                position += new Vector2Int(-1, -1);
                break;
            case 7:
            case 8:
                position += Vector2Int.down;
                break;
        }

        direction %= 4;



        return (GetPosition(position) & (1 << direction)) > 0;
    }



    public void SetPathway(Vector2Int position, byte direction)
    {
        switch (direction)
        {
            case 4:
            case 5:
                position += Vector2Int.left;
                break;
            case 6:
                position += new Vector2Int(-1, -1);
                break;
            case 7:
            case 8:
                position += Vector2Int.down;
                break;
        }

        direction %= 4;

        grid[position] = 1;
    }
}
