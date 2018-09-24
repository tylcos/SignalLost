using UnityEngine;



public class PlayerController : MovementController
{ 
    void FixedUpdate()
    {
        Movement();
    }
    


    void Movement()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (move.sqrMagnitude == 0)
            return;
        
        move = move.normalized * Speed;
        Move(rb2d, gameObject.transform.position, move);
    }



    public void DealDamage(float damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            Die();
        }
    }
}
