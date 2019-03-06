using System.Collections;
using UnityEngine;

public class MeleeEnemyController : EnemyController {

    #region events and fields

    [Tooltip("Time it takes to charge up an attack in seconds.")]
    public float attackChargeTime;
    [Tooltip("Time to keep the weapon's collider on in attacks.")]
    public float attackColliderEnableDuration;
    [Tooltip("Time to wait immediately after attacking in seconds.")]
    public float postAttackPauseDuration;
    [Tooltip("Distance to run away from the player after attacking.")]
    public float postAttackRetreatDistance;

    #endregion


    #region event handlers

    public override void OnHitOpponent(MovementController opponent)
    {
        if (Time.time - lastAttackTime > attackCooldownLength)
        {
            base.OnHitOpponent(opponent);
            lastAttackTime = Time.time;
        }
        // do anything specific to the melee enemy here
    }

    #endregion


    #region monobehavior



    void FixedUpdate()
    {
        if(Time.time - lastAttackTime <= attackCooldownLength) { return; } // cancel if this enemy just attacked

        if (target != null && !attacking) // if the enemy is not attacking, do movement and try to attack
        {
            Vector2 vectorToTarget = Vector2.zero;
            RaycastHit2D[] hits = new RaycastHit2D[raycastDepth];
            Physics2D.RaycastNonAlloc(weaponHolder.transform.position, target.transform.position - transform.position, hits, aggroRange, collideLayerMask);
            string hitTag = null;
            bool hitIsTarget = false;
            foreach (RaycastHit2D hit in hits)
            {
                // go through hits from closest to farthest
                if (hit.collider != null && hit.fraction != 0)
                {
                    hitTag = hit.collider.tag;
                    vectorToTarget = hit.point - (Vector2)transform.position;

                    // if the tag of this object is a target, then we will target this one since it is the closest
                    foreach (string tag in targetTags)
                    {
                        if (tag == hitTag)
                        {
                            hitIsTarget = true;
                            break;
                        }
                    }

                    if (hitIsTarget) { break; }
                }
            }
            
            if (vectorToTarget != Vector2.zero)
            {
                if (vectorToTarget.magnitude < aggroRange) // if within aggro range, move and aim weapon towards target
                {
                    //MoveTowards(vectorToTarget);
                    Move(vectorToTarget);
                    AimWeaponAtTarget(vectorToTarget);
                }
                if (Time.time - lastAttackTime > attackCooldownLength && vectorToTarget.magnitude < attackRange && hitIsTarget) // attack
                {
                    StartCoroutine(SwordAttack(vectorToTarget, attackChargeTime, attackColliderEnableDuration, postAttackPauseDuration, postAttackRetreatDistance));
                }
            }
        }
    }

    #endregion


    /// <summary>
    ///     Times the attacking cycle for a sword style attack
    /// </summary>
    /// <remark>
    ///    The total time this coroutine takes to execute is equal to chargeTime+enableLength+pauseTime+retreatLength
    /// </remark>
    ///     <param name="vectorToTarget">The vector from the center of this gameObject's transform to the target</param>
    ///     <param name="chargeTime">The time this enemy waits before attacking</param>
    ///     <param name="enableLength">How long to enable the weapon's collider for</param>
    ///     <param name="pauseTime">The time after turning off the collider this enemy waits before retreating</param>
    ///     <param name="retreatLength">The length of time over which this enemy retreats</param>
    private IEnumerator SwordAttack(Vector2 vectorToTarget, float chargeTime, float enableLength, float pauseTime, float retreatLength)
    {
        attacking = true;
        AimWeaponAtTarget(vectorToTarget);
        float startTime = Time.time;
        Coroutine retreat = null;
        bool doing = true;
        while(doing)
        {
            Vector3 startingPosition = transform.position;
            if(Time.time < startTime + chargeTime) // CHARGING ATTACK
            {
                attackIndicator.SetActive(true);
                // do animations here
                yield return new WaitForSeconds(.05f);
            } else if(Time.time < startTime + chargeTime + enableLength) // ATTACKING
            {
                attackIndicator.SetActive(false);
                weapon.GetComponent<Collider2D>().enabled = true;
                // do animations here
                yield return new WaitForSeconds(.05f);
            } else if (Time.time < startTime + chargeTime + enableLength + pauseTime) // STANDING STILL AFTER ATTACK (window for the player to counter)
            {
                weapon.GetComponent<Collider2D>().enabled = false;
                // do animations here
                yield return new WaitForSeconds(.05f);
            } else if(Time.time > startTime+chargeTime+enableLength+pauseTime && doing) // RETREATING (jumping backwards)
            {
                if (retreat == null)
                {
                    AimWeaponAtTarget(-vectorToTarget);
                    retreat = MoveTo(-vectorToTarget.normalized * retreatLength);
                } else if (!RunningThisRoutine(retreat))
                {
                    doing = false;
                }
                // do animations here
                yield return new WaitForFixedUpdate();
            }
        }
        attacking = false;
    }

    
}
