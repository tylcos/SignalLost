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
}
