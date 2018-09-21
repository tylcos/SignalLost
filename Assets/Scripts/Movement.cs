using UnityEngine;



public class Movement : MovementController
{
    public float speed;

    public Rigidbody2D rb2d;



    void FixedUpdate()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (move.sqrMagnitude == 0)
            return;
        
        move = move.normalized * speed;
        Move(rb2d, gameObject.transform.position, move);
    }
}
