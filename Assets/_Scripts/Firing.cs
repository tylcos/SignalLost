using UnityEngine;



public class Firing : MonoBehaviour 
{
    public WeaponManager weaponManager;
    public Transform bulletSpawnPoint;



    private float timeLastFired;

    
	
	void Update() 
	{
		if (Input.GetMouseButton(0) && Time.time - timeLastFired > weaponManager.weapon.cycleTime)
        {
            timeLastFired = Time.time;

            GameObject bullet = Instantiate(
                weaponManager.weapon.bullet,
                bulletSpawnPoint.position,
                transform.rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();



            // Need to use atan instead of mouseDirection
            rb.velocity = MouseWrapper.GetMouseDirection() * weaponManager.weapon.bulletSpeed;

            Destroy(bullet, 2.0f);
        }
	}
}
