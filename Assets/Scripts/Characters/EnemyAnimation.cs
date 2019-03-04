using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{

    private RangedEnemyController rangedEnemyController;
    public Animator enemyAnimator;

    // Use this for initialization
    void Start()
    {
        rangedEnemyController = this.gameObject.GetComponent<RangedEnemyController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (rangedEnemyController.movingForAnimation)
        {
            
        }
    }

}
