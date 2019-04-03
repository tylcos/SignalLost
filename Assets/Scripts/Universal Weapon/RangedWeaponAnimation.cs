using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponAnimation : MonoBehaviour
{
    [HideInInspector]


    private Animator weaponAnimator;
    private GunAutomatic gunAutomaticRef;

    // Start is called before the first frame update
    void Start()
    {
        weaponAnimator = this.GetComponent<Animator>();
        gunAutomaticRef = this.GetComponent<GunAutomatic>();
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (gunAutomaticRef.fireForAnim)
        {
            weaponAnimator.SetBool("Shot", true);
            gunAutomaticRef.fireForAnim = false;
        }
        else
        {
            weaponAnimator.SetBool("Shot", false);
        }


        /*weaponAnimator.SetBool("Shot", false);
        gunAutomaticRef.fireForAnim = false;*/

        /*  else
          {
              weaponAnimator.SetBool("Shot", false);
              gunAutomaticRef.fireForAnim - false;;
          }*/
    }
}
