using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEditor;



public class RoomManager : MonoBehaviour 
{
    public BoundsInt bounds;

    public Side[] sides = { new Side(), new Side(), new Side(), new Side() };



    private void OnEnable()
    {
        var roomBounds = gameObject.GetComponent<Tilemap>().localBounds;
        var min = roomBounds.min + transform.position;
        var size = roomBounds.size;

        bounds = new BoundsInt((int)min.x, (int)min.y, 0, (int)size.x, (int)size.y, 0);

        gizmosEnabled = true;
    }


    private bool gizmosEnabled;
    void OnDrawGizmos()
    {   
        if (!gizmosEnabled)
            OnEnable();

        Gizmos.color = Color.magenta;

        var minX = bounds.min + new Vector3(1, 0.5f);
        var minY = bounds.min + new Vector3(0.5f, 1);
        foreach (int pos in sides[0].connections) Gizmos.DrawCube(minX + new Vector3Int(pos, 0, 0), new Vector3(2,1));
        foreach (int pos in sides[1].connections) Gizmos.DrawCube(minX + new Vector3Int(pos, bounds.size.y - 1, 0), new Vector3(2, 1));
        foreach (int pos in sides[2].connections) Gizmos.DrawCube(minY + new Vector3Int(0, pos, 0), new Vector3(1, 2));
        foreach (int pos in sides[3].connections) Gizmos.DrawCube(minY + new Vector3Int(bounds.size.x - 1, pos, 0), new Vector3(1, 2));
    }

    void OnValidate()
    {
        for (int i = 0; i < 4; i++)
            sides[i].connections = sides[i].connections.Select(c => Mathf.Clamp(c, 0, bounds.size.x - 2)).ToArray();
    }
    
    [ContextMenu("Update Bound Size")]
    public void UpdateBoundSize()
    {
        OnEnable();
    }



    public void SetState(bool enabled)
    {
        gameObject.SetActive(enabled);
    }
}



[System.Serializable]
public class Side
{
    public int[] connections;

    public Side()
    {
        connections = new int[0];
    }
}
