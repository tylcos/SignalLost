﻿using System.Collections;
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

    // Update is called once per frame
    private void Update()
    {

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
        /*
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) 
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("Moving", true);
            idle = false;
            if (Input.GetKeyDown(KeyCode.V))
            {
                MeleeAnimation();
            }
            //Setup for Shooting animation
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ShootingAnimation();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ReloadingAnimation();
            }
        }
        else
        {
            idle = true;
        }

        MeleeAnimation();
        ShootingAnimation();
        ReloadingAnimation();

        if (idle)
        {
            transform.hasChanged = false;
            animator.SetBool("Moving", false);
        }
        */
    /*
    private void MeleeAnimation()
    {
        // TODO?: 
        //Is input key good? Will the animation for moving be fine with this
        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetBool("ClickedMeleeButton", true);
            idle = false;
        }
        else
        {
            animator.SetBool("ClickedMeleeButton", false);
        }
    }
    */

    /*
    private void ShootingAnimation()
    {
        if (Input.GetKey(KeyCode.Mouse0)&& weaponAccessor.Weapon.CurrentAmmo!=0)
        {
            animator.SetBool("Shooting", true);
            idle = false;
            Debug.Log(idle);
        }
        else
        {
            animator.SetBool("Shooting", false);
        }
    }
    */

    /*
    private void ReloadingAnimation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            idle = false;
            animator.SetBool("Reloading", true);
            animator.SetBool("Shooting", false);
        }
        else
        {
            animator.SetBool("Reloading", false);
        }
    }
    */
}
