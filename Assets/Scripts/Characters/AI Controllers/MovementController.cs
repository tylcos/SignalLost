using System.Collections;
using UnityEngine;



public class MovementController : MonoBehaviour
{
    public float speed; // in units per second
    public Rigidbody2D rb2d; // attached rigidbody
    
    // This is used for the inspector until we have compact data storage
    [SerializeField]
    private float _MaxHitPoints;
    public float CurrentHitPoints { get; set; }
    public float MaxHitPoints { get; private set; }
    
    public float invincibilityDuration; // length of invincibility frames
    private float lastDamageTime; // last time took damage

    protected bool moving = false; // is this character executing a coroutine move function?
    private Coroutine activeCoroutine; // the active coroutine if there is one


    private void Awake()
    {
        lastDamageTime = invincibilityDuration;
        MaxHitPoints = _MaxHitPoints;
        CurrentHitPoints = _MaxHitPoints;
    }


    /// <summary>
    ///     Checks if the character is currently invincible.
    /// </summary>
    /// <returns>
    ///     Returns <c>true</c> if the character is invincible.
    /// </returns>
    public bool IsInvincible()
    {
        return Time.time - lastDamageTime < invincibilityDuration;
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
        if(IsInvincible()) { return false; }
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
        if (IsInvincible()) { return false; }
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
        if (IsInvincible()) { return false; }
        CancelMovement();
        MoveTowards(knockbackVector, knockbackVector.magnitude);
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
        if (IsInvincible()) { return false; }
        CancelMovement();
        MoveTowards(knockbackVector, knockbackVector.magnitude);
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



    /// <summary>
    ///      Cancels current movement allowing more movement functions to be called.
    /// </summary>
    /// <returns>
    ///     Returns <c>false</c> if this object wasn't moving, <c>true</c> otherwise.
    /// </returns>
    protected bool CancelMovement()
    {
        if(moving)
        {
            moving = false;
            activeCoroutine = null;
            StopCoroutine(activeCoroutine);
            return true;
        } else
        {
            return false;
        }
    }

    protected bool RunningThisRoutine(Coroutine c)
    {
        return activeCoroutine == c && activeCoroutine != null;
    }

    /* TO DO!!!!!!!!!
     * PRVENT MOVING EACH OTHER BY CHECKING RAYCASTS
     * IF THE RAYCAST HITS SOMETHING, ACCOUNT FOR THAT
     * THERE MIGHT STILL BE COLLISION THOUGH, HOW DO I FIX THAT?
     * 
     * */

    /// <summary>
    ///      Moves from the character's current position to the specified coordinates at the character's speed.
    /// </summary>
    ///     <param name="destination">The location to move to</param>
    protected Coroutine MoveToLocation(Vector2 destination)
    {
        if (moving) return null;
        Vector2 source = transform.position;
        float duration = Mathf.Pow(speed / Mathf.Abs(Vector2.Distance(source, destination)), -1);
        activeCoroutine = StartCoroutine(MoveFromAToB(source, source + destination, duration));
        return activeCoroutine;
    }

    // potential bug that if it hits a collider it will just ram into it because it cannot detect if its blocked
    private IEnumerator MoveFromAToB(Vector2 a, Vector2 b, float duration) // thanks stackexchange
    {
        moving = true;
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            rb2d.MovePosition(Vector2.Lerp(a, b, t)); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        rb2d.MovePosition(b);
        moving = false;
        activeCoroutine = null;
    }


    /// <summary>
    ///      Moves the character in the direction of the specified vector at the character's speed.
    /// </summary>
    ///     <param name="direction">The direction to move in.</param>
    protected void MoveTowards(Vector2 direction)
    {
        if (moving) return;
        rb2d.MovePosition((Vector2)transform.position + (direction.normalized * (speed * Time.fixedDeltaTime)));
    }


    /// <summary>
    ///      Moves the character in the direction of the specified vector at the character's speed.
    /// </summary>
    ///     <param name="direction">The direction to move in.</param>
    ///     <param name="speed">Overrides movement speed for a new speed</param>
    protected void MoveTowards(Vector2 direction, float speed)
    {
        if (moving) return;
        rb2d.MovePosition((Vector2)transform.position + (direction.normalized * (speed * Time.fixedDeltaTime)));
    }

    // This model of the function moves it a given distance. This is jank and would probably never be needed, but here it is just in case.
    /*protected void MoveTowards(Vector2 direction, float distance)
    {
        if (moving) return;
        rb2d.MovePosition((Vector2) transform.position + (direction.normalized * distance));
    }*/



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
    [System.Obsolete("Move is deprecated. Use MoveTowards instead.")]
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
