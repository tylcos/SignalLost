using UnityEngine;
using System.Collections;


public class PlayerController : MovementController
{
    private PlayerWeaponController PWC;
    [SerializeField]
    private GameObject reloadFailIndicator = null;
    private const float indicatorOnLength = 1f;
    private Coroutine coroutine = null;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PWC = gameObject.GetComponentInChildren<PlayerWeaponController>();
        PWC.OnEnable();
        PWC.FireError += OnFireError;
        EquippedWeapon.ReloadStateChanged += OnReloadStateChanged;
    }

    private void OnDestroy()
    {
        EquippedWeapon.ReloadStateChanged -= OnReloadStateChanged;
    }

    private void OnReloadStateChanged(bool reloading)
    {
        if(!reloading & coroutine != null)
        {
            StopCoroutine(coroutine);
            reloadFailIndicator.SetActive(false);
        }
    }

    private void OnFireError()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            //reloadFailIndicator.SetActive(false);
        }
        coroutine = StartCoroutine(ReloadIndicatorThingy());

        // start a coroutine to turn on the indicator for a few seconds
        // if this is called again during this time, refresh the coroutine
    }

    private IEnumerator ReloadIndicatorThingy()
    {
        reloadFailIndicator.SetActive(true);
        float time = Time.time;
        while (Time.time - time < indicatorOnLength)
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
        Vector2 move = DungeonGameManager.GetMovementVector();
        if (move.sqrMagnitude == 0)
            return;

        Move(move.normalized);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if(collision.tag == "EnemyWeapon")
        {
            collision.GetComponentInParent<EnemyController>().OnHitOpponent(this);
        } else if(collision.tag == "Projectile")
        {
            //collision.GetComponentInParent<BulletController>().source.OnHitOpponent(this);
        }*/
    }

    public override void OnHitDealt(MovementController opponent, bool killedOpponent)
    {
        base.OnHitDealt(opponent, killedOpponent);
        //print("player hit an enemy and killed?:" + killedOpponent);
        if (killedOpponent)
            DungeonGameManager.AddScore(500);
    }

    public override bool OnHitReceived(MovementController opponent, float damageReceived)
    {
        bool targetKilled = base.OnHitReceived(opponent, damageReceived);
        //print("player hit by an enemy and took " + damageReceived + " damage");
        return targetKilled;
    }


}
