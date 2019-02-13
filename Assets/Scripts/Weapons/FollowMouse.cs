using UnityEngine;



public class FollowMouse : MonoBehaviour 
{
    public Texture2D cursorTexture;



    void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(16, 16), CursorMode.ForceSoftware);
    }

    
    
    void FixedUpdate() 
	{
        Vector2 shootDir = GameController.GetMovementVector();

        float angleDifference;
        if (shootDir.sqrMagnitude == 0)
        {
            Vector3 mousePosRelative = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            float mouseAngle = Mathf.Rad2Deg * Mathf.Atan2(mousePosRelative.y, mousePosRelative.x);
            angleDifference = mouseAngle - transform.eulerAngles.z;
        }
        else
            angleDifference = (Mathf.Rad2Deg * Mathf.Atan2(shootDir.y, shootDir.x)) - transform.eulerAngles.z;

        transform.RotateAround(transform.position, Vector3.forward, angleDifference);
    }
}
