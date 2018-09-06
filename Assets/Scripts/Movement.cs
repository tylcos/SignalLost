using UnityEngine;



public class Movement : MonoBehaviour
{
    public float speed;

    public Rigidbody2D rb;



    void FixedUpdate()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rb.velocity = move.normalized * speed * Time.deltaTime ;
    }
}
