using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponAnimation : MonoBehaviour
{
    [HideInInspector]


    private Animator weaponAnimator;




    // Start is called before the first frame update
    void Start()
    {
        weaponAnimator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Debug.Log(weaponAnimator);

    }
}
