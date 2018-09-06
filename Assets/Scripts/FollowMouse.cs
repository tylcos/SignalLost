using UnityEngine;



public class FollowMouse : MonoBehaviour 
{
    public Texture2D cursorTexture;



    private float initialYOffset;



    void Start()
    {
        initialYOffset = transform.position.y;

        Cursor.SetCursor(cursorTexture, new Vector2(16, 16), CursorMode.ForceSoftware);
    }

    void Update() 
	{
        Vector3 parentPos = transform.parent.transform.position + new Vector3(0, initialYOffset);



        Vector2 shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));

        float angleDifference;

        if (shootDir.sqrMagnitude == 0)
        {
            Vector3 mousePosRelative = Camera.main.ScreenToWorldPoint(Input.mousePosition) - parentPos;

            float mouseAngle = Mathf.Rad2Deg * Mathf.Atan2(mousePosRelative.y, mousePosRelative.x);
            angleDifference = mouseAngle - transform.eulerAngles.z;
        }
        else
            angleDifference = (Mathf.Rad2Deg * Mathf.Atan2(shootDir.y, shootDir.x)) - transform.eulerAngles.z;

        transform.RotateAround(parentPos, Vector3.forward, angleDifference);
    }
}
