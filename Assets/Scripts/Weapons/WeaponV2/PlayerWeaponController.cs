using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : WeaponController
{
    public delegate void PWCReloadHandler(bool reloading, float progress);
    public event PWCReloadHandler ReloadUpdate;
    public delegate void PWCFireError();
    public event PWCFireError FireError;

    // Update is called once per frame
    void Update()
    {
        // To swap weapons on the arcade, pause the sim and pull up a radial selector and use joysticks to select
        if (Input.GetKeyDown(KeyCode.Alpha1) && swapListIndex != 0)
        {
            swapList[swapListIndex].SetEnabled(false);
            swapListIndex = 0;
            swapList[swapListIndex].SetEnabled(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && swapListIndex != 1)
        {
            swapList[swapListIndex].SetEnabled(false);
            swapListIndex = 1;
            swapList[swapListIndex].SetEnabled(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && swapListIndex != 2)
        {
            swapList[swapListIndex].SetEnabled(false);
            swapListIndex = 2;
            swapList[swapListIndex].SetEnabled(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && swapListIndex != 3)
        {
            swapList[swapListIndex].SetEnabled(false);
            swapListIndex = 3;
            swapList[swapListIndex].SetEnabled(true);
        }
        else if (master.SwapPressed())
        {
            swapList[swapListIndex].SetEnabled(false);
            swapListIndex++;
            if (swapListIndex == 4) swapListIndex = 0;
            swapList[swapListIndex].SetEnabled(true);
        } else if (master.ReloadPressed() && swapList[swapListIndex].CanReload())
        {
            swapList[swapListIndex].Reload();
            // on R press, reload. If arcade, on P2 press, reload, one P2 hold, open weapon wheel
        } else /*if (swapList[swapListIndex].CanFire())*/
        {
            Vector2 shootDir = Vector2.zero;
            if (GameController.inputMethod == "keyboard")
            {
                shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));
            }
            else if (GameController.inputMethod == "arcade")
            {
                shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeysArcade"), Input.GetAxisRaw("VerticalKeysArcade"));
            }

            if ((Input.GetAxis("Fire1") > 0 || shootDir.sqrMagnitude != 0))
            {
                if(swapList[swapListIndex].CanFire())
                {
                    swapList[swapListIndex].Fire(shootDir);
                } else if(swapList[swapListIndex].reloading)
                {
                    FireError();
                }
            }
        }

        // update reload bar if applicable
        if(swapList[swapListIndex].reloading)
        {
            ReloadUpdate(swapList[swapListIndex].reloading, swapList[swapListIndex].reloadProgress);
            // when reloading, put an indicator over players head that shows bullets and flashes red when you try to shoot
            // also put a bar above the ammo counter that fills up with respect to the reloadProgress
            // use events to do update the UI btw
        }
    }
}
