using UnityEngine;
using System.Collections;


public class PlayerController : MovementController
{
    private GameManager master;
    private PlayerWeaponController PWC;
    [SerializeField]
    private GameObject reloadFailIndicator = null;
    private const float indicatorOnLength = 1f;
    private Coroutine coroutine = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        master = GameObject.FindGameObjectWithTag("Master").GetComponent<GameManager>();
        PWC = gameObject.GetComponentInChildren<PlayerWeaponController>();
        PWC.FireError += OnFireError;
    }

    private void OnFireError()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ReloadIndicatorThingy());

        // start a coroutine to turn on the indicator for a few seconds
        // if this is called again during this time, refresh the coroutine
    }

    private IEnumerator ReloadIndicatorThingy()
    {
        reloadFailIndicator.SetActive(true);
        float time = Time.time;
        while(Time.time - time < indicatorOnLength)
        {
            yield return new WaitForEndOfFrame();
        }
        reloadFailIndicator.SetActive(false);
        coroutine = null;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 move = GameManager.GetMovementVector();
        if (move.sqrMagnitude == 0)
            return;

        Move(move.normalized);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyWeapon")
        {
            collision.GetComponentInParent<EnemyController>().OnHitOpponent(gameObject);
        } else if(collision.tag == "Projectile")
        {
            collision.GetComponentInParent<BulletManager>().sourceEnemy.OnHitOpponent(gameObject);
        }
    }
}
