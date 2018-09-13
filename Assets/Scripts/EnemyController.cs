using UnityEngine;



public class EnemyController : MonoBehaviour
{
    public float speed;
    public int damage;
    public int health;
    public float aggroRange;
    public Rigidbody2D rb2d;
    public float stunLength;



    //private static float stunAnimationLength = 0.1f;
    private const float knockbackDistance = 2;

    private GameObject target = null;
    private float internalSpeed;
    private float internalAggroRange;
    private bool stunned;
    //private bool inStunAnimation;
    private float stunStart;
    


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
    }



    private void FixedUpdate()
    {
        if (target != null && !stunned)
        {
            Vector2 move = target.transform.position - gameObject.transform.position;
            //rb2d.velocity = move.normalized * internalSpeed * Time.deltaTime;
            Vector2 v = move.normalized * speed;
            rb2d.MovePosition((Vector2)gameObject.transform.position + v);
        }

        /*if(inStunAnimation && Time.time - stunStart >= stunAnimationLength)
        {
            rb2d.velocity = Vector2.zero;
            inStunAnimation = false;
        }*/
    }



    void OnTriggerEnter2D(Collider2D collEvent)
    {
        if (collEvent.gameObject.tag == "Player")
        {
            //rb2d.velocity = -rb2d.velocity.normalized * knockbackVelocity;
            Vector2 normalToTarget = (target.transform.position - gameObject.transform.position).normalized;
            rb2d.MovePosition((Vector2)gameObject.transform.position - (normalToTarget*knockbackDistance));
            stunStart = Time.time;
            stunned = true;
            //inStunAnimation = true;
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
        target = overlap.gameObject;
    }
}
