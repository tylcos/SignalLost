using System.Collections;
using UnityEngine;



public class EnemyController : MovementController
{
    public float aggroRange;
    public int damage;
    public float stunLength;



    //private static float stunAnimationLength = 0.1f;
    public float knockbackDistance = 3;

    private GameObject target;
    private float internalAggroRange;
    private bool stunned;
    //private bool inStunAnimation;
    private float stunStart;
    public float knockbackTime;
    private bool knockingBack = false;
    private float knockbackStartTime;
    //private Knockback knockback;



    void Start()
    {
        internalAggroRange = aggroRange * aggroRange;
    }
    


    void Update()
    {
        if(stunned && Time.time - stunStart >= stunLength)
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
        if(knockingBack)
        {
            if(Time.time - knockbackStartTime >= knockbackTime)
            {
                knockingBack = false;
            }
        } else if (target != null && !stunned)
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
            PlayerController pc = collEvent.gameObject.GetComponent<PlayerController>();
            pc.DealDamage(this.damage);
            Vector2 normalToTarget = (target.transform.position - transform.position).normalized;
            Knockback(knockbackTime, -normalToTarget, knockbackDistance);
            stunStart = Time.time;
            stunned = true;
        }
        else if (collEvent.gameObject.tag == "Projectile")
        {
            BulletManager bm = collEvent.gameObject.GetComponent<BulletManager>();

            Health -= bm.damage;

            // hit();
        }
    }

    protected void Knockback(float duration, Vector2 unitVector, float distance)
    {
        knockbackStartTime = Time.time;
        knockingBack = true;

        StartCoroutine(MoveOverSeconds(gameObject, unitVector * distance, duration));

        
    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector2 end, float seconds)
    {
        float elapsedTime = 0;
        Vector2 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            transform.position = Vector2.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = end;
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
