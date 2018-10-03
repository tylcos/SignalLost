using UnityEngine;

<<<<<<< HEAD
public class MeleeEnemy : EnemyController {
=======
>>>>>>> c21cdfe69d8a64523529934d70bda0651150d5cd

    public GameObject weapon;

public class MeleeEnemy : EnemyController
{
    void FixedUpdate()
    {
        if (Time.time - lastAttackTime < waitTime)
            return;

        if (target != null)
        {
            Vector2 vectorToTarget = Vector2.zero;
            RaycastHit2D[] hits = new RaycastHit2D[2];
            Physics2D.RaycastNonAlloc(transform.position, target.transform.position - transform.position, hits, aggroRange, collideLayerMask);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.fraction != 0)
                {
                    vectorToTarget = hit.point - (Vector2)transform.position;
                }
            }

            if (!(vectorToTarget == Vector2.zero))
            {
                if (vectorToTarget.magnitude > attackRange)
                {
                    Move(rb2d, transform.position, vectorToTarget.normalized * speed);
                }
                else if (Time.time - lastAttackTime > attackCooldownLength)
                {
                    //OverlapAttack(vectorToTarget);
                    SwordAttack(vectorToTarget);
                    OverlapAttack(vectorToTarget);
                }
            }
        }
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

    private void SwordAttack(Vector2 vectorToTarget)
    {
        Quaternion look = Quaternion.LookRotation(vectorToTarget, Vector3.up); ;
        weapon.transform.rotation = look;
    }
}
