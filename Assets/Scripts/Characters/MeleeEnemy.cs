using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController {

    public float attackChargeTime;
    public float attackColliderEnableLength;
    public float attackAfterPauseDuration;
    public float attackAfterRetreatDistance;

    void FixedUpdate()
    {
        if(Time.time - lastAttackTime <= waitTime) { return; } // cancel if this enemy just attacked

        if (target != null && !attacking) // if the enemy is not attacking, do movement and try to attack
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
            foreach(string tag in targetTags)
            {
                if(tag == hitTag)
                {
                    hitIsTarget = true;
                }
            }

            if (!(vectorToTarget == Vector2.zero))
            {
                if (vectorToTarget.magnitude < aggroRange) // if within aggro range, move and aim weapon towards target
                {
                    Move(rb2d, transform.position, vectorToTarget.normalized * speed);
                    AimWeaponAtTarget(vectorToTarget);
                }
                if (Time.time - lastAttackTime > attackCooldownLength && vectorToTarget.magnitude < attackRange && hitIsTarget) // attack
                {
                    StartCoroutine(SwordAttack(vectorToTarget, attackChargeTime, attackColliderEnableLength, attackAfterPauseDuration, attackAfterRetreatDistance));
                }
            }
        }
    }


    public override void OnHitOpponent(GameObject entityHit)
    {
        if(Time.time - lastAttackTime > waitTime)
        {
            base.OnHitOpponent(entityHit);
            lastAttackTime = Time.time;
        }
        // do anything specific to the melee enemy here
    }

    private void OverlapAttack(Vector2 vectorToTarget)
    {
        Collider2D[] overlap = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayerMask);
        foreach (Collider2D col in overlap)
        {
            col.gameObject.GetComponent<MovementController>().Damage(damage);
            lastAttackTime = Time.time;
        }
        if (lastAttackTime == Time.time)
        {
            rb2d.MovePosition(-vectorToTarget.normalized * retreatDistance);
        }
    }

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
        while(Time.time < startTime + chargeTime + pauseTime + enableLength + retreatLength)
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
            } else if(Time.time < startTime+chargeTime+enableLength+pauseTime+retreatLength) // RETREATING (jumping backwards)
            {
                transform.position = Vector3.Lerp(startingPosition, -vectorToTarget.normalized * retreatLength, Time.deltaTime / retreatLength);
                // do animations here
                yield return new WaitForFixedUpdate();
            }
        }
        attacking = false;
    }

    
}
