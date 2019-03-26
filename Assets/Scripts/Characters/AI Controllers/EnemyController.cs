using UnityEngine;



public class EnemyController : MovementController
{

    #region events and fields

    public const int raycastDepth = 2; // change this to increase the penetration depth of raycasts. Lower numbers wont penetrate walls
    
    public GameObject attackIndicator;

    public WeaponController WC;

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
    public override void OnHitDealt(MovementController opponent, bool killedOpponent)
    {
        base.OnHitDealt(opponent, killedOpponent);
        print("reached enemy child onhitopponent");
    }

    public override bool OnHitReceived(MovementController opponent, float damageReceived)
    {
        bool targetKilled = base.OnHitReceived(opponent, damageReceived);
        print("enemy was hit by player and took " + damageReceived + " damage");
        return targetKilled;
    }
    #endregion


    #region monobehavior

    protected virtual void Start()
    {
        lastAttackTime = -attackCooldownLength;
    }

    protected override void Update()
    {
        base.Update();
        if (target != null)
        {
            if (!IsGameObjectInRadius(transform.position, target, aggroRange, targetLayerMask))
            {
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
        /*if (collision.gameObject.CompareTag("Projectile"))
        {
            BulletController bm = collision.gameObject.GetComponent<BulletController>();
            Damage(bm.damage);
        }*/
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

    #endregion

}
