using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour {

    public Animator animator;
    [HideInInspector]
    public GameObject meleeEnemy;
    public EnemyController enemyControllerAccessor;

    private bool formationComplete = false;
    private bool inRange = true;
    private bool attackComplete = true;

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
        else
        {
            animator.SetBool("PlayerInRange", false);
            animator.SetBool("Formation", false);
            animator.SetBool("Attack", false);
            inRange = false;
            formationComplete = false;
            attackComplete = false;
        }

        if (inRange)
        {
            animator.SetBool("Formation", true);
            formationComplete = true;
        }
        if (formationComplete)
        {
            animator.SetBool("Attack", true);
            attackComplete = true;
        }
    }
}
