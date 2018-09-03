using UnityEngine;



public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    public float mouseWeight = 2f;
	


	void Update()
	{
        // Direction vector scaled to the screen

        Vector3 mousePos = new Vector3(
            2f * (((Input.mousePosition.x) / Screen.width) - 0.5f),
            2f * (((Input.mousePosition.y) / Screen.height) - 0.5f),
            -100f);

        transform.position = player.position + mousePos * mouseWeight;

        Debug.Log(mousePos);
    }
}
