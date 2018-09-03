using UnityEngine;



public class WeaponManager : MonoBehaviour 
{
    public SpriteRenderer sr;
    public Weapon weapon;



	void Start() 
	{
        sr.sprite = weapon.sprite;
    }
	
	void Update() 
	{
		// Code to switch weapons
	}
}
