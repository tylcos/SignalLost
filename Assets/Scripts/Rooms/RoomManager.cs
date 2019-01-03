using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;



public class RoomManager : MonoBehaviour 
{
    public BoundsInt bounds;

    public int[] bottomConnectors;
    public int[] leftConnectors;
    public int[] topConnectors;
    public int[] rightConnectors;



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

        Gizmos.color = Color.blue;

        var minX = bounds.min + new Vector3(1, 0.5f);
        var minY = bounds.min + new Vector3(.5f, 1);
        foreach (int pos in bottomConnectors) Gizmos.DrawCube(minX + new Vector3Int(pos, 0, 0), new Vector3(2,1));
        foreach (int pos in topConnectors)    Gizmos.DrawCube(minX + new Vector3Int(pos, bounds.size.y - 1, 0), new Vector3(2, 1));
        foreach (int pos in leftConnectors)   Gizmos.DrawCube(minY + new Vector3Int(0, pos, 0), new Vector3(1, 2));
        foreach (int pos in rightConnectors)  Gizmos.DrawCube(minY + new Vector3Int(bounds.size.x - 1, pos, 0), new Vector3(1, 2));
    }

    void OnValidate()
    {
        bottomConnectors = bottomConnectors.Select(c => Mathf.Clamp(c, 0, bounds.size.x - 2)).ToArray();
        topConnectors = topConnectors.Select(c => Mathf.Clamp(c, 0, bounds.size.x - 2)).ToArray();
        leftConnectors = leftConnectors.Select(c => Mathf.Clamp(c, 0, bounds.size.y - 2)).ToArray();
        rightConnectors = rightConnectors.Select(c => Mathf.Clamp(c, 0, bounds.size.y - 2)).ToArray();
    }



    public void SetState(bool enabled)
    {
        gameObject.SetActive(enabled);
    }
}