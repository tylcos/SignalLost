using System.Collections;
using UnityEngine;



public class WeaponManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public Transform bulletSpawnPoint;

    public Weapon Weapon
    {
        get { return Weapons[CurrentWeapon]; }
    }

    [HideInInspector]
    public int CurrentWeapon = -1;
    public Weapon[] Weapons;

    public float WeaponSwitchTime = .2f;



    private float weaponPos;
    private bool reloading = false;
    private int weaponBeingReloaded;

    private float timeLastSwitched = 0f;



    void Start()
    {
        foreach (Weapon weapon in Weapons)
            weapon.ammo = weapon.clipSize;
    }



    void Update() 
	{
        weaponPos += Mathf.Abs(Input.GetAxis("ScrollWheel"));

        if (Input.GetKey(KeyCode.Tab) && Time.time - timeLastSwitched > WeaponSwitchTime)
        {
            ++weaponPos;
            timeLastSwitched = Time.time;
        }


        weaponPos %= Weapons.Length;
        if (CurrentWeapon != (int)weaponPos)
        {
            CurrentWeapon = (int)weaponPos;

            sr.sprite = Weapon.weaponSprite;
            bulletSpawnPoint.localPosition = Weapon.bulletSpawnOffset;
            reloading = false;



            if (Weapon.ammo == 0)
                Reload();
        }
    }



    public void Reload()
    {   
        if (!reloading)
        {
            weaponBeingReloaded = CurrentWeapon;
            StartCoroutine(ReloadInternal());
        }

        reloading = true;
    }

    private IEnumerator ReloadInternal()
    {
        yield return new WaitForSeconds(Weapon.reloadTime);

        if (CurrentWeapon == weaponBeingReloaded)
        {
            Weapon.ammo = Weapon.clipSize;
            reloading = false;
        }
    }
}
