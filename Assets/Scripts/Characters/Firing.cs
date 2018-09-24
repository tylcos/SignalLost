using UnityEngine;



public class Firing : MonoBehaviour
{
    public WeaponManager weaponManager;
    public Transform bulletSpawnPoint;
    public ParticleSystem ps;



    private float timeLastFired;



    void Update()
    {
        Vector2 shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));

        if ((Input.GetMouseButton(0) || shootDir.sqrMagnitude != 0) 
            && Time.time - timeLastFired > weaponManager.Weapon.cycleTime)
        {
            if (weaponManager.Weapon.ammo == 0)
            {
                weaponManager.Reload();
                return;
            }

            timeLastFired = Time.time;
            --weaponManager.Weapon.ammo;



            float halfAngle = weaponManager.Weapon.inaccuracy / 2f;
            Quaternion randomQuaternion = bulletSpawnPoint.rotation 
                * Quaternion.Euler(0f, 0f, Random.Range(-halfAngle, halfAngle));

            GameObject bullet = Instantiate(weaponManager.Weapon.bullet, bulletSpawnPoint.position, randomQuaternion);
            
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            BulletManager bm = bullet.GetComponent<BulletManager>();



            bm.lifeTime = weaponManager.Weapon.bulletLifeTime;
            bm.damage = weaponManager.Weapon.damage;
            
            float randomAngle = randomQuaternion.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 directionVector = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;

            rbBullet.velocity = directionVector * weaponManager.Weapon.bulletSpeed; // + rbPlayer.velocity;

            ps.Play();



            if (weaponManager.Weapon.ammo == 0)
                weaponManager.Reload();
        }
    }
}
