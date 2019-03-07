using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : EnemyController {

    #region fields

    [Tooltip("Target distance to try and follow the player from.")]
    public float followDistance;
    [Tooltip("Distance at which to back away from the target.")]
    public float tooCloseThreshold;
    [Tooltip("Distance at which to get closer to the target.")]
    public float tooFarThreshold;
    [Tooltip("Time it takes to charge up an attack in seconds.")]
    public float attackChargeTime;
    [Tooltip("Time to wait immediately after attacking in seconds.")]
    public float postAttackPauseDuration;

    public WeaponController WC;
    
    [HideInInspector]
    public bool shooting = false;
    private Coroutine fleeing = null;

    #endregion


    #region event handlers

    public override void OnHitOpponent(MovementController opponent, bool killedOpponent)
    {
        if (Time.time - lastAttackTime > attackCooldownLength)
        {
            base.OnHitOpponent(opponent, killedOpponent);
            lastAttackTime = Time.time;
        }
        // do anything specific to the melee enemy here
    }

    #endregion


    #region monobehavior



    void FixedUpdate () {
		if(target != null && !attacking && !RunningThisRoutine(fleeing))
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
                if (vectorToTarget.magnitude < aggroRange) // if within aggro range
                {
                    if (vectorToTarget.magnitude > attackRange || vectorToTarget.magnitude > tooFarThreshold)
                    {
                        //MoveTowards(vectorToTarget);
                        Move(vectorToTarget);
                        //AimWeaponAtTarget(vectorToTarget);
                        WC.AimInDirection(vectorToTarget);
                    } else if (vectorToTarget.magnitude < tooCloseThreshold && hitIsTarget)
                    {
                        //fleeing = MoveToLocation(vectorToTarget.normalized * -1 * Mathf.Abs(followDistance - vectorToTarget.magnitude));
                        fleeing = MoveTo(vectorToTarget.normalized * -1 * Mathf.Abs(followDistance - vectorToTarget.magnitude));
                        //AimWeaponAtTarget(-vectorToTarget);
                        WC.AimInDirection(-vectorToTarget);
                    } else if(Time.time - lastAttackTime > attackCooldownLength && vectorToTarget.magnitude < attackRange && hitIsTarget)
                    {
                        if (/*weapon.GetComponent<RangedWeapon>().CanFire()*/WC.CanFire())
                        {
                            StartCoroutine(ShootAttack(vectorToTarget, attackChargeTime, postAttackPauseDuration));
                        }
                    }
                }
            }
        }
	}

    #endregion


    private IEnumerator ShootAttack(Vector2 vectorToTarget, float chargeTime, float pauseTime)
    {
        attacking = true;
        //AimWeaponAtTarget(vectorToTarget);
        WC.AimInDirection(vectorToTarget);
        float startTime = Time.time;
        bool shot = false;
        while (Time.time < startTime + chargeTime + pauseTime)
        {
            Vector3 startingPosition = transform.position;
            if (Time.time < startTime + chargeTime) // CHARGING ATTACK
            {
                attackIndicator.SetActive(true);
                // do animations here
                yield return new WaitForSeconds(.05f);
            }
            else if (Time.time < startTime + chargeTime + pauseTime && !shot) // ATTACKING
            {
                attackIndicator.SetActive(false);
                // shoot gun
                /*StartCoroutine(weapon.GetComponent<RangedWeapon>().Shoot(vectorToTarget));
                while (shooting)
                {
                    yield return new WaitForSeconds(.05f);
                }*/
                WC.Fire(vectorToTarget);
                shot = true;
                startTime = Time.time - chargeTime;
                // call an IEnumerator that makes a flag that is true while running, then wait for that to complete (just yield while its true)
                // then keep on completing this IEnumerator
                // do animations here
                yield return new WaitForSeconds(.05f);
            }
            else if(Time.time < startTime + chargeTime + pauseTime && shot) // PLAYER RIPOSTE OPPORTUNITY
            {
                yield return new WaitForSeconds(.05f);
            }
        }
        attacking = false;
    }



}
