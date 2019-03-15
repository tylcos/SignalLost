using System.Collections.Generic;
using UnityEngine;



public class WeaponController : MonoBehaviour
{
    // we need a dedicated enemyweaponcontroller
    protected const int INVSIZE = 4;
    public const int COMBATMODE_GUN = 0;
    public const int COMBATMODE_MELEE = 1;

    [SerializeField]
    public WeaponV2Information[] inventory = new WeaponV2Information[INVSIZE];
    protected List<EquippedWeapon> swapList = new List<EquippedWeapon>(INVSIZE);
    protected int swapListIndex = 0;

    public string bulletLayer;
    [HideInInspector]
    public int combatMode;

    private void OnValidate()
    {
        if (inventory.Length != INVSIZE)
        {
            Debug.LogWarning("Don't change the weapon inventory array size!");
            System.Array.Resize(ref inventory, INVSIZE);
        }
    }

    private void OnEnable()
    {
        // populates the swaplist
        foreach (WeaponV2Information wep in inventory)
        {
            // create a class that stores a reference to the object we 
            if (wep == null)
                swapList.Add(EquippedWeapon.empty);
            else
                swapList.Add(new EquippedWeapon(wep, transform, bulletLayer, GetComponentInParent<MovementController>(), wep.combatMode));
            // so we have a parallel array that holds the objects, their bullets, and ammo
            // so we call commands there when we need to fire or whatnot since that stores everything
        }
        SwapTo(0);
    }

    private void SwapTo(int index)
    {
        swapList[swapListIndex].SetEnabled(false);
        swapList[index].SetEnabled(true);
        swapListIndex = index;
        combatMode = swapList[index].combatMode;
        
    }

    public EquippedWeapon GetEquippedWeapon()
    {
        return swapList[swapListIndex];
    }

    public void AimInDirection(Vector2 direction)
    {
        var xd = Quaternion.LookRotation(direction, Vector3.up);
        xd.z = xd.x;
        xd.x = 0;
        xd.y = 0;
        transform.rotation = xd;
        float angleDifference = (Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x)) - transform.eulerAngles.z;

        transform.RotateAround(transform.position, Vector3.forward, angleDifference);
    }

    public void Fire(Vector2 shootDirection)
    {
        swapList[swapListIndex].Fire(shootDirection);
    }

    public bool CanFire()
    { //TECHNINCALLY SPEAKING, ENEMIES HAVE LIMITED AMMO SINCE THEY DONT RELOAD YET SO THIS IS ALWAYS TRUE UNLESS IT JUST FIRED LMAO
        return swapList[swapListIndex].CanFire();
    }
}
