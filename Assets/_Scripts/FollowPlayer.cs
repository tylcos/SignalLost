using UnityEngine;



public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    public float mouseWeight = 2f;
	


	void Update()
	{
        transform.position = player.position + MouseWrapper.GetMouseDirection() * mouseWeight;
    }
}
