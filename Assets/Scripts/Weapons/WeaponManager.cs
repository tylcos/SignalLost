using System.Collections;
using System.Linq;
using UnityEngine;



public class WeaponManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public Transform bulletSpawnPoint;

    public WeaponInfo[] WeaponInfos;



    [HideInInspector]
    public Weapon Weapon
    {
        get { return Weapons[CurrentWeapon]; }
    }
    [HideInInspector]
    public Weapon[] Weapons;
    [HideInInspector]
    public int CurrentWeapon = 0;



    private float weaponPos = 0f;
    private bool reloading = false;
    private int weaponBeingReloaded;

    public delegate void WeaponUpdateHandler();
    public event WeaponUpdateHandler WeaponDataChanged;

    public int CurrentAmmo
    {
        get { return Weapon.CurrentAmmo;  }
    }

    public int MaxAmmo
    {
        get { return Weapon.Info.clipSize;  }
    }



    private void OnEnable()
    {
        if (WeaponInfos.Length == 0)
            throw new System.ArgumentOutOfRangeException("No weapon infos selected.");

        Weapons = WeaponInfos.Select(w => new Weapon(w)).ToArray();
        CurrentWeapon = 0;
    }



    void Update()
    {
        
        weaponPos += Mathf.Abs(Input.GetAxis("ScrollWheel"));

        if (Input.GetKeyDown(KeyCode.Tab))
            ++weaponPos;


        
        weaponPos %= Weapons.Length;
        int flooredWeaponPos = Mathf.FloorToInt(weaponPos); 
        if (CurrentWeapon != flooredWeaponPos)
            ChangeWeapon(flooredWeaponPos);



        if (Input.GetKeyDown(KeyCode.R))
        {
            Weapon.CurrentAmmo = 0;
            Reload();
        }

        WeaponDataChanged();
    }

    private void ChangeWeapon(int flooredWeaponPos)
    {
        CurrentWeapon = flooredWeaponPos;

        sr.sprite = Weapon.Info.weaponSprite;
        bulletSpawnPoint.localPosition = Weapon.Info.bulletSpawnOffset;
        reloading = false;



        if (Weapon.CurrentAmmo == 0)
            Reload();
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
        yield return new WaitForSeconds(Weapon.Info.reloadTime);

        if (CurrentWeapon == weaponBeingReloaded)
        {
            Weapon.CurrentAmmo = Weapon.Info.clipSize;
            reloading = false;
        }
    }
}