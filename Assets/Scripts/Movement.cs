using UnityEngine;



public class Movement : MonoBehaviour
{
    public float speed;

    public Rigidbody2D rb;



    private float internalSpeed;



    void Start()
    {
        internalSpeed = speed / 100;
    }

    void FixedUpdate()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rb.velocity = move.normalized * internalSpeed * Time.deltaTime ;
    }
}
