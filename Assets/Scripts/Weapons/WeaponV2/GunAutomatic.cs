using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAutomatic : Gun
{
    private Coroutine rel;
    private EquippedWeapon dad;

    public override void Initialize(EquippedWeapon wep)
    {
        dad = wep;
    }

    public override void CancelReload()
    {
        if(rel != null)
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
        float start = Time.time;
        //dad.reloading = true;
        //dad.reloadProgress = 0;
        do
        {
            yield return new WaitForEndOfFrame();
            dad.reloadProgress = (Time.time - start) / duration; 
        } while (dad.reloadProgress < duration);
        //dad.reloadProgress = 1;
        //dad.reloading = false;
        dad.FillMag();
        Debug.Log("Filled mag");
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
