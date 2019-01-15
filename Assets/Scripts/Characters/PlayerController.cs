using UnityEngine;



public class PlayerController : MovementController
{

    private UIController ui;

    protected override void Awake()
    {
        base.Awake();
        ui = GameObject.FindGameObjectWithTag("UI Parent").GetComponent<UIController>();
    }
    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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
