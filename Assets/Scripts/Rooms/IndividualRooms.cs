using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualRooms : MonoBehaviour {
//    RoomSpawner roomSpawnerObject = new RoomSpawner();

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.CompareTag("Room"))
        {
            Destroy(gameObject);
        }
    }
}
