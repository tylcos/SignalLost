using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "My new weapon", menuName = "WeaponV2")]
public class WeaponInformation : ScriptableObject
{
    [Tooltip("The prefab for the projectile fired.")]
    public GameObject bullet;

    [Tooltip("The prefab for the weapon.")]
    public GameObject weapon;

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
}
