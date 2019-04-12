using UnityEngine;

public class EquippedWeapon : Object
{
    #region fields and variables

    /* COMMON FIELDS */
    private readonly bool logical = true; // whether or not this object can do anything
    private readonly bool sendsEvents; // whether this object should send UI events (i.e. its a player weapon)
    public GameObject weapon;
    private readonly float minTimeBetweenAttacks; // minimum time that must pass after an attack before allowing another attack
    private readonly int layer; // layer for collisions
    private readonly float baseDamage; // base, unmodified damage
    public readonly MovementController character; // The character that owns this weapon
    public readonly int combatMode; // Combat mode to use for attacks

    /* GUN */
    public GameObject bullet;
    public Gun gunScript; // the script attached to the gun's gameobject
    private readonly float speed; // bullet speed
    private readonly float lifetime; // bullet lifetime
    private readonly float reloadTime; // duration of a reload
    private readonly int maxAmmo;
    public int MaxAmmo => maxAmmo; // Maximum ammo allowed in mag
    private int currentAmmo;
    public int CurrentAmmo { get => currentAmmo; private set => currentAmmo = value; } // Ammo currently in mag
    private Transform bulletSpawnLocation; // Where to spawn bullets

    /* CQC */
    public CQC swordScript; // the script attached to the CQC's gameobject

    /* FLAGS */
    public bool reloading = false;
    public bool swinging = false;
    public float reloadProgress;
    private float timeOfLastShot;

    #endregion

    #region events

    public delegate void WeaponUpdateHandler();
    public static event WeaponUpdateHandler WeaponAmmoChanged; // Called when the ammo numbers in this weapon change.

    public delegate void WeaponSwapHandler(EquippedWeapon wep, int combatMode);
    public static event WeaponSwapHandler WeaponSwapped; // Called when this EquippedObject is swapped to.

    public delegate void WeaponReloadStateHandler(bool reloading);
    public static event WeaponReloadStateHandler ReloadStateChanged; // Called when this weapon starts or stops reloading

    #endregion

    #region constructors

    public static readonly EquippedWeapon empty = new EquippedWeapon();

    /// <summary>
    /// Create a new empty <c>EquippedWeapon</c> that has no functionality.
    /// </summary>
    private EquippedWeapon()
    {
        logical = false;
    }

    /// <summary>
    /// Create a new <c>EquippedWeapon</c>.
    /// </summary>
    /// <param name="info">The data that describes this weapon.</param>
    /// <param name="parent">The <c>Transform</c> to instantiate this weapon under.</param>
    /// <param name="layer">The layer to be used for collision.</param>
    /// <param name="parentMoveController">The character who owns this weapon.</param>
    /// <param name="combatMode">The combatmode that this weapon will use. See <see cref="WeaponController"/> for values.</param>
    public EquippedWeapon(UWeaponInformation info, Transform parent, string layer, MovementController parentMoveController, int combatMode)
    {
        sendsEvents = parentMoveController.gameObject.CompareTag("Player");
        this.combatMode = combatMode;
        weapon = Instantiate(info.weapon, parent);
        character = parentMoveController;
        this.layer = LayerMask.NameToLayer(layer);
        if (combatMode == WeaponController.COMBATMODE_GUN)
        {
            gunScript = weapon.GetComponent<Gun>();
            gunScript.Initialize(this);
            bullet = info.bullet;
            minTimeBetweenAttacks = info.cycleTime;
            timeOfLastShot = Time.time - minTimeBetweenAttacks;
            bulletSpawnLocation = weapon.GetComponentInChildren<Transform>();
            speed = info.muzzleVelocity;
            lifetime = info.lifetime;
            baseDamage = info.bulletDamage;
            maxAmmo = info.clipSize;
            currentAmmo = maxAmmo;
            reloadTime = info.reloadTime;
        }
        else if (combatMode == WeaponController.COMBATMODE_MELEE)
        {
            swordScript = weapon.GetComponent<CQC>();
            swordScript.Initialize(this, character, this.layer);
            minTimeBetweenAttacks = info.exhaustTime;
            baseDamage = info.meleeDamage;
        }
        weapon.SetActive(false);
    }

