using UnityEngine;



public class Firing : MonoBehaviour
{
    public WeaponManager weaponManager;
    public Transform bulletSpawnPoint;
    public ParticleSystem ps;
    public string bulletLayer;
    
    private float timeLastFired;

    void Update()
    {
        Vector2 shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));

        if ((Input.GetAxis("Fire1") > 0 || shootDir.sqrMagnitude != 0) 
            && Time.time - timeLastFired > weaponManager.Weapon.Info.cycleTime)
        {
            if (weaponManager.Weapon.CurrentAmmo == 0)
            {
                weaponManager.Reload();
                return;
            }

            timeLastFired = Time.time;
            --weaponManager.Weapon.CurrentAmmo;



            float halfAngle = weaponManager.Weapon.Info.inaccuracy / 2f;
            Quaternion randomQuaternion = bulletSpawnPoint.rotation 
                * Quaternion.Euler(0f, 0f, Random.Range(-halfAngle, halfAngle));

            GameObject bullet = Instantiate(weaponManager.Weapon.Info.bullet, bulletSpawnPoint.position, randomQuaternion);
            
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            BulletManager bm = bullet.GetComponent<BulletManager>();
            
            bm.lifeTime = weaponManager.Weapon.Info.bulletLifeTime;
            bm.damage = weaponManager.Weapon.Info.damage;
            bm.maxCollisions = weaponManager.Weapon.Info.penetrationDepth;
            bm.gameObject.layer = 12;
            //bm.targetTags = new string[] {"Enemy"};

            float randomAngle = randomQuaternion.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 directionVector = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;

            rbBullet.velocity = directionVector * weaponManager.Weapon.Info.bulletSpeed; // + rbPlayer.velocity;

            //ps.Play();



            if (weaponManager.Weapon.CurrentAmmo == 0)
                weaponManager.Reload();
        }
    }
}
