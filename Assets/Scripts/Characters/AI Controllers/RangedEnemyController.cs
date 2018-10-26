using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : EnemyController {

    public float followDistance;
    public float tooCloseThreshold;
    public float tooFarThreshold;

    public float attackChargeTime;
    public float attackPauseDuration;

    private bool runningAway = false;
    
	void FixedUpdate () {
		if(target != null && !attacking && !runningAway)
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
                        Move(rb2d, transform.position, vectorToTarget.normalized * speed);
                        AimWeaponAtTarget(vectorToTarget);
                    } else if (vectorToTarget.magnitude < tooCloseThreshold && hitIsTarget)
                    {
                        StartCoroutine(RunAway(-vectorToTarget.normalized * (followDistance - vectorToTarget.magnitude)));
                        AimWeaponAtTarget(-vectorToTarget);
                    } else if(Time.time - lastAttackTime > attackCooldownLength && vectorToTarget.magnitude < attackRange && hitIsTarget)
                    {
                        StartCoroutine(ShootAttack(vectorToTarget, attackChargeTime, attackPauseDuration));
                    }
                }
            }
        }
	}

    private IEnumerator ShootAttack(Vector2 vectorToTarget, float chargeTime, float pauseTime)
    {
        attacking = true;
        AimWeaponAtTarget(vectorToTarget);
        float startTime = Time.time;
        while (Time.time < startTime + chargeTime + pauseTime)
        {
            Vector3 startingPosition = transform.position;
            if (Time.time < startTime + chargeTime) // CHARGING ATTACK
            {
                attackIndicator.SetActive(true);
                // do animations here
                yield return new WaitForSeconds(.05f);
            }
            else if (Time.time >= startTime + chargeTime) // ATTACKING
            {
                attackIndicator.SetActive(false);
                // shoot gun
                // call an IEnumerator that makes a flag that is true while running, then wait for that to complete (just yield while its true)
                // then keep on completing this IEnumerator
                // do animations here
                yield return new WaitForSeconds(.05f);
            }
        }
        attacking = false;
    }

    private IEnumerator RunAway(Vector2 vectorToDestination)
    {
        runningAway = true;
        Vector3 startingPosition = transform.position;
        float timeToRun = vectorToDestination.magnitude / speed;
        float startTime = Time.time;
        do
        {
            transform.position = Vector3.Lerp(startingPosition, vectorToDestination, Time.deltaTime / timeToRun);
            yield return new WaitForFixedUpdate();
        } while (Time.time < startTime + timeToRun);
        runningAway = false;
    }
}
