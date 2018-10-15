using UnityEngine;
using System.Collections.Generic;



public class RoomSpawner : MonoBehaviour
{
    public Room startRoom;

    public int iterations = 2;
    public int rooms = 4;

    

    public void SpawnRooms()
    {
        Room baseRoom;

        for (int i = 0; i < iterations; i++)
        {
            

            int ratio = rooms / (iterations * startRoom.Children.Capacity);
        }
    }
}



public class Room
{
    public List<Room> Children = new List<Room>();
    public Vector2 Position = new Vector2();



    public List<Room> GetLowestChildren()
    {
        List<Room> children = new List<Room>();

        foreach (Room child in Children)
            children.AddRange(GetLowestChildren()):

        return children;
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
