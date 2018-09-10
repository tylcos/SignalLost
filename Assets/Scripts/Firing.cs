using UnityEngine;



public class Firing : MonoBehaviour
{
    public WeaponManager weaponManager;
    public Transform bulletSpawnPoint;

    public Rigidbody2D rbPlayer;
    public GameObject player;



    private float timeLastFired;



    void Update()
    {
        Vector2 shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));

        if ((Input.GetMouseButton(0) || shootDir.sqrMagnitude != 0) && Time.time - timeLastFired > weaponManager.weapon.cycleTime)
        {
            timeLastFired = Time.time;

            float halfAngle = weaponManager.weapon.inaccuracy / 2f;
            Quaternion randomQuaternion = bulletSpawnPoint.rotation 
                * Quaternion.Euler(0f, 0f, Random.Range(-halfAngle, halfAngle));

            GameObject bullet = Instantiate(
                weaponManager.weapon.bullet,
                bulletSpawnPoint.position,
                randomQuaternion);

            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            bullet.GetComponent<BulletCollider>().lifeTime = weaponManager.weapon.bulletLifeTime;



            float randomAngle = randomQuaternion.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 directionVector = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;

            rbBullet.velocity = directionVector * weaponManager.weapon.bulletSpeed; // + rbPlayer.velocity;
        }
    }
}
