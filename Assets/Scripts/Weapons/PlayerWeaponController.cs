using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : WeaponController
{
    private const int INVSIZE = 4;
    [SerializeField]
    public WeaponInformation[] inventory = new WeaponInformation[INVSIZE];
    private GameController master;
    private List<EquippedWeapon> swapList = new List<EquippedWeapon>(INVSIZE);
    private int swapListIndex = 0;

    private void OnValidate()
    {
        if(inventory.Length != INVSIZE)
        {
            Debug.LogWarning("Don't change the weapon inventory array size!");
            System.Array.Resize(ref inventory, INVSIZE);
        }
    }

    public class EquippedWeapon
    {
        public static readonly EquippedWeapon empty = new EquippedWeapon();
        public GameObject gun;
        public GameObject bullet;
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

        private EquippedWeapon()
        {
            logical = false;
        }

        public EquippedWeapon(WeaponInformation info, Transform parent, string layer)
        {
            gun = Instantiate(info.weapon, parent);
            bullet = info.bullet;
            minTimeBetweenShots = info.cycleTime;
            bulletSpawnLocation = gun.GetComponentInChildren<Transform>();
            speed = info.muzzleVelocity;
            lifetime = info.lifetime;
            this.layer = LayerMask.NameToLayer(layer);
            baseDamage = info.damage;
            maxAmmo = info.clipSize;
            currentAmmo = maxAmmo;
            reloadTime = info.reloadTime;
            gun.SetActive(false);
        }

        public void SetEnabled(bool state)
        {
            if (!logical) return;
            if(state)
            {
                gun.SetActive(true);
            }
            else
            {
                gun.SetActive(false);
            }
        }

        public bool CanFire()
        {
            return Time.time - timeOfLastShot >= minTimeBetweenShots;
        }

        public void Fire(Vector2 direction)
        {
            if (Time.time - timeOfLastShot < minTimeBetweenShots) return;
            if (currentAmmo <= 0)
            {
                //Reload();
                // lets call an event or somehow access the enclosing class so we can access the ui and do updates
                // better yet call into a script attached to the gun gameobject which is abstracted to be fairly generalized
                return;
            }
            currentAmmo--;
            GameObject shot = Instantiate(bullet, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
            float angle = bulletSpawnLocation.rotation.eulerAngles.z * Mathf.Deg2Rad;
            shot.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
            BulletManager bm = bullet.GetComponent<BulletManager>();
            bm.gameObject.layer = layer;
            bm.lifeTime = lifetime;
            bm.damage = baseDamage;
            timeOfLastShot = Time.time;
        }

        public void Reload()
        {
            currentAmmo = maxAmmo;
        }
    }

    private void OnEnable()
    {
        master = GameObject.FindGameObjectWithTag("Master").GetComponent<GameController>();
        // populates the swaplist
        foreach (WeaponInformation wep in inventory)
        {
            // create a class that stores a reference to the object we 
            if(wep == null)
                swapList.Add(EquippedWeapon.empty);
            else
                swapList.Add(new EquippedWeapon(wep, transform, bulletLayer));
            // so we have a parallel array that holds the objects, their bullets, and ammo
            // so we call commands there when we need to fire or whatnot since that stores everything
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        swapList[0].gun.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // To swap weapons on the arcade, pause the sim and pull up a radial selector and use joysticks to select
        if (Input.GetKeyDown(KeyCode.Alpha1) && swapListIndex != 0)
        {
            swapList[swapListIndex].SetEnabled(false);
            swapListIndex = 0;
            swapList[swapListIndex].SetEnabled(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && swapListIndex != 1)
        {
            swapList[swapListIndex].SetEnabled(false);
            swapListIndex = 1;
            swapList[swapListIndex].SetEnabled(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && swapListIndex != 2)
        {
            swapList[swapListIndex].SetEnabled(false);
            swapListIndex = 2;
            swapList[swapListIndex].SetEnabled(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && swapListIndex != 3)
        {
            swapList[swapListIndex].SetEnabled(false);
            swapListIndex = 3;
            swapList[swapListIndex].SetEnabled(true);
        }
        else
        {
            Vector2 shootDir = Vector2.zero;
            if (master.inputMethod == "keyboard")
            {
                shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));
            }
            else if (master.inputMethod == "arcade")
            {
                shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeysArcade"), Input.GetAxisRaw("VerticalKeysArcade"));
            }

            if ((Input.GetAxis("Fire1") > 0 || shootDir.sqrMagnitude != 0) && swapList[swapListIndex].CanFire())
            {
                print("shooting");
                swapList[swapListIndex].Fire(shootDir);
            }
        }
    }
}
