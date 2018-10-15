using UnityEngine;



public class EnemyController : MovementController
{
    public float aggroRange;
    public float retreatDistance;
    public float waitTime;
    public float damage;

    public float attackRange;
    public float attackCooldownLength;
    protected float lastAttackTime;

    public string[] targetLayers;
    protected int targetLayerMask;

    public string[] collideLayers;
    protected int collideLayerMask;

    public GameObject attackIndicator;

    //private bool attacking = false;
    protected GameObject target;
    protected bool attacking = false;
    



    private void Start()
    {
        targetLayerMask = LayerMask.GetMask(targetLayers);
        collideLayerMask = LayerMask.GetMask(collideLayers);
        lastAttackTime = -attackCooldownLength;
    }



    protected virtual void Update()
    {
        if(target != null)
        {
            if(!IsGameObjectInRadius(transform.position, target, aggroRange)) {
                target = null;
            }
        }
        else
        {
            target = AttemptFindNewSingleTarget(transform.position, aggroRange, targetLayerMask);
        }
    }

    
    public virtual void OnHitOpponent(GameObject entityHit)
    {
        entityHit.GetComponent<MovementController>().Damage(damage);
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
