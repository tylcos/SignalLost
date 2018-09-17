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
        if(move == Vector2.zero) { return;  }

        //rb2d.velocity = move.normalized * internalSpeed * Time.deltaTime;
        Vector2 v = move.normalized * speed;
        Move(rb2d, gameObject.transform.position, v);
    }
}
