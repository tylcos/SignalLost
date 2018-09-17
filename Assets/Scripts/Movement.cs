using UnityEngine;



public class Movement : MovementController
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
        //rb2d.MovePosition((Vector2)gameObject.transform.position + v);
        Vector2 bcv = gameObject.GetComponent<BoxCollider2D>().size;
        Move(rb2d, gameObject.transform.position, v);
        // for the origin. it needs to be the edge of the collider
    }
}
