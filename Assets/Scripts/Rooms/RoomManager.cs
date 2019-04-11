using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;



[ExecuteInEditMode]
[System.Serializable]
public class RoomManager : MonoBehaviour 
{
    // Necessary because unity cannot serialize an array of lists
    [HideInInspector]
    public List<int>[] connectors = new List<int>[4];
    [HideInInspector]
    public string connectorsString = "0|0|0|0";

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

            //Debug.Log("Center: " + bounds.center + "   Extents: " + bounds.extents);
            //Debug.Log("Min: " + bounds.min + "   Max: " + bounds.max);
        }
    }



    public void SetState(bool enabled)
    {
        gameObject.SetActive(enabled);
    }

    public void UpdateConnectors()
    {
        connectors = new List<int>[4];
        string[] sides = connectorsString.Split('|');

        for (int i = 0; i < 4; i++)
            connectors[i] = sides[i].Length == 0 ? new List<int>() : sides[i].Split(',').Select(int.Parse).ToList();
    }



    public override string ToString()
    {
        return connectorsString;
    }
}