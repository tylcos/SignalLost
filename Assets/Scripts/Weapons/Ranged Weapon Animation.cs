using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponAnimation : PlayerAnimation
{
    [HideInInspector]
    public GameObject weapon;

    private PlayerWeaponController pWC;

   
    // Start is called before the first frame update
    void Start()
    {
        pWC = this.GetComponentInChildren<PlayerWeaponController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Debug.Log(pWC);
    }
}
