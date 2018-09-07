using UnityEngine;



public class Firing : MonoBehaviour
{
    public WeaponManager weaponManager;
    public Transform bulletSpawnPoint;

    public Rigidbody2D rbPlayer;



    private float timeLastFired;



    void Update()
    {
        Vector2 shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));

        if ((Input.GetMouseButton(0) || shootDir.sqrMagnitude != 0) && Time.time - timeLastFired > weaponManager.weapon.cycleTime)
        {
            timeLastFired = Time.time;

            float halfAngle = weaponManager.weapon.inaccuracy / 2;
            Quaternion randomQuaternion = Quaternion.Euler(0, 0, Random.Range(-halfAngle, halfAngle));

            GameObject bullet = Instantiate(
                weaponManager.weapon.bullet,
                bulletSpawnPoint.position,
                bulletSpawnPoint.rotation * randomQuaternion);

            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();




            float angle = bulletSpawnPoint.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 directionVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            rbBullet.velocity = directionVector * weaponManager.weapon.bulletSpeed; // + rbPlayer.velocity;
        }
    }
}
