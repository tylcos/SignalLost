using UnityEngine;



public class Movement : MonoBehaviour
{
    public float speed;

    public Rigidbody2D rb2d;



    private float internalSpeed;



    void Start()
    {
        internalSpeed = speed * 100;
    }

    void FixedUpdate()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //rb2d.velocity = move.normalized * internalSpeed * Time.deltaTime;
        Vector2 v = move.normalized * speed;
        rb2d.MovePosition((Vector2)gameObject.transform.position + v);
    }
}
