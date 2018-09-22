using UnityEngine;



public class EnemyController : MovementController
{
    public float speed;
    public float aggroRange;
    public int damage;
    public float stunLength;
    public Character characterData;
    public Rigidbody2D rb2d;



    //private static float stunAnimationLength = 0.1f;
    private const float knockbackDistance = 2;

    private GameObject target;
    private float internalSpeed;
    private float internalAggroRange;
    private bool stunned;
    //private bool inStunAnimation;
    private float stunStart;
    private bool displayHealth = false;



    void Start()
    {
        internalSpeed = speed * 100;
        internalAggroRange = aggroRange * aggroRange;
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



        if(characterData.Health != characterData.MaxHealth && !displayHealth)
        {
            displayHealth = true;
        }
    }



    private void FixedUpdate()
    {
        if (target != null && !stunned)
        {
            Vector2 move = target.transform.position - transform.position;
            Vector2 v = move.normalized * speed;
            Move(rb2d, transform.position, v);
        }
        else
            rb2d.velocity = Vector2.zero;
    }



    void OnTriggerEnter2D(Collider2D collEvent)
    {
        if (collEvent.gameObject.tag == "Player")
        {
            Vector2 normalToTarget = (target.transform.position - transform.position).normalized;
            Move(rb2d, transform.position, normalToTarget * -knockbackDistance);
            stunStart = Time.time;
            stunned = true;
            //inStunAnimation = true;
        }
        else if (collEvent.gameObject.tag == "Projectile")
        {
            BulletManager bm = collEvent.gameObject.GetComponent<BulletManager>();

            characterData.Health -= bm.damage;
            if (characterData.Health <= 0)
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
        Collider2D overlap = Physics2D.OverlapCircle(gameObject.transform.position, aggroRange, LayerMask.GetMask("Player"));
        target = (overlap == null) ? null : overlap.gameObject;
    }



    private void Die() // Implement later for death animation / loot
    {
        Destroy(gameObject);
    }
}
