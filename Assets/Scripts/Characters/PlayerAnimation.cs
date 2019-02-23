using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    //public GameObject weaponInfo;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public MovementController movementAccessor;

    private Animator playerAnimator;

    private bool idle;
    private Coroutine animationDelayObject;

	// Use this for initialization    
        public void Start()
        {
            transform.hasChanged = false;
            player = GameObject.FindGameObjectWithTag("Player");
            movementAccessor = player.GetComponent<PlayerController>();
            playerAnimator = movementAccessor.GetComponent<PlayerController>().spriteAnimator;
        }

    void LateUpdate() {
        if (movementAccessor.movingForAnimation)
        {
            StopCoroutine("AnimationDelay");
            playerAnimator.SetBool("Idle", false);
        }
        else 
        {
            StartCoroutine("AnimationDelay");
        }

        if (movementAccessor.animationDirection == 1) // North && East
        {
            playerAnimator.SetInteger("PlayerDirection", 1);
        }
        else if (movementAccessor.animationDirection == 3) // South
        {
            playerAnimator.SetInteger("PlayerDirection", 3);
        }
        else if (movementAccessor.animationDirection == 4) // West
        {
            playerAnimator.SetInteger("PlayerDirection", 4);
        }

        movementAccessor.movingForAnimation = false;
    }

    IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(3.0f);
        playerAnimator.SetBool("Idle", true);
    }
}
