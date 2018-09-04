using UnityEngine;



public class Firing : MonoBehaviour 
{
    public WeaponManager weaponManager;
    public Transform bulletSpawnPoint;

    public CharacterController cc;



    private float timeLastFired;

    
	
	void Update() 
	{
		if (Input.GetMouseButton(0) && Time.time - timeLastFired > weaponManager.weapon.cycleTime)
        {
            timeLastFired = Time.time;

            GameObject bullet = Instantiate(
                weaponManager.weapon.bullet,
                bulletSpawnPoint.position,
                bulletSpawnPoint.rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Debug.Log(bulletSpawnPoint.rotation.eulerAngles);

            // Need to use atan instead of mouseDirection
            float angle = bulletSpawnPoint.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 playerVelocity = (Vector2)(cc.velocity * Time.deltaTime);
            rb.velocity = (new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) - playerVelocity) * weaponManager.weapon.bulletSpeed;

            Destroy(bullet, 2.0f);
        }
	}
}
