using UnityEngine;



public class FollowMouse : MonoBehaviour
{
    public Texture2D cursorTexture;



    void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(16, 16), CursorMode.ForceSoftware);
    }

    private Vector3 lastMousePosition = new Vector3(0, 0, float.MaxValue);

    void FixedUpdate()
	{
        Vector2 shootDir = GameManager.GetAimingVector();

        float angleDifference = 0;
        if (shootDir.sqrMagnitude == 0)
        {
            Vector3 trueMousePos = Input.mousePosition;
            if(lastMousePosition == new Vector3(0, 0, float.MaxValue) || lastMousePosition != trueMousePos || Input.GetAxis("Fire1") > 0)
            {
                lastMousePosition = trueMousePos;
                Vector3 mousePosRelative = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float mouseAngle = Mathf.Rad2Deg * Mathf.Atan2(mousePosRelative.y, mousePosRelative.x);
                angleDifference = mouseAngle - transform.eulerAngles.z;
                Cursor.visible = true;
            }
        }
        else
        {
            angleDifference = (Mathf.Rad2Deg * Mathf.Atan2(shootDir.y, shootDir.x)) - transform.eulerAngles.z;
            Cursor.visible = false;
        }

        transform.RotateAround(transform.position, Vector3.forward, angleDifference);

    }
}
