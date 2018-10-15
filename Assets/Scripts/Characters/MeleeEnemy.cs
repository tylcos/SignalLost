using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController {

    public GameObject weaponHolder;
    public GameObject weapon;
    public float attackChargeTime;
    public float attackColliderEnableLength;
    public float attackAfterPauseDuration;
    public float attackAfterRetreatDistance;

    void FixedUpdate()
    {
        if(Time.time - lastAttackTime <= waitTime) { return; }
        if (target != null && !attacking)
        {
            Vector2 vectorToTarget = Vector2.zero;
            RaycastHit2D[] hits = new RaycastHit2D[2];
            Physics2D.RaycastNonAlloc(transform.position, target.transform.position - transform.position, hits, aggroRange, collideLayerMask);
            string blah = null;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.fraction != 0)
                {
                    blah = hit.collider.tag;
                    vectorToTarget = hit.point - (Vector2)transform.position;
                }
            }

            if (!(vectorToTarget == Vector2.zero) && blah == "Player")
            {
                if (vectorToTarget.magnitude < aggroRange)
                {
                    Move(rb2d, transform.position, vectorToTarget.normalized * speed);
                    SwordAim(vectorToTarget);
                }
                if (Time.time - lastAttackTime > attackCooldownLength && vectorToTarget.magnitude < attackRange)
                {
                    //OverlapAttack(vectorToTarget);
                    attacking = true;
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

    private IEnumerator SwordAttack(Vector2 vectorToTarget, float chargeTime, float enableLength, float pauseTime, float retreatLength)
    {
        SwordAim(vectorToTarget);
        float startTime = Time.time;
        // maybe keep flag booleans here in case of heavy lag so that the enemy still does EVEYTHING in this function
        while(Time.time < startTime + chargeTime + pauseTime + enableLength + retreatLength)
        {
            //print("Start:" + startTime + " now: " + Time.time);
            Vector3 startingPosition = transform.position;
            if(Time.time < startTime + chargeTime)
            {
                attackIndicator.SetActive(true);
                yield return new WaitForSeconds(.05f);
            } else if(Time.time < startTime + chargeTime + enableLength)
            {
                attackIndicator.SetActive(false);
                weapon.GetComponent<Collider2D>().enabled = true;
                yield return new WaitForSeconds(.05f);
            } else if (Time.time < startTime + chargeTime + enableLength + pauseTime)
            {
                weapon.GetComponent<Collider2D>().enabled = false;
                yield return new WaitForSeconds(.05f);
            } else if(Time.time < startTime+chargeTime+enableLength+pauseTime+retreatLength)
            {
                transform.position = Vector3.Lerp(startingPosition, -vectorToTarget.normalized * retreatLength, Time.deltaTime / retreatLength);
                yield return new WaitForFixedUpdate();
            }
        }
        attacking = false;
        //rb2d.MovePosition(-vectorToTarget.normalized * retreatDistance);
        // turn on collider, check for collision, turn off collider
    }

    private void SwordAim(Vector2 vectorToTarget)
    {
        var xd = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        xd.z = xd.x;
        xd.x = 0;
        xd.y = 0;
        weaponHolder.transform.rotation = xd;
        float angleDifference = (Mathf.Rad2Deg * Mathf.Atan2(vectorToTarget.y, vectorToTarget.x)) - weaponHolder.transform.eulerAngles.z;

        weaponHolder.transform.RotateAround(weaponHolder.transform.position, Vector3.forward, 180 + angleDifference);
    }

    
}
