using UnityEngine;



public class TeleporterBlock : MonoBehaviour 
{
    public Transform target;
    public Vector3 offset;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (roomClear && collision.gameObject.tag == "Player")
        {
            collision.transform.position = target.position + target.gameObject.GetComponent<TeleporterBlock>().offset;
        }
    }
}
