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
    private bool attackComplete = false;

    // Use this for initialization
    void Start () {
        enemyControllerAccessor = meleeEnemy.GetComponent<EnemyController>();
    }
	
	// Update is called once per frame
	void Update () {

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
            animator.SetBool("Attack", true);
            if (enemyControllerAccessor.isAggro == false)
            {
                animator.SetBool("PlayerInRange", false);
                attackComplete = false;
                inRange = false;
                formation = false;
            }
        }
    }
}