    #endregion

    #region public

    /// <summary>
    /// Sets the weapon GameObject active or inactive and appropriately cleans up related operations.
    /// </summary>
    /// <param name="state">Enable or disable the weapon.</param>
    public void SetEnabled(bool state)
    {
        if (!logical) return;

        weapon.SetActive(state);
        if (combatMode == WeaponController.COMBATMODE_GUN)
            CancelReload();
        else if (combatMode == WeaponController.COMBATMODE_MELEE)
            CancelSwing();

        if (state && sendsEvents)
            WeaponSwapped(this, combatMode);
    }

    /// <summary>
    /// Returns if the weapon can attack.
    /// </summary>
    /// <returns>Whether the weapon can attack.</returns>
    public bool CanFire()
    {
        return Time.time - timeOfLastShot >= minTimeBetweenAttacks && !reloading && !swinging;
    }

    /// <summary>
    /// Fires the gun or swings the sword.
    /// </summary>
    /// <param name="direction">The direction to attack in.</param>
    public void Fire(Vector2 direction)
    {
        if (!CanFire()) return;
        if (combatMode == WeaponController.COMBATMODE_GUN)
        {
            if (currentAmmo <= 0)
            {
                Reload();
                // lets call an event or somehow access the enclosing class so we can access the ui and do updates
                // better yet call into a script attached to the gun gameobject which is abstracted to be fairly generalized
                return;
            }
            currentAmmo--;
            GameObject shot = Instantiate(bullet, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
            float angle = bulletSpawnLocation.rotation.eulerAngles.z * Mathf.Deg2Rad;
            shot.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
            BulletController bm = bullet.GetComponent<BulletController>();
            bm.gameObject.layer = layer;
            bm.lifeTime = lifetime;
            bm.damage = baseDamage;
            bm.source = character;
            gunScript.Fire(direction);
            timeOfLastShot = Time.time;
            if (sendsEvents)
                WeaponAmmoChanged();
        }
        else if (combatMode == WeaponController.COMBATMODE_MELEE)
        {
            // swing sword
            Swing();
        }
    }

    /// <summary>
    /// Returns if this weapon is currently firing.
    /// </summary>
    /// <returns>Whether the weapon is firing.</returns>
    public bool IsFiring()
    {
        return swinging;
    }

    /// <summary>
    /// Starts the sequence of attacking with a CQC weapon.
    /// </summary>
    public void Swing()
    {
        swinging = true;
        swordScript.Attack(baseDamage);
    }

    /// <summary>
    /// Cancels an attack early with a CQC weapon.
    /// </summary>
    public void CancelSwing()
    {
        swordScript.CancelAttack();
        swinging = false;
    }

    /// <summary>
    /// Successfully ends an attack with a CQC weapon.
    /// </summary>
    public void EndSwing()
    {
        swinging = false;
        timeOfLastShot = Time.time;
    }

    /// <summary>
    /// Cancel a reload early with a Gun weapon.
    /// </summary>
    public void CancelReload()
    {
        gunScript.CancelReload();
        reloading = false;
        ReloadStateChanged(false);
        reloadProgress = 0;
    }

    /// <summary>
    /// Returns if this weapon can currently reload.
    /// </summary>
    /// <returns>Whether the weapon can reload.</returns>
    public bool CanReload()
    {
        return maxAmmo != currentAmmo;
    }

    /// <summary>
    /// Starts the sequence of reloading with a Gun.
    /// </summary>
    public void Reload()
    {
        gunScript.Reload(reloadTime);
        reloading = true;
        ReloadStateChanged(true);
        reloadProgress = 0;
    }

    /// <summary>
    /// Ends the sequence of reloading with a Gun.
    /// </summary>
    public void FillMag()
    {
        currentAmmo = maxAmmo;
        reloading = false;
        ReloadStateChanged(false);
        reloadProgress = 1;
        if (sendsEvents)
        {
            WeaponAmmoChanged();
        }
    }
    #endregion

}
