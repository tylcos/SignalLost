using UnityEngine;



public class PlayerController : MovementController
{
    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 move = GameController.GetMovementVector();
        
        if (move.sqrMagnitude == 0)
            return;

        Move(move.normalized);
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
