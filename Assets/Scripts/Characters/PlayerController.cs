using UnityEngine;



public class PlayerController : MovementController
{
    private GameController master;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        master = GameObject.FindGameObjectWithTag("Master").GetComponent<GameController>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 move = Vector2.zero;
        if(master.inputMethod == "keyboard")
        {
            move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        } else if(master.inputMethod == "arcade")
        {
            move = new Vector2(Input.GetAxisRaw("HorizontalArcade"), Input.GetAxisRaw("VerticalArcade"));
        }
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
