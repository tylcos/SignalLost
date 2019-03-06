using UnityEngine;

public class EquippedWeapon : Object
{
    public static readonly EquippedWeapon empty = new EquippedWeapon();
    public GameObject gun;
    public GameObject bullet;
    public Gun gunScript;
    private readonly bool logical = true;
    private readonly float minTimeBetweenShots;
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
    public float reloadProgress;
    public readonly MovementController character;

    public int CurrentAmmo { get => currentAmmo; private set => currentAmmo = value; }

    public int MaxAmmo => maxAmmo;

    public delegate void WeaponUpdateHandler();
    public static event WeaponUpdateHandler WeaponAmmoChanged;

    public delegate void WeaponSwapHandler(EquippedWeapon wep);
    public static event WeaponSwapHandler WeaponSwapped;

    private EquippedWeapon()
    {
        logical = false;
    }

    public EquippedWeapon(WeaponV2Information info, Transform parent, string layer, MovementController parentMoveController)
    {
        gun = Instantiate(info.weapon, parent);
        gunScript = gun.GetComponent<Gun>();
        gunScript.Initialize(this);
        bullet = info.bullet;
        minTimeBetweenShots = info.cycleTime;
        timeOfLastShot = Time.time - minTimeBetweenShots;
        bulletSpawnLocation = gun.GetComponentInChildren<Transform>();
        speed = info.muzzleVelocity;
        lifetime = info.lifetime;
        this.layer = LayerMask.NameToLayer(layer);
        baseDamage = info.damage;
        maxAmmo = info.clipSize;
        currentAmmo = maxAmmo;
        reloadTime = info.reloadTime;
        gun.SetActive(false);
        character = parentMoveController;
    }

    public void SetEnabled(bool state)
    {
        if (!logical) return;
        gun.SetActive(state);
        CancelReload();
        WeaponSwapped(this);
    }

    public bool CanFire()
    {
        return Time.time - timeOfLastShot >= minTimeBetweenShots && !reloading;
    }

    public void Fire(Vector2 direction)
    {
        if (!CanFire()) return;
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
