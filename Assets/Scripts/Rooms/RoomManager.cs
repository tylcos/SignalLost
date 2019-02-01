using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;



[ExecuteInEditMode]
public class RoomManager : MonoBehaviour 
{
    // Necessary because unity cannot serialize an array of lists
    public List<int>[] connectors = new List<int>[4];
    public List<int> side1; public List<int> side2;
    public List<int> side3; public List<int> side4;

    public Bounds bounds;



    private Tilemap[] tilemaps;
    private Vector3 center;



    private void OnEnable()
    {
        if (tilemaps == null)
            tilemaps = gameObject.GetComponentsInChildren<Tilemap>();

        // Update bounds
        bounds = tilemaps[0].localBounds;
        center = transform.position;

        UpdateConnectors();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        /*
        Debug.Log("-------------");
        Debug.Log(transform.position);
        Debug.Log(bounds.center);
        Debug.Log(bounds.max.x);
        Debug.Log(bounds.min.x);

        Debug.Log(center + new Vector3(bounds.max.x - 0.5f, 0, 0));
        */

        foreach (int pos in connectors[0]) Gizmos.DrawCube(center + new Vector3(pos + bounds.center.x, bounds.min.y, 0), new Vector3(2f, .2f));
        foreach (int pos in connectors[1]) Gizmos.DrawCube(center + new Vector3(pos + bounds.center.x, bounds.max.y, 0), new Vector3(2f, .2f));
        foreach (int pos in connectors[2]) Gizmos.DrawCube(center + new Vector3(bounds.min.x, pos + bounds.center.y, 0), new Vector3(.2f, 2f));
        foreach (int pos in connectors[3]) Gizmos.DrawCube(center + new Vector3(bounds.max.x, pos + bounds.center.y, 0), new Vector3(.2f, 2f));
    }
#endif



    [ContextMenu("Update Bound Size")]
    public void UpdateBoundSize()
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            tilemap.CompressBounds();
            OnEnable();

            Debug.Log("Center: " + bounds.center + "   Extents: " + bounds.extents);
            Debug.Log("Min: " + bounds.min + "   Max: " + bounds.max);
        }
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