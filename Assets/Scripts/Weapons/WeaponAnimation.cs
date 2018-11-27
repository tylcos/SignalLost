﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : EnemyAnimation {

    private bool formation = false;
    private bool inRange = false;
    private bool shot = false;
    public Animation anim;


    // Use this for initialization
    void Start () {
        enemyControllerAccessor = meleeEnemy.GetComponent<EnemyController>();
	}

    // Update is called once per frame
    void Update()
    {
        if(enemyControllerAccessor.isAggro == true)
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
                shot = false;
                inRange = false;
                formation = false;
            }
        }
    }   
}
