using UnityEngine;



public class EnemyController : CharacterController
{
    public float aggroRange;
    public int damage;
    public float stunLength;



    //private static float stunAnimationLength = 0.1f;
    private const float knockbackDistance = 2;

    private GameObject target;
    private float internalAggroRange;
    private bool stunned;
    //private bool inStunAnimation;
    private float stunStart;



    void Start()
    {
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
    }



    void FixedUpdate()
    {
        if (target != null && !stunned)
        {
            Vector2 move = target.transform.position - transform.position;
            Vector2 v = move.normalized * Speed; 
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

            Health -= bm.damage;

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
}
