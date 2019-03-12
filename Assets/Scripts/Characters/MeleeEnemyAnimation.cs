using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnimation : MonoBehaviour
{
    [HideInInspector]
    public MovementController movementAccessor;

    private Animator meleeAnimator;


    // Start is called before the first frame update
    void Start()
    {
        movementAccessor = this.GetComponent<MovementController>();
        meleeAnimator = movementAccessor.GetComponent<MovementController>().spriteAnimator;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (movementAccessor.movingForAnimation)
        {
            meleeAnimator.SetBool("Idle", false);
        }
        else
        {
            meleeAnimator.SetBool("Idle", true);
        }
        if(movementAccessor.animationDirection == 3) // Checks if sprite is moving east
        {
            meleeAnimator.SetInteger("Direction", 1);
        }
        else if(movementAccessor.animationDirection == 4) // Checks if sprite is moving west
        {
            meleeAnimator.SetInteger("Direction", 4);
        }
    }

    // For Idle
    IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(3.0f);
        meleeAnimator.SetBool("Idle", true);
    }
}
