using Prime31;
using UnityEngine;



public class PlayerController : MovementController
{

    public CharacterController2D cc2d;

    private void Start()
    {
        
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

        //move = move.normalized * speed;
        //Move(rb2d, gameObject.transform.position, move);
        cc2d.move(move.normalized * (speed * Time.fixedDeltaTime));
        //MoveTowards(move.normalized);
        //CancelMovement();
        //MoveToLocation(move.normalized * 2);
        //MoveToRelativeToSource(transform.position, move.normalized * 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyWeapon")
        {
            collision.GetComponentInParent<EnemyController>().OnHitOpponent(gameObject);
        } else if(collision.tag == "Projectile")
        {
            collision.GetComponentInParent<BulletManager>().sourceEnemy.OnHitOpponent(gameObject);
        }
    }

}
