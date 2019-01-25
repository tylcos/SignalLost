using UnityEngine;



public class EnemyController : MovementController
{

    #region events and fields

    public const int raycastDepth = 2; // change this to increase the penetration depth of raycasts. Lower numbers wont penetrate walls
    
    public GameObject weaponHolder;
    public GameObject weapon;
    public GameObject attackIndicator;

    [Tooltip("Aggro range.")]
    public float aggroRange;
    [Tooltip("Damage dealt per hit.")]
    public float damage;
    [Tooltip("Targets within a circle of this radius can be attacked.")]
    public float attackRange;
    [Tooltip("Cooldown between attacks in seconds.")]
    public float attackCooldownLength;
    protected float lastAttackTime; // stores the last time this enemy attacked
    [Tooltip("The layers this enemy will search for their target on.")]
    public LayerMask targetLayerMask;
    [Tooltip("The tags that a target object can have.")]
    public string[] targetTags;

    protected GameObject target; // stores target gameobject

    protected bool attacking = false;

    public bool isAggro { get; private set; } = false;
    
    #endregion


    #region event handlers

    // Override this in child
    public virtual void OnHitOpponent(GameObject entityHit)
    {
        entityHit.GetComponent<MovementController>().Damage(damage);
    }

    #endregion


    #region monobehavior

    protected virtual void Start()
    {   
        lastAttackTime = -attackCooldownLength;
    }
    
    protected virtual void Update()
    {
        if(target != null)
        {
            if(!IsGameObjectInRadius(transform.position, target, aggroRange, targetLayerMask)) {
                target = null;
            }
        }
        else
        {
            target = AttemptFindNewSingleTarget(transform.position, aggroRange, targetLayerMask);
        }
        isAggro = target != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            BulletManager bm = collision.gameObject.GetComponent<BulletManager>();
            Damage(bm.damage);
        }
    }

    #endregion


    #region public
    
    /// <summary>
    ///     Looks for the target within a circle defined by center and radius
    /// </summary>
    /// <returns>
    /// A boolean representing the presence of the target
    /// </returns>
    ///     <param name="center">The center of the circle</param>
    ///     <param name="target">The target to look for</param>
    ///     <param name="radius">The radius of the circle</param>
    protected static bool IsGameObjectInRadius(Vector3 center, GameObject target, float radius, int layerMask)
    {
        return Mathf.Sqrt((center - target.transform.position).sqrMagnitude) <= radius;
    }

    #endregion


    #region protected

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

    /// <summary>
    ///     Aims this enemy's weapon towards the vector given by rotating the weapon holder
    /// </summary>
    ///     <param name="vectorToTarget">The vector from the center of this gameObject's transform to the target</param>
    protected void AimWeaponAtTarget(Vector2 vectorToTarget)
    {
        var xd = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        xd.z = xd.x;
        xd.x = 0;
        xd.y = 0;
        weaponHolder.transform.rotation = xd;
        float angleDifference = (Mathf.Rad2Deg * Mathf.Atan2(vectorToTarget.y, vectorToTarget.x)) - weaponHolder.transform.eulerAngles.z;

        weaponHolder.transform.RotateAround(weaponHolder.transform.position, Vector3.forward, 180 + angleDifference);
    }

    #endregion

}
