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
    private Coroutine animationDelayObject = null;

	// Use this for initialization    
    public void Start()
    {
        transform.hasChanged = false;
        player = GameObject.FindGameObjectWithTag("Player");
        movementAccessor = player.GetComponent<PlayerController>();
        playerAnimator = movementAccessor.GetComponent<PlayerController>().spriteAnimator;
    }

    private void Update()
    {
    }

    void LateUpdate()
    {

        if (movementAccessor.movingForAnimation)
        {
            if (animationDelayObject != null)
                StopCoroutine(animationDelayObject);
            animationDelayObject = null;
            playerAnimator.SetBool("Idle", false);
            playerAnimator.SetBool("Moving", true);

        }
        else if (!movementAccessor.movingForAnimation && animationDelayObject == null)
        {
            playerAnimator.SetBool("Moving", false);
            animationDelayObject = StartCoroutine("AnimationDelay");
        }

        if (!movementAccessor.movingForAnimation)
        {
            if (movementAccessor.animationDirection == 1) // North
            {
                playerAnimator.SetInteger("PlayerDirection", 1);
            }
            else if (movementAccessor.animationDirection == 2) // East
            {
                playerAnimator.SetInteger("PlayerDirection", 2);
            }
            else if (movementAccessor.animationDirection == 3) // South
            {
                playerAnimator.SetInteger("PlayerDirection", 3);
            }
            else if (movementAccessor.animationDirection == 4) // West
            {
                playerAnimator.SetInteger("PlayerDirection", 4);
            }
        }
        else
        {
            playerAnimator.SetBool("Moving", true);
        }
        movementAccessor.movingForAnimation = false;
    }

        IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(3.0f);
        playerAnimator.SetBool("Idle", true);
    }
}
