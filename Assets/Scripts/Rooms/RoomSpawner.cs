using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {

    private int randomRoom;
    public GameObject room;
    public GameObject currentRoom;
    private GameObject[] roomsList;

    /*   Get reference to currentRoom coordinates */
    //int zCoordinate = 38;
    Vector3 topPosition = new Vector3(0, 38, 0);
    Vector3 rightPosition = new Vector3(38, 0, 0);
    Vector3 botttomPosition = new Vector3(0, -38, 0);
    Vector3 leftPosition = new Vector3(-38, 0, 0);

    // Use this for initialization
    void Start()
    {
        RoomSpawning();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RoomSpawning()
    {
        for (int i = 0; i < 5; i++)
        {
            randomRoom = Random.Range(1, 4 + 1);

            // Spawn in room to the top
            if (randomRoom == 1)
            {
                currentRoom = Instantiate(room, topPosition, transform.rotation);
            }
            //Spawn in room to the right
            if (randomRoom == 2)
            {
                currentRoom = Instantiate(room, rightPosition, transform.rotation);
            }
            //Spawn in room to the bottom
            if (randomRoom == 3)
            {
                currentRoom = Instantiate(room, botttomPosition, transform.rotation);
            }
            //Spawn in room to the left
            if (randomRoom == 4)
            {
                currentRoom = Instantiate(room, leftPosition, transform.rotation);
            }
        }
    }
}
