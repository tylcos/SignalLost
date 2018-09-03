using UnityEngine;



public class Firing : MonoBehaviour 
{
    public WeaponManager weaponManager;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;



    private float timeLastFired;

    
	
	void Update() 
	{
		if (Input.GetMouseButton(0))
        {
            if (Time.time - timeLastFired > weaponManager.weapon.cycleTime)
            {
                timeLastFired = Time.time;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                rb.velocity = MouseWrapper.GetMouseDirection() * weaponManager.weapon.bulletSpeed;

                Destroy(bullet, 2.0f);
            }
        }
	}
}
