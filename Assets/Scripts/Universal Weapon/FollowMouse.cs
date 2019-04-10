using UnityEngine;



public class FollowMouse : MonoBehaviour
{
    public Texture2D cursorTexture;



    private Vector3 lastMousePosition = new Vector3(0, 0, float.MaxValue);



    void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(16, 16), CursorMode.ForceSoftware);
    }
    
    void FixedUpdate()
	{
        Vector2 shootDir = DungeonGameManager.GetAimingVector();

        float angleDifference = 0;
        if (shootDir.sqrMagnitude == 0)
        {
            Vector3 trueMousePos = Input.mousePosition;
            if (lastMousePosition == new Vector3(0, 0, float.MaxValue) || lastMousePosition != trueMousePos || Input.GetAxis("Fire1") > 0)
            {
                lastMousePosition = trueMousePos;
                Vector3 mousePosRelative = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float mouseAngle = Mathf.Rad2Deg * Mathf.Atan2(mousePosRelative.y, mousePosRelative.x);
                angleDifference = mouseAngle - transform.eulerAngles.z;
            }
        }
        else
            angleDifference = (Mathf.Rad2Deg * Mathf.Atan2(shootDir.y, shootDir.x)) - transform.eulerAngles.z;

        transform.RotateAround(transform.position, Vector3.forward, angleDifference);
    }
}
