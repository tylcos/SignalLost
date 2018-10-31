using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour {

    public RangedEnemyController parentController;
    public Transform bulletSpawnPoint;
    public WeaponInfo gunType;

    private float timeLastFired;

    private void Start()
    {
        parentController = GetComponentInParent<RangedEnemyController>();
    }

    void OnEnable () {
        timeLastFired = Time.time - gunType.cycleTime;
	}
	
	public IEnumerator Shoot(Vector2 vector)
    {
        parentController.shooting = true;
        yield return new WaitForFixedUpdate();
        SpawnBulletsTowards(vector);
        yield return new WaitForFixedUpdate();
        timeLastFired = Time.time;
        parentController.shooting = false;
    }

    public void SpawnBulletsTowards(Vector2 vector)
    {
        float halfAngle = gunType.inaccuracy / 2f;
        Quaternion randomQuaternion = bulletSpawnPoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(-halfAngle, halfAngle));
        GameObject bullet = Instantiate(gunType.bullet, bulletSpawnPoint.position, randomQuaternion);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        BulletManager bm = bullet.GetComponent<BulletManager>();

        bm.lifeTime = gunType.bulletLifeTime;
        bm.damage = gunType.damage;
        bm.maxCollisions = gunType.penetrationDepth;
        //bm.targetTags = parentController.targetTags;

        float randomAngle = randomQuaternion.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 directionVector = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
        rbBullet.velocity = directionVector * gunType.bulletSpeed;
    }

    public bool CanFire()
    {
        return Time.time - timeLastFired > gunType.cycleTime;
    }
}
