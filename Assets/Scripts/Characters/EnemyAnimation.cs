using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour {

    public Animator animator;
    [HideInInspector]
    public GameObject meleeEnemy;
    [HideInInspector]
    public EnemyController enemyControllerAccessor;

    private bool formation = false;
    private bool inRange = false;

    // Use this for initialization
    void Start () {
        enemyControllerAccessor = meleeEnemy.GetComponent<EnemyController>();
    }
	
	// Update is called once per frame
	void Update () {
        // May use if statements to call specific methods for specific enemies
        // OR May make seperate scripts for various types of enemies
        // OR May seperate if statements into own methods to call in enemy type animations
        RangedEnemyAnim();
    }

    //Enemy Type Animations
    private void RangedEnemyAnim()
    {
        if (enemyControllerAccessor.IsAggro == true)
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
            animator.SetBool("Attack", true);
            if (enemyControllerAccessor.IsAggro == false)
            {
                animator.SetBool("PlayerInRange", false);
                inRange = false;
                formation = false;
            }
        }
    }
 
    /* private void MeleeEnemyAnim(){
     
    }*/

    /*private void BossAnim(){
    
    }*/
}
