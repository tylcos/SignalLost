using Prime31;
using System.Collections;
using UnityEngine;



public class MovementController : MonoBehaviour
{

    #region fields and events

    [Tooltip("This character's speed in units/second.")]
    public float speed; // in units per second
    public CharacterController2D cc2d; // attached charactercontroller2d
    protected LayerMask collideLayerMask; // the layermask used for collision
    [SerializeField]
    [Tooltip("Default maximum health.")]
    private float _MaxHitPoints = 1;
    public float CurrentHitPoints { get; set; }
    public float MaxHitPoints { get; private set; }
    [Tooltip("Length of invincibility frames.")]
    public float invincibilityDuration; // length of invincibility frames
    private float lastDamageTime; // last time took damage

    public bool movingForAnimation = false;
    private bool moving = false; // is this character executing a coroutine move function?
    private Coroutine activeCoroutine; // the active coroutine if there is one

    public delegate void DamageEventHandler(float amount);
    public event DamageEventHandler DamageTaken; // for when damage is taken
    public delegate void DeathEventHandler();
    public event DeathEventHandler Died; // for when this character dies

    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public int animationDirection = 1;
    private Vector2 spriteDirection = new Vector2(0.0f, 0.0f); // for checking sprite direction 
    public Animator spriteAnimator;


    #endregion


    #region Monobehaviors

    protected virtual void Awake()
    {
        DamageTaken += OnTakeDamage;
        Died += OnDeath;
    }

    protected virtual void OnEnable()
    {
        lastDamageTime = invincibilityDuration;
        collideLayerMask = cc2d.platformMask;
        MaxHitPoints = _MaxHitPoints;
        CurrentHitPoints = _MaxHitPoints;
    }



    protected virtual void Update()
    {
        if (spriteAnimator != null) // Checks if there is an animator attached
        {
            if (spriteDirection.y > 0.0f) // Checks if the sprite is moving north
            {
                animationDirection = 1;
            }
            else // Checks if sprite is moving South
            {
                animationDirection = 3;
            }
            if (spriteDirection.x < 0.0f) // Checks if the sprite is moving West
            {
                animationDirection = 4;
            }
            if (spriteDirection.x > 0.0f) // Checks if the sprite is moving East
            {
                animationDirection = 3;
            }
        }
    }

    protected virtual void LateUpdate()
    {
        // Debug.Log(movingForAnimation);
        //  movingForAnimation = false;
    }

    #endregion


    // Implement later for death animation / loot
    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    protected virtual void OnTakeDamage(float damageReceived)
    {
        CurrentHitPoints -= damageReceived;
        if (CurrentHitPoints <= 0)
        {
            Died();
        }
    }


    #region damage functions

    /// <summary>
    ///     Deals <c>damage</c> damage to the enemy's health.
    /// </summary>
    /// <returns>
    ///     Returns <c>true</c> if the enemy should be killed by the damage, false otherwise.
    /// </returns>
    ///     <param name="damage">The quantity of damage to deal</param>
    public bool Damage(float damage)
    {
        if (IsInvincible()) { return false; }
        lastDamageTime = Time.time;
        DamageTaken(damage);
        if (MaxHitPoints - CurrentHitPoints <= 0)
        {
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
        Move(knockbackVector, knockbackVector.magnitude);
        return Damage(damage);
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
        Move(knockbackVector, knockbackVector.magnitude);
        return Damage(damage, duration);
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

    #endregion


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
    ///      Checks if the specified coroutine is currently being run by this controller.
    /// </summary>
    /// <returns>
    ///     Returns whether or not it is running.
    /// </returns>
    protected bool RunningThisRoutine(Coroutine c)
    {
        return activeCoroutine == c && activeCoroutine != null;
    }

    public virtual void OnHitDealt(MovementController opponent, bool killedOpponent)
    {
        
    }

    public virtual bool OnHitReceived(MovementController opponent, float damageReceived)
    {
        return Damage(damageReceived);
    }


    #region movement functions

    /// <summary>
    ///      Cancels current movement allowing more movement functions to be called.
    /// </summary>
    /// <returns>
    ///     Returns <c>false</c> if this object wasn't moving, <c>true</c> otherwise.
    /// </returns>
    protected bool CancelMovement()
    {
        if (moving)
        {
            StopCoroutine(activeCoroutine);
            moving = false;
            movingForAnimation = false;
            activeCoroutine = null;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    ///      Moves the character in the direction of the specified vector at the character's speed.
    /// </summary>
    ///     <param name="direction">The direction to move in.</param>
    protected void Move(Vector2 direction)
    {
        movingForAnimation = true;
        if (moving) return;
        cc2d.move(direction.normalized * (speed * Time.fixedDeltaTime));
        spriteDirection = direction;
    }

    /// <summary>
    ///      Moves the character in the direction of the specified vector at the character's speed.
    /// </summary>
    ///     <param name="direction">The direction to move in.</param>
    ///     <param name="speed">Overrides movement speed for a new speed</param>
    protected void Move(Vector2 direction, float speed)
    {
        movingForAnimation = true;
        if (moving) return;
        cc2d.move(direction.normalized * (speed * Time.fixedDeltaTime));
    }

    /// <summary>
    ///      Moves from the character's current position to the specified coordinates at the character's speed.
    /// </summary>
    ///     <param name="destination">The location to move to</param>
    protected Coroutine MoveTo(Vector2 destination)
    {
        movingForAnimation = true;
        if (moving) return null;
        Vector2 source = transform.position;
        Vector3 destination3 = destination;
        spriteDirection = transform.position - destination3;
        float duration = Mathf.Pow(speed / Mathf.Abs(Vector2.Distance(source, destination)), -1);
        activeCoroutine = StartCoroutine(MoveToTarget(destination, duration));
        return activeCoroutine;
    }

    /// <summary>
    ///      Moves from the character's current position to the specified coordinates at the specified speed.
    /// </summary>
    ///     <param name="destination">The location to move to</param>
    ///     <param name="speed">The speed to move at</param>
    protected Coroutine MoveTo(Vector2 destination, float speed)
    {
        movingForAnimation = true;
        if (moving) return null;
        Vector2 source = transform.position;
        float duration = Mathf.Pow(speed / Mathf.Abs(Vector2.Distance(source, destination)), -1);
        activeCoroutine = StartCoroutine(MoveToTarget(destination, duration, speed));
        return activeCoroutine;
    }

    // potential bug that if it hits a collider it will just ram into it because it cannot detect if its blocked
    // UPDATE THIS SO IT MOVES ONLY AS FAR AS IT GOT IN THIS CURRENT ITERATION
    private IEnumerator MoveToTarget(Vector2 b, float duration) // thanks stackexchange
    {
        Vector2 a = Vector2.zero;
        moving = true;
        movingForAnimation = true;
        float step = (speed / b.magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            //rb2d.MovePosition(Vector2.Lerp(a, b, t)); // Move objectToMove closer to b
            cc2d.move(b * step);
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        //rb2d.MovePosition(b);
        moving = false;
        movingForAnimation = false;
        activeCoroutine = null;
    }

    private IEnumerator MoveToTarget(Vector2 b, float duration, float speed) // thanks stackexchange
    {
        Vector2 a = Vector2.zero;
        moving = true;
        movingForAnimation = true;
        float step = (speed / b.magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            //rb2d.MovePosition(Vector2.Lerp(a, b, t)); // Move objectToMove closer to b
            cc2d.move(b * step);
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        //rb2d.MovePosition(b);
        moving = false;
        movingForAnimation = false;
        activeCoroutine = null;
    }
    #endregion
}
