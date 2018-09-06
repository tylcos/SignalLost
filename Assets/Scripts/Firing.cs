    using UnityEngine;



public class Firing : MonoBehaviour 
{
    public WeaponManager weaponManager;
    public Transform bulletSpawnPoint;

    public Rigidbody2D rb;



    private float timeLastFired;

    
	
	void Update() 
	{
        Vector2 shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));

        if ((Input.GetMouseButton(0) || shootDir.sqrMagnitude != 0) && Time.time - timeLastFired > weaponManager.weapon.cycleTime)
        {
            timeLastFired = Time.time;

            GameObject bullet = Instantiate(
                weaponManager.weapon.bullet,
                bulletSpawnPoint.position,
                bulletSpawnPoint.rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();



            float angle = bulletSpawnPoint.rotation.eulerAngles.z * Mathf.Deg2Rad;
            rb.velocity = (new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) - (rb.velocity * Time.deltaTime)) * weaponManager.weapon.bulletSpeed;

            //Destroy(bullet, 2.0f);
        }
	}
}
