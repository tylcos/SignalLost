using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;



[ExecuteInEditMode]
public class RoomManager : MonoBehaviour 
{
    // Necessary because unity cannot serialize an array of lists
    public List<int>[] connectors = new List<int>[4];
    public List<int> side1 = new List<int>(); public List<int> side2 = new List<int>();
    public List<int> side3 = new List<int>(); public List<int> side4 = new List<int>();

    public Bounds bounds;



    private Tilemap[] tilemaps;
    private Vector3 center;



    private void OnEnable()
    {
        if (tilemaps == null)
            tilemaps = gameObject.GetComponentsInChildren<Tilemap>();

        // Update bounds
        bounds = tilemaps[0].localBounds;
        center = bounds.center + transform.position;
        var size = bounds.size;

        UpdateConnectors();
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        var centerX = center + new Vector3(0, -bounds.extents.y + 0.5f);
        var centerY = center + new Vector3(-bounds.extents.x + 0.5f, 0);

        foreach (int pos in connectors[0]) Gizmos.DrawCube(centerX + new Vector3(pos, 0, 0), new Vector3(2,1));
        foreach (int pos in connectors[1]) Gizmos.DrawCube(centerX + new Vector3(pos, bounds.size.y - 1, 0), new Vector3(2, 1));
        foreach (int pos in connectors[2]) Gizmos.DrawCube(centerY + new Vector3(0, pos, 0), new Vector3(1, 2));
        foreach (int pos in connectors[3]) Gizmos.DrawCube(centerY + new Vector3(bounds.size.x - 1, pos, 0), new Vector3(1, 2));
    }


    
    [ContextMenu("Update Bound Size")]
    public void UpdateBoundSize()
    {
        foreach (Tilemap tilemap in tilemaps)
            tilemap.CompressBounds();

        OnEnable();
    }



    public void SetState(bool enabled)
    {
        gameObject.SetActive(enabled);
    }

    public void UpdateConnectors()
    {
        connectors[0] = side1;
        connectors[1] = side2;
        connectors[2] = side3;
        connectors[3] = side4;
    }
}