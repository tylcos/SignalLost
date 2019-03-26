using System.Collections.Generic;
using UnityEngine;



public class WeaponController : MonoBehaviour
{
    #region fields and constants

    // we need a dedicated enemyweaponcontroller
    protected const int INVSIZE = 4;
    public const int COMBATMODE_GUN = 0;
    public const int COMBATMODE_MELEE = 1;

    [SerializeField]
    public WeaponV2Information[] inventory = new WeaponV2Information[INVSIZE];
    /// <summary>
    /// The list of all EquippedWeapons that this WeaponController has in its inventory.
    /// </summary>
    protected List<EquippedWeapon> swapList = new List<EquippedWeapon>(INVSIZE);
    /// <summary>
    /// The index of the currently active weapon.
    /// </summary>
    protected int swapListIndex = 0;
    /// <summary>
    /// The layer this weapon will use for collisions.
    /// </summary>
    public string bulletLayer;
    [HideInInspector]
    public int combatMode; // used to determine whether we're shooting bullets or swinging swords

    #endregion

    #region monobehavior

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
        GetEquippedWeapon().SetEnabled(true);
    }

    #endregion

    /// <summary>
    /// Sets the weapon at <c>index</c> as active and deactivated the currently active weapon.
    /// </summary>
    /// <param name="index">Index to swap to.</param>
    private void SwapTo(int index)
    {
        swapList[swapListIndex].SetEnabled(false);
        swapList[index].SetEnabled(true);
        swapListIndex = index;
        combatMode = swapList[index].combatMode;
    }

    #region public 

    /// <summary>
    /// Returns the weapon currently in use.
    /// </summary>
    /// <returns>The equipped weapon.</returns>
    public EquippedWeapon GetEquippedWeapon()
    {
        return swapList[swapListIndex];
    }

    /// <summary>
    /// Aims the weapon in a direction.
    /// </summary>
    /// <param name="direction">The direction to aim the weapon in.</param>
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

    /// <summary>
    /// Attack with the equipped weapon (i.e. shoot gun or swing sword).
    /// </summary>
    /// <param name="shootDirection">The direction to shoot in.</param>
    public void Fire(Vector2 shootDirection)
    {
        GetEquippedWeapon().Fire(shootDirection);
    }

    /// <summary>
    /// Checks whether the equipped weapon is in the process of executing attack functions.
    /// </summary>
    /// <returns>Whether or not the equipped weapon is attacking.</returns>
    public bool IsFiring()
    {
        return GetEquippedWeapon().IsFiring();
    }

    /// <summary>
    /// Checks if the weapon can attack.
    /// </summary>
    /// <returns>Whether the weapon can attack.</returns>   
    public bool CanFire()
    { //TECHNINCALLY SPEAKING, ENEMIES HAVE LIMITED AMMO SINCE THEY DONT RELOAD YET SO THIS IS ALWAYS TRUE UNLESS IT JUST FIRED
        return GetEquippedWeapon().CanFire();
    }

    #endregion

}
