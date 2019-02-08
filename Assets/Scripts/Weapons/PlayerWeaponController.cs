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
        private float timeOfLastShot;
        private Transform bulletSpawnLocation;

        private EquippedWeapon()
        {
            logical = false;
        }

        public EquippedWeapon(WeaponInformation info, Transform parent)
        {
            gun = Instantiate(info.weapon, parent);
            bullet = info.bullet;
            minTimeBetweenShots = info.cycleTime;
            bulletSpawnLocation = gun.GetComponentInChildren<Transform>();
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
            GameObject shot = Instantiate(bullet, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = bulletSpawnLocation.eulerAngles * 4;
            BulletManager bm = bullet.GetComponent<BulletManager>();
            bm.lifeTime = 10;
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
                swapList.Add(new EquippedWeapon(wep, transform));
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
