using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAutomatic : Gun
{
    Coroutine rel;
    EquippedWeapon dad;

    public override void Initialize(EquippedWeapon wep)
    {
        dad = wep;
    }

    public override void CancelReload()
    {
        StopCoroutine(rel);
    }

    public override void Fire(Vector2 direction)
    {
        throw new System.NotImplementedException();
    }

    public override void Reload(float time)
    {
        rel = StartCoroutine(Reloading(time));
    }

    private IEnumerator Reloading(float duration)
    {
        bool gamer = true;
        if(gamer)
        {
            yield return new WaitForSeconds(1);
            gamer = false;
        } else
        {
            dad.FillMag();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
