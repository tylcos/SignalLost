using UnityEngine;



public class Firing : MonoBehaviour
{

    // merge this into weaponmanager and call fire methods from a gun object
    // for gods sake put some of this stuff in methods
    public WeaponManager weaponManager;
    public Transform bulletSpawnPoint;
    public ParticleSystem ps;
    public string bulletLayer;
    private GameController master;

    private float timeLastFired;

    private void OnEnable()
    {
        master = GameObject.FindGameObjectWithTag("Master").GetComponent<GameController>();
    }

    void Update()
    {
        Vector2 shootDir = Vector2.zero;
        if (master.inputMethod == "keyboard")
        {
            shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));
        }
        else if(master.inputMethod == "arcade")
        {
            shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeysArcade"), Input.GetAxisRaw("VerticalKeysArcade"));
        }



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
            Quaternion randomQuaternion = bulletSpawnPoint.rotation;
            randomQuaternion.eulerAngles += new Vector3(0f, 0f, Random.Range(-halfAngle, halfAngle));

            GameObject bullet = Instantiate(weaponManager.Weapon.Info.bullet, bulletSpawnPoint.position, randomQuaternion);
            
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            BulletManager bm = bullet.GetComponent<BulletManager>();
            
            bm.lifeTime = weaponManager.Weapon.Info.bulletLifeTime;
            bm.damage = weaponManager.Weapon.Info.damage;
            bm.maxCollisions = weaponManager.Weapon.Info.penetrationDepth;
            bm.gameObject.layer = LayerMask.NameToLayer(bulletLayer);
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
