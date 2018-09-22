using UnityEngine;



public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    public float mouseWeight = 2f;
	
    

	void Update()
	{
        transform.position = player.position + MouseWrapper.GetMouseVector() * mouseWeight + new Vector3(0,0,-100);
    }
}
