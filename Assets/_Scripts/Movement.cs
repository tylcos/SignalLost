using UnityEngine;



public class Movement : MonoBehaviour
{
    public float speed;

    public Rigidbody2D rb2d;



    void FixedUpdate()
    {
        Vector2 move = new Vector2();

        move += Vector2.up    * Input.GetAxisRaw("Vertical");
        move += Vector2.right * Input.GetAxisRaw("Horizontal");

        rb2d.velocity = move * speed;

    }
}
