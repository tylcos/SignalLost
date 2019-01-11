using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEditor;



public class RoomManager : MonoBehaviour 
{
    public BoundsInt bounds;

    public int[][] connectors = { new int[0], new int[0], new int[0], new int[0] };



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
        var minY = bounds.min + new Vector3(.5f, 1);
        foreach (int pos in connectors[0]) Gizmos.DrawCube(minX + new Vector3Int(pos, 0, 0), new Vector3(2,1));
        foreach (int pos in connectors[1]) Gizmos.DrawCube(minX + new Vector3Int(pos, bounds.size.y - 1, 0), new Vector3(2, 1));
        foreach (int pos in connectors[2]) Gizmos.DrawCube(minY + new Vector3Int(0, pos, 0), new Vector3(1, 2));
        foreach (int pos in connectors[3]) Gizmos.DrawCube(minY + new Vector3Int(bounds.size.x - 1, pos, 0), new Vector3(1, 2));
    }

    void OnValidate()
    {
        for (int i = 0; i < 4; i++)
            connectors[i] = connectors[i].Select(c => Mathf.Clamp(c, 0, bounds.size.x - 2)).ToArray();
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



public class Connector
{
    public int[] connections;

    public Connector()
    {
        connections = new int[0];
    }

    public int this [int i]
    {
        get { return connections[i]; }
        set { connections[i] = value; }
    }
}

