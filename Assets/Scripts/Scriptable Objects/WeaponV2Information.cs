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
    [Tooltip("How much damage one bullet will deal.")]
    public float bulletDamage;

    [ShowIf("GunMode")]
    [Tooltip("The minimum time between shots.")]
    [MinValue(0)]
    public float cycleTime;

    [ShowIf("GunMode")]
    [Tooltip("Max ammo per clip.")]
    [MinValue(0)]
    public int clipSize;

    [ShowIf("GunMode")]
    [Tooltip("How long it takes to reload.")]
    public float reloadTime;

    [ShowIf("GunMode")]
    [Tooltip("How fast the bullets travel.")]
    [MinValue(0)]
    public float muzzleVelocity;

    [ShowIf("GunMode")]
    [Tooltip("How long bullets stay in the scene.")]
    [MinValue(0)]
    public float lifetime;

    [ShowIf("GunMode")]
    [Tooltip("The angle of the spread cone divided by 2.")]
    [Range(0, 90)]
    public float halfSpreadAngle;

    [ShowIf("GunMode")]
    [Tooltip("Max number of impacts (penetrations + ricochets) allowed.")]
    [MinValue(0)]
    public int impacts;

    // ricochets are calculated first, if failed a penetration is attempted
    [ShowIf("GunMode")]
    [Tooltip("Whether the bullet can ricochet.")]
    public bool canRicochet;

    [ShowIf("GunMode")]
    [Tooltip("The largest angle a bullet can ricochet at.")]
    [Range(0, 90)]
    [EnableIf(("canRicochet"))]
    public float ricochetAngle;

    [ShowIf("GunMode")]
    [Tooltip("Whether the bullet can penetrate soft targets.")]
    public bool canPenetrate;

    [ShowIf("GunMode")]
    [Tooltip("The maximum strength material the bullet can penetrate.\nNO IMPLEMENTATION")]
    [EnableIf("canPenetrate")]
    public int penetrationStrength;

    [ShowIf("GunMode")]
    [Tooltip("Radius around impacts to deal damage.")]
    [MinValue(0)]
    public float impactRadius;

    private bool AOE()
    {
        return impactRadius > 0;
    }

    [ShowIf("GunMode")]
    [Tooltip("Whether or not to use damage falloff in the impact radius.")]
    [EnableIf("AOE")]
    public bool falloff;
    // change this to select which function fallout is calculated with e.g. linear, square, cubic

    #endregion

    #region melee

    [ShowIf("MeleeMode")]
    [BoxGroup("Prefabs")]
    [Tooltip("The prefab for the swing effect.")]
    public GameObject swingVFX;

    [ShowIf("MeleeMode")]
    [Tooltip("How much damage one hit will deal.")]
    [MinValue(0)]
    public float meleeDamage;

    #endregion

    // below this put rarity variations
}
