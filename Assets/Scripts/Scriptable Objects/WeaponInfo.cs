using UnityEngine;



[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponInfo : ScriptableObject
{
    [Tooltip("The ingame name of the weapon.")]
    public new string name;
    [Tooltip("A description of the weapob.")]
    public string description;



    [Space(20)]
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
    [Tooltip("The time in seconds inbetween reloads.")]
    public float reloadTime;



    [Space(20)]
    [Tooltip("The total number of projectiles fired per shot.")]
    public int numberOfProjectiles;
    [Range(0f, 90f)]
    [Tooltip("The cone of inaccuracy in degrees.")]
    public float inaccuracy;
    [Tooltip("How long in seconds for the projectiles to stay alive for.")]
    public float bulletLifeTime;
    [Tooltip("Does the bullet continue after hitting an enemy.")]
    public bool penetrates;



    [Space(20)]
    [Tooltip("A position offset from the Weapon gameObject to spawn bullets at.")]
    public Vector3 bulletSpawnOffset;
}
