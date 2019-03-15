using UnityEngine;

public class EquippedWeapon : Object
{
    public static readonly EquippedWeapon empty = new EquippedWeapon();
    public GameObject weapon;
    public GameObject bullet;
    public Gun gunScript;
    public Sword swordScript;
    private readonly bool logical = true;
    private readonly float minTimeBetweenAttacks;
    private readonly float speed;
    private readonly float lifetime;
    private readonly int layer;
    private readonly float baseDamage;
    private readonly int maxAmmo;
    private readonly float reloadTime;
    private int currentAmmo;
    private float timeOfLastShot;
    private Transform bulletSpawnLocation;
    public bool reloading = false;
    public bool swinging = false;
    public float reloadProgress;
    public readonly MovementController character;
    public readonly int combatMode;

    public int CurrentAmmo { get => currentAmmo; private set => currentAmmo = value; }

    public int MaxAmmo => maxAmmo;

    public delegate void WeaponUpdateHandler();
    public static event WeaponUpdateHandler WeaponAmmoChanged;

    public delegate void WeaponSwapHandler(EquippedWeapon wep, int combatMode);
    public static event WeaponSwapHandler WeaponSwapped;

    private EquippedWeapon()
    {
        logical = false;
    }

    public EquippedWeapon(WeaponV2Information info, Transform parent, string layer, MovementController parentMoveController, int combatMode)
    {
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
            swordScript = weapon.GetComponent<Sword>();
            swordScript.Initialize(this, this.layer, parentMoveController);
            minTimeBetweenAttacks = info.exhaustTime;
            baseDamage = info.meleeDamage;
        }
        weapon.SetActive(false);
    }

    public void SetEnabled(bool state)
    {
        if (!logical) return;
        weapon.SetActive(state);
        if (combatMode == WeaponController.COMBATMODE_GUN)
            CancelReload();
        WeaponSwapped(this, combatMode);
    }

    public bool CanFire()
    {
        return Time.time - timeOfLastShot >= minTimeBetweenAttacks && !reloading && !swinging;
    }

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
            timeOfLastShot = Time.time;
            WeaponAmmoChanged();
        }
        else if (combatMode == WeaponController.COMBATMODE_MELEE)
        {
            // swing sword
            swinging = true;
            swordScript.Attack(baseDamage);
        }
    }

    public void EndSwing()
    {
        swinging = false;
        timeOfLastShot = Time.time;
    }

    public void CancelReload()
    {
        gunScript.CancelReload();
        reloading = false;
        reloadProgress = 0;
    }

    public bool CanReload()
    {
        return maxAmmo != currentAmmo;
    }

    public void Reload()
    {
        gunScript.Reload(reloadTime);
        reloading = true;
        reloadProgress = 0;
    }

    public void FillMag()
    {
        currentAmmo = maxAmmo;
        reloading = false;
        reloadProgress = 1;
        WeaponAmmoChanged();
    }
}
