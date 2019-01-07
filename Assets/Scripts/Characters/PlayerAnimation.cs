using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public Animator animator;
    [HideInInspector]
    public PlayerController playerControllerAccessor;

	// Use this for initialization    
        public void Start()
        {
            transform.hasChanged = false;
        }    

	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) 
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("Moving", true);
            if (Input.GetKeyDown(KeyCode.V))
            {
                MeleeAnimation();
            }
        }
        else
        {
            transform.hasChanged = false;
            animator.SetBool("Moving", false);
        }
        MeleeAnimation();
    }



    private void MeleeAnimation()
    {
        // TODO?: 
        //Is input key good? Will the animation for moving be fine with this
        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetBool("ClickedMeleeButton", true);
        }
        else
        {
            animator.SetBool("ClickedMeleeButton", false);
        }
    }
}
