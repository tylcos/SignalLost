using UnityEngine;



[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon (Old)")]
[System.Obsolete("Use a U-Weapon instead.")]
public class WeaponInfo : ScriptableObject
{
    [Tooltip("The ingame name of the weapon.")]
    public new string name;
    [Tooltip("A description of the weapon.")]
    public string description;
    [Tooltip("The sprite for the weapon.")]
    public Sprite weaponSprite;
    [Tooltip("The prefab for the projectile fired.")]
    public GameObject bullet;



    [Space(20)]
    [Tooltip("The damage done to enemies when hit.")]
    public float damage;
    [Tooltip("The speed of the bullet in units per second.")]
    public float bulletSpeed;



    [Space(20)]
    [Tooltip("The time in seconds inbetween shots.")]
    public float cycleTime;
    [Tooltip("The number of bullets in each clip.")]
    public int clipSize;
    [Tooltip("The time it takes to reload.")]
    public float reloadTime;
    [Tooltip("The number of bursts this weapon shoots per pull of the trigger.")]
    public int numberOfBursts;
    [Tooltip("The time between bursts.")]
    public float burstCycleTime;
    [Tooltip("The total number of projectiles fired per burst.")]
    public int numberOfProjectilesPerBurst;
    [Range(0f, 90f)]
    [Tooltip("The cone of inaccuracy in degrees.")]
    public float inaccuracy;
    [Tooltip("How long in seconds for the projectiles to stay alive for.")]
    public float bulletLifeTime;
    [Tooltip("How many colliders can bullets hit before being destroyed (1 disabled penetration).")]
    public float penetrationDepth;
    [Range(0f, 90f)]
    [Tooltip("The smallest angle a bullet can hit a wall and still ricochet (0 is disabled, 90 is always ricochet).\nRicochets consume a penetration.")]
    public float richochetAngle;



    [Space(20)]
    [Tooltip("A position offset from the Weapon gameObject to spawn bullets at.")]
    public Vector3 bulletSpawnOffset;
}
