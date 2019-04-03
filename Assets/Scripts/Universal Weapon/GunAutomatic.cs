using System.Collections.Generic;
using UnityEngine;

public class GunAutomatic : Gun
{
    private Coroutine rel; // active reload routine or null
    private EquippedWeapon dad;
    public bool fireForAnim;

    /// <summary>
    /// Initialize this gun's values.
    /// </summary>
    /// <param name="wep">The <c>EquippedWeapon</c> that represents this <c>Gun</c>.</param>
    public override void Initialize(EquippedWeapon wep)
    {
        dad = wep;
    }

    /// <summary>
    /// Cancel the current reload progress.
    /// </summary>
    public override void CancelReload()
    {
        if (rel != null)
            StopCoroutine(rel);
    }

    /// <summary>
    /// Called when the <c>EquippedWeapon</c> fires.
    /// </summary>
    /// <param name="direction">The direction the weapon fired in.</param>
    public override void Fire(Vector2 direction)
    {
        fireForAnim = true;
    }

    /// <summary>
    /// Reloads the gun.
    /// </summary>
    /// <param name="time">The duration of the reload.</param>
    public override void Reload(float time)
    {
        rel = StartCoroutine(Reloading(time));
    }

    /// <summary>
    /// Reloads the gun over time.
    /// </summary>
    /// <param name="duration">How long reloading takes.</param>
    private IEnumerator<WaitForEndOfFrame> Reloading(float duration)
    {
        float start = Time.time;
        do
        {
            yield return new WaitForEndOfFrame();
            dad.reloadProgress = (Time.time - start) / duration;
        } while (dad.reloadProgress < duration);

        dad.FillMag();
        Debug.Log("Filled mag");
    }
}
