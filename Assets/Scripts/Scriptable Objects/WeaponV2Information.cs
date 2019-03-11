using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "My new weapon", menuName = "WeaponV2")]
public class WeaponV2Information : ScriptableObject
{
    [Dropdown("Mode")]
    public int combatMode;

    private DropdownList<int> Mode = new DropdownList<int>()
    {
        { "Melee", WeaponController.COMBATMODE_MELEE },
        { "Gun"  , WeaponController.COMBATMODE_GUN }
    };

    [BoxGroup("Prefabs")]
    [Tooltip("The prefab for the weapon.")]
    [ShowAssetPreview]
    [Required]
    public GameObject weapon;

    private bool GunMode()
    {
        return combatMode == WeaponController.COMBATMODE_GUN;
    }

    private bool MeleeMode()
    {
        return combatMode == WeaponController.COMBATMODE_MELEE;
    }

    #region guns
    [ShowIf("GunMode")]
    [BoxGroup("Prefabs")]
    [Tooltip("The prefab for the projectile fired.")]
    [ShowAssetPreview]
    [Required]
    public GameObject bullet;

    [ShowIf("GunMode")]
    [BoxGroup("Prefabs")]
    [Tooltip("The prefab for the final impact effect.")]
    public GameObject impactVFX;

    [ShowIf("GunMode")]
    [BoxGroup("Prefabs")]
    [Tooltip("The prefab for the muzzle effect.")]
    public GameObject muzzleVFX;

    [ShowIf("GunMode")]
    [BoxGroup("Prefabs")]
    [Tooltip("The prefab for the penetration effect.")]
    public GameObject penetrationVFX;

    [ShowIf("GunMode")]
    [BoxGroup("Prefabs")]
    [Tooltip("The prefab for the ricochet effect.")]
    public GameObject ricochetVFX;

    [ShowIf("GunMode")]
    [Tooltip("The minimum time between shots.")]
    public float cycleTime;

    [ShowIf("GunMode")]
    [Tooltip("How fast the bullets travel.")]
    public float muzzleVelocity;

    [ShowIf("GunMode")]
    [Tooltip("How long bullets stay in the scene.")]
    public float lifetime;

    [ShowIf("GunMode")]
    [Tooltip("How much damage one bullet will deal.")]
    public float bulletDamage;

    [ShowIf("GunMode")]
    [Tooltip("Max ammo per clip.")]
    public int clipSize;

    [ShowIf("GunMode")]
    [Tooltip("How long it takes to reload.")]
    public float reloadTime;

    [ShowIf("GunMode")]
    [Tooltip("Max number of impacts (penetrations + ricochets) allowed.")]
    public int impacts;

    // ricochets are calculated first, if failed a penetration is attempted
    [ShowIf("GunMode")]
    [Tooltip("Whether the bullet can ricochet.")]
    public bool canRicochet;

    [ShowIf("GunMode")]
    [Tooltip("The largest angle a bullet can ricochet at.")]
    [Range(0, 90)]
    public float ricochetAngle;

    [ShowIf("GunMode")]
    [Tooltip("Whether the bullet can penetrate soft targets.")]
    public bool canPenetrate;

    [ShowIf("GunMode")]
    [Tooltip("The maximum strength material the bullet can penetrate.\nNO IMPLEMENTATION")]
    public int penetrationStrength;

    [ShowIf("GunMode")]
    [Tooltip("The angle of the spread cone divided by 2.")]
    [Range(0, 90)]
    public float halfSpreadAngle;

    [ShowIf("GunMode")]
    [Tooltip("Radius around impacts to deal damage.")]
    public float impactRadius;

    [ShowIf("GunMode")]
    [Tooltip("Whether or not to use damage falloff in the impact radius.")]
    public bool falloff;

    #endregion

    #region melee

    [ShowIf("MeleeMode")]
    [Tooltip("How much damage one hit will deal.")]
    public float meleeDamage;

    [ShowIf("MeleeMode")]
    [BoxGroup("Prefabs")]
    [Tooltip("The prefab for the swing effect.")]
    public GameObject swingVFX;

    #endregion

    // below this put rarity variations
}
