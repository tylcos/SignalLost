using UnityEngine;
using System.Collections.Generic;



public class RoomSpawner : MonoBehaviour
{
    public Room startRoom = new Room();

    public int iterations = 3;
    public int rooms = 6;



    private const float randomThreshold = .2f;
    private const float randomRoomChance = .5f;



    private void Start()
    {
        SpawnRooms();
    }



    public void SpawnRooms()
    {
        startRoom.Children.Add(new Room(Room.GetDirection()));
        
        for (int i = 0; i < iterations; i++)
        {
            float ratio = rooms / ((iterations - i) * startRoom.GetChildCountAtLevel(i + 1));

            foreach (Room child in startRoom.GetChildrenAtLevel(i + 1))
            {
                int numberOfRooms = GetRandom(ratio);

                child.Children.Add(new Room(Room.GetDirection()));

                Debug.Log(i + "    "  + child.Children.Count + "  Ratio :  " + ratio + "   Rooms :  " + numberOfRooms + "    Children " + startRoom.GetChildCountAtLevel(1 + i));
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



    public Room()
    {

    }

    public Room(Vector2Int position)
    {
        Position = position;
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



    public static Vector2Int GetDirection(int value)
    {
        Vector2Int[] directions = { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 1), new Vector2Int(-1, 1),
            new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1) };

        return directions[value];
    }

    public static Vector2Int GetDirection()
    {
        return GetDirection(Random.Range(0, 8));
    }
}



public class DictonaryGrid
{
    private Dictionary<Vector2, byte> grid = new Dictionary<Vector2, byte>();

    

    public byte GetPosition(Vector2 pos)
    {
        byte outValue = 0;

        grid.TryGetValue(pos, out outValue);

        return outValue;
    }



    public void SetPosition(Vector2 pos, byte value)
    {
        grid.Add(pos, value);
    }
}
