using UnityEngine;
    


[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject 
{
    public new string name;
    public string description;

    public Sprite weaponSprite;
    public GameObject bullet;

    public double damage;
    public float bulletSpeed;

<<<<<<< HEAD
    public float fireRate;
=======
    public float cycleTime;
>>>>>>> parent of 4b21d91... Added random inaccuracy for weapons and improved the Weapon object
    public float reloadTime;

    public int numberOfProjectiles;
    public float inaccuracy; // Angle of inaccuracy measured

    public float range;
    public bool penetrates;

    public Vector3 bulletSpawnOffset;
}
