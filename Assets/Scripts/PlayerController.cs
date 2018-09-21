using UnityEngine;



public class PlayerController : Character
{
    public float speed;
    public float health;

    public Rigidbody2D rb2d;

    private void Start()
    {
        CurrentHealth = health;
        MaxHealth = health;
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (move.sqrMagnitude == 0)
            return;

        move = move.normalized * speed;
        Move(rb2d, gameObject.transform.position, move);
    }

    public void DealDamage(float damage)
    {
        CurrentHealth -= damage;
        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //die
    }


}
