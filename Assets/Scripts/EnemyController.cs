using UnityEngine;



public class EnemyController : MovementController
{
    public float speed;
    public int damage;
    public EnemyData data;
    public Rigidbody2D rb2d;
    public float stunLength;



    //private static float stunAnimationLength = 0.1f;
    private const float knockbackDistance = 2;

    private GameObject target = null;
    private float health;
    private float internalSpeed;
    private float internalAggroRange;
    private bool stunned;
    //private bool inStunAnimation;
    private float stunStart;
    


    void Start()
    {
        health = data.health;
        internalSpeed = speed * 100;
        internalAggroRange = data.aggroRange * data.aggroRange;
    }
    


    void Update()
    {
        if(stunned && Time.time - stunStart > stunLength)
        {
            stunned = false;
        }

        if (target != null)
        {
            CheckForTarget();
        }
        else
        {
            AttemptFindNewTarget();
        }
    }



    private void FixedUpdate()
    {
        if (target != null && !stunned)
        {
            Vector2 move = target.transform.position - gameObject.transform.position;
            //rb2d.velocity = move.normalized * internalSpeed * Time.deltaTime;
            Vector2 v = move.normalized * speed;
            //rb2d.MovePosition((Vector2)gameObject.transform.position + v);
            Move(rb2d, gameObject.transform.position, v);
        }
        else
            rb2d.velocity = Vector2.zero;
    }



    void OnTriggerEnter2D(Collider2D collEvent)
    {
        if (collEvent.gameObject.tag == "Player")
        {
            //rb2d.velocity = -rb2d.velocity.normalized * knockbackVelocity;
            Vector2 normalToTarget = (target.transform.position - gameObject.transform.position).normalized;
            //rb2d.MovePosition((Vector2)gameObject.transform.position - (normalToTarget*knockbackDistance));
            Move(rb2d, gameObject.transform.position, normalToTarget * -knockbackDistance);
            stunStart = Time.time;
            stunned = true;
            //inStunAnimation = true;
        }
        else if (collEvent.gameObject.tag == "Projectile")
        {
            BulletManager bm = collEvent.gameObject.GetComponent<BulletManager>();

            health -= bm.damage;
            if (health <= 0)
                Die();

            // hit();
        }
    }



    private void CheckForTarget()
    {
        if ((transform.position - target.transform.position).sqrMagnitude > internalAggroRange)
            target = null;
    }



    private void AttemptFindNewTarget()
    {
        Collider2D overlap = Physics2D.OverlapCircle(gameObject.transform.position, data.aggroRange, LayerMask.GetMask("Player"));
        target = (overlap == null) ? null : overlap.gameObject;
    }



    private void Die() // Implement later for death animation / loot
    {
        Destroy(gameObject);
    }
}
