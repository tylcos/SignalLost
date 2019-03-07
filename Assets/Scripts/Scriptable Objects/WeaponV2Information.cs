using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "My new weapon", menuName = "WeaponV2")]
public class WeaponV2Information : ScriptableObject
{
    [Dropdown("test")]
    public int combatMode;

    private DropdownList<int> test = new DropdownList<int>()
    {
        { "Melee", 1 },
        { "Gun"  , 2 }
    };

    [Tooltip("The prefab for the projectile fired.")]
    [ShowAssetPreview]
    [Required]
    public GameObject bullet;

    [Tooltip("The prefab for the weapon.")]
    [ShowAssetPreview]
    [Required]
    public GameObject weapon;

    [Tooltip("The prefab for the final impact effect.")]
    public GameObject impactVFX;

    [Tooltip("The prefab for the muzzle effect.")]
    public GameObject muzzleVFX;

    [Tooltip("The prefab for the penetration effect.")]
    public GameObject penetrationVFX;

    [Tooltip("The prefab for the ricochet effect.")]
    public GameObject ricochetVFX;

    [Tooltip("The minimum time between shots.")]
    public float cycleTime;

    [Tooltip("How fast the bullets travel.")]
    public float muzzleVelocity;

    [Tooltip("How long bullets stay in the scene.")]
    public float lifetime;

    [Tooltip("How much damage one bullet will deal.")]
    public float damage;

    [Tooltip("Max ammo per clip.")]
    public int clipSize;

    [Tooltip("How long it takes to reload.")]
    public float reloadTime;

    [Tooltip("Max number of impacts (penetrations + ricochets) allowed.")]
    public int impacts;
    // ricochets are calculated first, if failed a penetration is attempted
    [Tooltip("Whether the bullet can ricochet.")]
    public bool canRicochet;

    [Tooltip("The largest angle a bullet can ricochet at.")]
    [Range(0, 90)]
    public float ricochetAngle;

    [Tooltip("Whether the bullet can penetrate soft targets.")]
    public bool canPenetrate;

    [Tooltip("The maximum strength material the bullet can penetrate.\nNO IMPLEMENTATION")]
    public int penetrationStrength;

    [Tooltip("Radius around impacts to deal damage.")]
    public float impactRadius;

    [Tooltip("Whether or not to use damage falloff in the impact radius.")]
    public bool falloff;

    [Tooltip("The angle of the spread cone divided by 2.")]
    [Range(0, 90)]
    public float halfSpreadAngle;

    // below this put rarity variations
}
