using UnityEngine;



public class Movement : MonoBehaviour
{
    public float speed;

    public Rigidbody2D rb;



    void FixedUpdate()
    {
        Vector2 move = new Vector2();

        move += Vector2.up    * Input.GetAxisRaw("Vertical");
        move += Vector2.right * Input.GetAxisRaw("Horizontal");

        rb.velocity = move * speed;
    }
}
