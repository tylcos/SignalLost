using UnityEngine;
using System.Collections.Generic;



public class RoomSpawner : MonoBehaviour
{
    public Room startRoom;

    public int iterations = 3;
    public int rooms = 6;

    

    public void SpawnRooms()
    {
        startRoom.Children.Add(new Room(Room.GetDirection()));

        for (int i = 0; i < iterations; i++)
        {
            int ratio = rooms / ((iterations - i) * startRoom.GetChildCountAtLevel(1 + iterations));

            int numberOfRooms = x
        }
    }
}



public class Room
{
    public List<Room> Children = new List<Room>();
    public Vector2Int Position;



    public Room(Vector2Int position)
    {
        Position = position;
    }
    


    public List<Room> GetChildAtLevel(int level)
    {
        List<Room> currentChildren = new List<Room>();

        if (level > 1)
        {
            foreach (Room room in Children)
                currentChildren.AddRange(room.GetChildAtLevel(level - 1));

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
