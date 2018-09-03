using UnityEngine;



public class FollowMouse : MonoBehaviour 
{
    public Camera camera;
    public SpriteRenderer sr;

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
        Vector3 mousePosRelative = Camera.main.ScreenToWorldPoint(Input.mousePosition) - parentPos;

        float mouseAngle = Mathf.Rad2Deg * Mathf.Atan2(mousePosRelative.y, mousePosRelative.x);
        float angleDifference = mouseAngle - transform.eulerAngles.z;

        transform.RotateAround(parentPos, Vector3.forward, angleDifference);
        


        sr.flipY = Mathf.Abs(mouseAngle) > 90;
    }
}
