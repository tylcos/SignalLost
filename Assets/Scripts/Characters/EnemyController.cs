using System.Collections;
using UnityEngine;



public class EnemyController : MovementController
{
    public float aggroRange;
    public float retreatDistance;
    public float damage;

    public float attackRange;
    //public GameObject weapon;
    public float attackLength;
    private float lastAttackTime;

    public string[] targetLayers;
    private int targetLayerMask;

    public string[] collideLayers;
    private int collideLayerMask;

    //private bool attacking = false;
    private GameObject target = null;

    private void Start()
    {
        targetLayerMask = LayerMask.GetMask(targetLayers);
        collideLayerMask = LayerMask.GetMask(collideLayers);
        lastAttackTime = -attackLength;
    }

    void Update()
    {
        if(target != null)
        {
            if(!IsGameObjectInRadius(transform.position, target, aggroRange)) {
                target = null;
            }
        } else
        {
            target = AttemptFindNewSingleTarget(transform.position, aggroRange, targetLayerMask);
        }
    }

    void FixedUpdate()
    {
        if(target != null)
        {
            Vector2 vectorToTarget = Vector2.zero;
            RaycastHit2D[] hits = new RaycastHit2D[2];
            Physics2D.RaycastNonAlloc(transform.position, target.transform.position - transform.position, hits, aggroRange, collideLayerMask);
            foreach(RaycastHit2D hit in hits)
            {
                if(hit.collider != null && hit.fraction != 0)
                {
                    vectorToTarget = hit.point - (Vector2)transform.position;
                }
            }
            if(!vectorToTarget.Equals(Vector2.zero))
            {
                if(vectorToTarget.magnitude > attackRange)
                {
                    Move(rb2d, transform.position, vectorToTarget.normalized * speed);
                } else if(Time.time - lastAttackTime > attackLength)
                {
                    Collider2D[] overlap = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayerMask);
                    foreach(Collider2D col in overlap)
                    {
                        col.gameObject.GetComponent<MovementController>().Damage(damage);
                        lastAttackTime = Time.time;
                    }
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            BulletManager bm = collision.gameObject.GetComponent<BulletManager>();
            Damage(bm.damage);
        }
    }

    /// <summary>
    ///     Looks for the target within a circle defined by center and radius
    /// </summary>
    /// <returns>
    /// A boolean representing the presence of the target
    /// </returns>
    ///     <param name="center">The center of the circle</param>
    ///     <param name="target">The target to look for</param>
    ///     <param name="radius">The radius of the circle</param>
    protected static bool IsGameObjectInRadius(Vector3 center, GameObject target, float radius)
    {
        return (center - target.transform.position).sqrMagnitude <= radius;
    }



    /// <summary>
    ///     Looks for and returns first enemy found within a circle defined by center and radius
    /// </summary>
    /// <remark>
    ///    Uses OverlapCircle
    /// </remark>
    /// <returns>
    /// The gameobject found or null
    /// </returns>
    ///     <param name="center">The center of the circle</param>
    ///     <param name="radius">The radius of the circle</param>
    ///     <param name="layerMask">The layer mask to use</param>
    protected static GameObject AttemptFindNewSingleTarget(Vector3 center, float radius, int layerMask)
    {
        Collider2D overlap = Physics2D.OverlapCircle(center, radius, layerMask);
        return (overlap == null) ? null : overlap.gameObject;
    }
}
