using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAnimation : MonoBehaviour
{
    [HideInInspector]
    public MovementController movementAccessor;

    private Animator rangedAnimator;

    // Start is called before the first frame update
    void Start()
    {
        movementAccessor = this.GetComponent<MovementController>();
        rangedAnimator = movementAccessor.GetComponent<MovementController>().spriteAnimator;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Debug.Log(movementAccessor.animationDirection);
       /* if (movementAccessor.movingForAnimation)
        {
            rangedAnimator.SetBool("Idle", false);
        }
        else
        {
            rangedAnimator.SetBool("Idle", true);
        }*/

        if (movementAccessor.animationDirection == 2) // Checks if sprite is moving east
        {
            rangedAnimator.SetInteger("Direction", 2);
        }
        else if (movementAccessor.animationDirection == 4) // Checks if sprite is moving west
        {
            rangedAnimator.SetInteger("Direction", 4);
        }

        if (movementAccessor.chargingAttackAnim)
        {
            rangedAnimator.SetBool("InRange", true);
        }
        else
        {
            rangedAnimator.SetBool("InRange", false);
        }
    }
}
