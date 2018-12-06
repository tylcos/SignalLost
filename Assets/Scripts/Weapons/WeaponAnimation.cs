using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : EnemyAnimation {

    private bool formation = false;
    private bool inRange = false;


    // Use this for initialization
    void Start () {
        enemyControllerAccessor = meleeEnemy.GetComponent<EnemyController>();
	}

    // Update is called once per frame
    void Update()
    {
        // May use if statements to call specific methods for specific enemies
        // OR May make seperate scripts for various types of enemies
        // OR May seperate if statements into own methods to call in enemy type animations
        GunAnim();
    }
   
// Weapon Types Animations
    private void GunAnim()
    {
        if (enemyControllerAccessor.isAggro == true)
        {
            animator.SetBool("PlayerInRange", true);
            inRange = true;
        }
        if (inRange)
        {
            animator.SetBool("Formation", true);
            formation = true;
        }
        if (formation)
        {
            animator.SetBool("Shot", true);
            if (enemyControllerAccessor.isAggro == false)
            {
                animator.SetBool("PlayerInRange", false);
                inRange = false;
                formation = false;
            }
        }
    }

    private void MeleeAnim()
    {
        if(enemyControllerAccessor.isAggro == true)
        {
            animator.SetBool("PlayerInRange", true);
            inRange = true;
        }
        if (inRange)
        {
            
        }
     /*   If player is in range, play walking/running animation
            If player is within attacking range, play attacking animation
        (KEEP LOOPING THROUGH)*/
    }
    /*private void SpecialWeaponAnim(){
    
    }*/
}