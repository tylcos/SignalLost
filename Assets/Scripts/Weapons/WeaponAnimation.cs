using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : EnemyAnimation {

	// Use this for initialization
	void Start () {
        meleeEnemyAccessor = meleeEnemy.GetComponent<MeleeEnemyController>();
	}

    // Update is called once per frame
    void Update()
    {

        if (meleeEnemyAccessor.isAggro == true)
        {
            animator.SetBool("PlayerInRange", true);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Formation"))
        {
            animator.SetBool("FormationComplete", true);
            if (meleeEnemyAccessor.isAggro == true)
            {
                animator.SetBool("Shot", true);
            }
        }
        else
        {
            animator.SetBool("FormationComplete", false);
            animator.SetBool("Shot", false);

        }

        if (meleeEnemyAccessor.isAggro == false)
        {
            animator.SetBool("PlayerInRange", false);
        }
    }   
}
