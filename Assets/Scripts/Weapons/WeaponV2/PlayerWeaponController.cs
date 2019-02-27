using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : WeaponController
{
    private const int INVSIZE = 4;
    [SerializeField]
    public WeaponV2Information[] inventory = new WeaponV2Information[INVSIZE];
    private GameController master;
    private List<EquippedWeapon> swapList = new List<EquippedWeapon>(INVSIZE);
    private int swapListIndex = 0;

    public delegate void PWCReloadHandler(bool reloading, float progress);
    public event PWCReloadHandler ReloadUpdate;
    public delegate void PWCFireError();
    public event PWCFireError FireError;

    private void OnValidate()
    {
        if(inventory.Length != INVSIZE)
        {
            Debug.LogWarning("Don't change the weapon inventory array size!");
            System.Array.Resize(ref inventory, INVSIZE);
        }
    }

    private void OnEnable()
    {
        master = GameObject.FindGameObjectWithTag("Master").GetComponent<GameController>();
        // populates the swaplist
        foreach (WeaponV2Information wep in inventory)
        {
            // create a class that stores a reference to the object we 
            if(wep == null)
                swapList.Add(EquippedWeapon.empty);
            else
                swapList.Add(new EquippedWeapon(wep, transform, bulletLayer));
            // so we have a parallel array that holds the objects, their bullets, and ammo
            // so we call commands there when we need to fire or whatnot since that stores everything
        }
        swapList[0].SetEnabled(true);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

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
        // on R press, reload. If arcade, on P2 press, reload, one P2 hold, open weapon wheel
        else /*if (swapList[swapListIndex].CanFire())*/
        {
            Vector2 shootDir = Vector2.zero;
            if (master.inputMethod == "keyboard")
            {
                shootDir = new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));
            }
            else if (master.inputMethod == "arcade")
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

    public EquippedWeapon GetEquippedWeapon()
    {
        return swapList[swapListIndex];
    }
}
