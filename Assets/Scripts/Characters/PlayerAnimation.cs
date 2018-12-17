using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public Animator animator;
    [HideInInspector]
    public PlayerController playerControllerAccessor;

	// Use this for initialization
	void Start () {
        transform.hasChanged = false;
	}

    /*
        public void Start()
        {
            StartCoroutine(Example_WaitForSeconds_Coroutine());
        }

        public IEnumerator Example_WaitForSeconds_Coroutine()
        {
            Debug.Log("Start of WaitForSeconds Example");

            yield return new WaitForSeconds(1.5f);

            Debug.Log("End of WaitForSeconds Example");
        }
    */

    //TESTING
    public IEnumerator TestCoroutine()
    {
        while (true)
        {
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
                yield return new WaitForSeconds(2f);
                animator.SetBool("Moving", false);
                transform.hasChanged = false;
            }
            MeleeAnimation();
        }
    }
    //END

	// Update is called once per frame
	void Update () {
        //TESTING
       // StartCoroutine(TestCoroutine());
        //END
        
        /*
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
            animator.SetBool("Moving", false);
            transform.hasChanged = false;

        }
        MeleeAnimation();*/
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
