using UnityEngine;



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
                    Collider2D[] overlap = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayerMask);
                    foreach (Collider2D col in overlap)
                    {
                        col.gameObject.GetComponent<MovementController>().Damage(damage);
                        lastAttackTime = Time.time;
                    }
                    if(lastAttackTime == Time.time)
                    {
                        rb2d.MovePosition(-vectorToTarget.normalized * retreatDistance);
                    }
                }
            }
        }
    }
}
