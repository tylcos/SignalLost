using UnityEngine;



public class WeaponManager : MonoBehaviour 
{
    public SpriteRenderer sr;
    public Transform bulletSpawnPoint;

    public Weapon weapon;

    public int currentWeapon = -1;
    public Weapon[] weapons;



    private float scrollWheelPos;



	void Update() 
	{
        scrollWheelPos += Input.GetAxis("ScrollWheel");
        scrollWheelPos = Mathf.Clamp(scrollWheelPos, 0, weapons.Length - 1);

        if (currentWeapon != (int)scrollWheelPos)
        {
            currentWeapon = (int)scrollWheelPos;

            weapon = weapons[currentWeapon];
            sr.sprite = weapon.weaponSprite;
            bulletSpawnPoint.localPosition = weapon.bulletSpawnOffset;
        }
    }
}
