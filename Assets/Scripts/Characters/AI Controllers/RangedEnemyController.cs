using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : EnemyController {

    #region fields

    public float followDistance;
    public float tooCloseThreshold;
    public float tooFarThreshold;

    public float attackChargeTime;
    public float attackPauseDuration;
    
    [HideInInspector]
    public bool shooting = false;

    private Coroutine fleeing = null;

    #endregion


    #region monobehavior

    void FixedUpdate () {
		if(target != null && !attacking && !RunningThisRoutine(fleeing))
        {
            Vector2 vectorToTarget = Vector2.zero;
            RaycastHit2D[] hits = new RaycastHit2D[2];
            Physics2D.RaycastNonAlloc(transform.position, target.transform.position - transform.position, hits, aggroRange, collideLayerMask);
            string hitTag = null;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.fraction != 0)
                {
                    hitTag = hit.collider.tag;
                    vectorToTarget = hit.point - (Vector2)transform.position;
                }
            }

            // This checks if the raycast hit the target
            bool hitIsTarget = false;
            foreach (string tag in targetTags)
            {
                if (tag == hitTag)
                {
                    hitIsTarget = true;
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
                        AimWeaponAtTarget(vectorToTarget);
                    } else if (vectorToTarget.magnitude < tooCloseThreshold && hitIsTarget)
                    {
                        //fleeing = MoveToLocation(vectorToTarget.normalized * -1 * Mathf.Abs(followDistance - vectorToTarget.magnitude));
                        fleeing = MoveTo(vectorToTarget.normalized * -1 * Mathf.Abs(followDistance - vectorToTarget.magnitude));
                        AimWeaponAtTarget(-vectorToTarget);
                    } else if(Time.time - lastAttackTime > attackCooldownLength && vectorToTarget.magnitude < attackRange && hitIsTarget)
                    {
                        if (weapon.GetComponent<RangedWeapon>().CanFire())
                        {
                            StartCoroutine(ShootAttack(vectorToTarget, attackChargeTime, attackPauseDuration));
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
        AimWeaponAtTarget(vectorToTarget);
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
                StartCoroutine(weapon.GetComponent<RangedWeapon>().Shoot(vectorToTarget));
                while (shooting)
                {
                    yield return new WaitForSeconds(.05f);
                }
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
