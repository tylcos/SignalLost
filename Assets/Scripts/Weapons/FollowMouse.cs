using UnityEngine;



public class FollowMouse : MonoBehaviour 
{
    public Texture2D cursorTexture;
    private GameController master;


    private void OnEnable()
    {
        master = GameObject.FindGameObjectWithTag("Master").GetComponent<GameController>();
    }


    void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(16, 16), CursorMode.ForceSoftware);
    }

    
    
    void FixedUpdate() 
	{
        Vector2 shootDir = Vector2.zero;
        if (master.inputMethod == "keyboard")
        {
            shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));
        }
        else if (master.inputMethod == "arcade")
        {
            shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeysArcade"), Input.GetAxisRaw("VerticalKeysArcade"));
        }
        // = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));

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
