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
       /* if (movementAccessor.movingForAnimation)
        {
            rangedAnimator.SetBool("Idle", false);
        }
        else
        {
            rangedAnimator.SetBool("Idle", true);
        }*/

        if (movementAccessor.animationDirection == 3) // Checks if sprite is moving east
        {
            rangedAnimator.SetInteger("Direction", 1);
        }
        else if (movementAccessor.animationDirection == 4) // Checks if sprite is moving west
        {
            rangedAnimator.SetInteger("Direction", 4);
        }   
    }
}
