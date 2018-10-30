using UnityEngine;
using System.Collections.Generic;
using System.Linq;



public class RoomSpawner : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject pathwayPrefab;



    public Room startRoom = new Room(Vector2Int.zero);
    public DictonaryGrid pathways = new DictonaryGrid();

    public int iterations = 3;
    public int rooms = 6;
    public int scale = 40;



    private const float randomThreshold = .2f;
    private const float randomRoomChance = .5f;



    private void Start()
    {
        StartCoroutine(CreateRoomTree());
        InstantiatePathways();
    }



    public IEnumerator<WaitForSeconds> CreateRoomTree()
    {
        if (rooms / iterations < 1 + randomThreshold)
        {
            Debug.LogError("Unsafe number of iterations for the specified number of rooms.");
            //return;
        }


        startRoom.Children.Add(new Room(Room.GetDirectionVector()));
        
        for (int currentIteration = 0; currentIteration < iterations; currentIteration++)
        {
            float ratio = rooms / ((iterations - currentIteration) * startRoom.GetChildCountAtLevel(currentIteration + 1));

            foreach (Room child in startRoom.GetChildrenAtLevel(currentIteration + 1))
            {
                int numberOfRooms = GetRandom(ratio);
                List<byte> possibleDirections = child.GetAvailableSpawnDirections();

                for (int roomNumber = 0; roomNumber < numberOfRooms; roomNumber++)
                {
                    Debug.Log(possibleDirections[0]

                    foreach (byte direction in RandomHelper.ShuffleList(possibleDirections))
                    {
                        if (pathways.GetPathwayValid(child.Position, direction))    // Valid position found
                        {
                            Vector2Int resultVector = child.Position + Room.GetDirectionVector(direction);

                            possibleDirections.Remove(direction);
                            
                            pathways.SetPathway(child.Position, direction);
                            Room.takenPositions.Add(resultVector);
                            child.Children.Add(new Room(resultVector));



                            Vector3 position = new Vector3(child.Position.x * scale, child.Position.y * scale);
                            Instantiate(roomPrefab, position, transform.rotation, transform);

                            yield return new WaitForSeconds(4);

                            break;
                        }
                    }
                }
            }
        }
    }

    public void InstantiatePathways()
    {
        Vector3 pathwayOffset = new Vector3(.5f,.5f) * scale;

        foreach (KeyValuePair<Vector2Int, byte> entry in pathways.grid)
        {
            foreach (Vector3 direction in GetDirectionVectors(entry.Value))
            {
                Vector3 position = new Vector3(entry.Key.x, entry.Key.y) * scale + pathwayOffset;
                Vector3 rotation = new Vector3(0, 0, Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg);

                Instantiate(pathwayPrefab, position, Quaternion.Euler(rotation), transform);
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

    public List<Vector3> GetDirectionVectors(byte direction)
    {
        List<Vector3> directions = new List<Vector3>(4);
            
        if ((direction & 1) > 0)
            directions.Add((Vector2)Room.GetDirectionVector(0));
        if ((direction & 2) > 0)
            directions.Add((Vector2)Room.GetDirectionVector(1));
        if ((direction & 4) > 0)
            directions.Add((Vector2)Room.GetDirectionVector(2));
        if ((direction & 8) > 0)
            directions.Add((Vector2)Room.GetDirectionVector(3));

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



    public static Vector2Int GetDirectionVector(int value)
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
    public Dictionary<Vector2Int, byte> grid = new Dictionary<Vector2Int, byte>();

    

    public byte GetPosition(Vector2Int pos)
    {
        byte outValue = 0;

        grid.TryGetValue(pos, out outValue);

        return outValue;
    }



    public bool GetPathwayValid(Vector2Int position, byte direction)
    {
        CorrectPosition(ref position, ref direction);

        return (GetPosition(position) & (1 << direction)) == 0;
    }



    public void SetPathway(Vector2Int position, byte direction)
    {
        CorrectPosition(ref position, ref direction);

        Debug.Log("Corrected direction" + direction + "     " + position);
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
