using UnityEngine;
using UnityEngine.Tilemaps;



public class TeleporterBlock : MonoBehaviour 
{
    public Transform room;
    public TileBase tile;

    public Transform target;
    public Vector3 offset;

    public bool roomClear = true;



    private void Start()
    {
        Vector3Int position = new Vector3Int(10, 0, 110);

        Tilemap tilemap = room.GetComponent<Tilemap>();
        tilemap.SetTile(position, tile);
        tilemap.RefreshTile(position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (roomClear && collision.gameObject.tag == "Player")
        {
            collision.transform.position = target.position + target.gameObject.GetComponent<TeleporterBlock>().offset;
        }
    }
}
