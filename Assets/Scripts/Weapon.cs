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

    public double cycleTime;
    public double reloadTime;

    public int numberOfProjectiles;
    public double inaccuracy;

    public float range;
    public bool penetrates;

    public Vector3 bulletSpawnOffset;
}
