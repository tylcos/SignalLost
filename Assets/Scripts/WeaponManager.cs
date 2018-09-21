using System.Collections;
using UnityEngine;



public class WeaponManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public Transform bulletSpawnPoint;

    public Weapon Weapon
    {
        get { return Weapons[currentWeapon]; }
    }

    [HideInInspector]
    public int currentWeapon = -1;
    public Weapon[] Weapons;



    private float scrollWheelPos;
    private bool reloading = false;
    private int weaponBeingReloaded;



    void Start()
    {
        foreach (Weapon weapon in Weapons)
            weapon.ammo = weapon.clipSize;
    }



    void Update() 
	{
        scrollWheelPos += Mathf.Abs(Input.GetAxis("ScrollWheel"));
        scrollWheelPos %= Weapons.Length;

        if (currentWeapon != (int)scrollWheelPos)
        {
            currentWeapon = (int)scrollWheelPos;

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
            weaponBeingReloaded = currentWeapon;
            StartCoroutine(ReloadInternal());
        }

        reloading = true;
    }

    private IEnumerator ReloadInternal()
    {
        yield return new WaitForSeconds(Weapon.reloadTime);

        if (currentWeapon == weaponBeingReloaded)
        {
            Weapon.ammo = Weapon.clipSize;
            reloading = false;
        }
    }
}
