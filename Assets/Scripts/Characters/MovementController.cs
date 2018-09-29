using System.Collections;
using UnityEngine;



public class MovementController : MonoBehaviour
{
    public float speed;

    public Rigidbody2D rb2d;

    public float CurrentHitPoints { get; set; }
    public float MaxHitPoints { get; private set; }



    [SerializeField]
    private float _MaxHitPoints;



    public virtual void OnEnable()
    {
        MaxHitPoints = _MaxHitPoints;
        CurrentHitPoints = _MaxHitPoints;
    }


    
    /// <summary>
     ///     Deals <c>damage</c> damage to the enemy's health.
     /// </summary>
     /// <returns>
     ///     Returns <c>true</c> if the enemy is killed by the damage, false otherwise.
     /// </returns>
     ///     <param name="damage">The quantity of damage to deal</param>
    public bool Damage(float damage)
    {
        CurrentHitPoints -= damage;
        if (CurrentHitPoints <= 0)
        {
            Die();
            return true;
        }
        else
        {
            return false;
        }
    }



    /// <summary>
    ///     Deals <c>damage</c> damage per second to the enemy's health over <c>duration</c> seconds.
    /// </summary>
    /// <returns>
    ///     Returns <c>true</c> if the enemy is killed by the damage dealt in the frame this function is called, false otherwise.
    /// </returns>
    ///     <param name="damage">The damage per second to deal</param>
    ///     <param name="duration">The duration over which to deal damage</param>
    public bool Damage(float damage, float duration)
    {
        StartCoroutine(DamageOverTime(damage, duration));
        return false;
    }



    /// <summary>
    ///      Deals <c>damage</c> damage to the enemy's health over <c>duration</c> seconds, then knocks them back along the given vector.
    /// </summary>
    /// <returns>
    ///     Returns <c>true</c> if the enemy is killed by the damage dealt in the frame this function is called, false otherwise.
    /// </returns>
    ///     <param name="damage">The damage per second to deal</param>
    ///     <param name="duration">The duration over which to deal damage</param>
    ///     <param name="knockbackVector">The vector to move the enemy along</param>
    public bool Damage(float damage, float duration, Vector2 knockbackVector)
    {
        Move(rb2d, transform.position, knockbackVector);
        return Damage(damage, duration);
    }



    /// <summary>
    ///      Deals <c>damage</c> damage to the enemy's health., then knocks them back along the given vector.
    /// </summary>
    /// <returns>
    ///     Returns <c>true</c> if the enemy is killed by the damage, false otherwise.
    /// </returns>
    ///     <param name="damage">The damage per second to deal</param>
    ///     <param name="knockbackVector">The vector to move the enemy along</param>
    public bool Damage(float damage, Vector2 knockbackVector)
    {
        Move(rb2d, transform.position, knockbackVector);
        return Damage(damage);
    }



    public IEnumerator DamageOverTime(float dps, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            float damageThisTick = dps / (1 / Time.deltaTime);
            Damage(damageThisTick);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }



    // !!!!!!!!!!!!!!!THIS NEEDS OPTIMIZATION!!!!!!!!!!!!!!!!!!!!
    /// <summary>
    ///     Moves the rigidbody to position with wall collision.
    /// </summary>
    /// <remark>
    ///    This simply uses the Rigidbody2D.MovePosition function to move the gameObject after calculating collision.
    /// </remark>
    ///     <param name="rb2d">The RigidBody2D object.</param>
    ///     <param name="origin">The origin of the movement vector.</param>
    ///     <param name="moveVector">The movement vector to use.</param>
    protected void Move(Rigidbody2D rb2d, Vector2 origin, Vector2 moveVector)
    {
        //rb2d.MovePosition(moveVector + origin);


        
        Vector2 bcSize = rb2d.gameObject.GetComponent<BoxCollider2D>().size;

        float vert = moveVector.y;
        float horz = moveVector.x;

        RaycastHit2D[] hitVert = new RaycastHit2D[2];
        int hitVertCount = Physics2D.RaycastNonAlloc(origin, new Vector2(0, moveVector.y), hitVert, moveVector.magnitude + bcSize.y, LayerMask.GetMask("Walls", "Player", "Enemy"));
        foreach (RaycastHit2D hit in hitVert) {
            if (hit.collider != null && hit.fraction != 0)
            {
                if (moveVector.y > 0)
                {
                    vert = hit.distance - bcSize.y;
                    if (vert < 0)
                    {
                        vert = 0;
                    }
                }
                else if (moveVector.y < 0)
                {
                    vert = -hit.distance + bcSize.y;
                    if (vert > 0)
                    {
                        vert = 0;
                    }
                }
                break;
            }
        }

        RaycastHit2D[] hitHorz = new RaycastHit2D[2];
        int hitHorzCount = Physics2D.RaycastNonAlloc(origin, new Vector2(moveVector.x, 0), hitHorz, moveVector.magnitude + bcSize.x, LayerMask.GetMask("Walls", "Player", "Enemy"));
        foreach (RaycastHit2D hit in hitHorz)
        {
            if (hit.collider != null && hit.fraction != 0)
            {
                if (moveVector.x > 0)
                {
                    horz = hit.distance - bcSize.x;
                    if (horz < 0)
                    {
                        horz = 0;
                    }
                }
                else if (moveVector.x < 0)
                {
                    horz = -hit.distance + bcSize.x;
                    if (horz > 0)
                    {
                        horz = 0;
                    }
                }
            }
        }

        Vector2 finalMoveVector = new Vector2(horz, vert);
        rb2d.MovePosition(finalMoveVector + origin);

        /*
        RaycastHit2D hit = Physics2D.Raycast(origin, moveVector, moveVector.magnitude, LayerMask.GetMask("Walls"));
        if(hit.collider != null)
        {
            RaycastHit2D hitBack = Physics2D.Raycast(hit.point, -moveVector, moveVector.magnitude, LayerMask.GetMask("Player"));
            Vector2 subtractVector = (moveVector.normalized * (hitBack.distance));
            rb2d.MovePosition(subtractVector + origin);
        } else
        {
            rb2d.MovePosition(moveVector + origin);
        }*/
    }



    protected virtual void Die() // Implement later for death animation / loot
    {
        Destroy(gameObject);
    }
}
