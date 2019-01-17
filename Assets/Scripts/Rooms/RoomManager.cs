using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;



public class RoomManager : MonoBehaviour 
{
    // Necessary because unity cannot serialize an array of lists
    public List<int>[] connectors = new List<int>[4];
    public List<int> side1 = new List<int>(); public List<int> side2 = new List<int>();
    public List<int> side3 = new List<int>(); public List<int> side4 = new List<int>();

    public BoundsInt bounds;



    private void OnEnable()
    {
        // Update bounds
        var roomBounds = gameObject.GetComponent<Tilemap>().localBounds;
        var min = roomBounds.min + transform.position;
        var size = roomBounds.size;

        bounds = new BoundsInt((int)min.x, (int)min.y, 0, (int)size.x, (int)size.y, 0);

        gizmosEnabled = true;
    }



    private bool gizmosEnabled;
    void OnDrawGizmos()
    {
        UpdateConnectors();
        if (!gizmosEnabled)
            OnEnable();
        

        Gizmos.color = Color.magenta;

        var minX = bounds.min + new Vector3(1, 0.5f);
        var minY = bounds.min + new Vector3(0.5f, 1);
        foreach (int pos in connectors[0]) Gizmos.DrawCube(minX + new Vector3Int(pos, 0, 0), new Vector3(2,1));
        foreach (int pos in connectors[1]) Gizmos.DrawCube(minX + new Vector3Int(pos, bounds.size.y - 1, 0), new Vector3(2, 1));
        foreach (int pos in connectors[2]) Gizmos.DrawCube(minY + new Vector3Int(0, pos, 0), new Vector3(1, 2));
        foreach (int pos in connectors[3]) Gizmos.DrawCube(minY + new Vector3Int(bounds.size.x - 1, pos, 0), new Vector3(1, 2));
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

    public void UpdateConnectors()
    {
        // Update connectors
        connectors[0] = side1;
        connectors[1] = side2;
        connectors[2] = side3;
        connectors[3] = side4;
    }
}