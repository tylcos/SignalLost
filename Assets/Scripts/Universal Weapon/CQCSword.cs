using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CQCSword : CQC
{
    [Tooltip("Damaging collider")]
    public Collider2D hitter = null;
    private EquippedWeapon mom = null;
    private MovementController source = null; // Source character
    private Coroutine atk = null; // Running attack routine or null
    private float damage = 0; // Damage to deal on the next hit

    /// <summary>
    /// Initialize this <c>Sword</c>'s values.
    /// </summary>
    /// <param name="wep">The <c>EquippedWeapon</c> that represents this <c>CQC</c>.</param>
    /// <param name="source">The Source character.</param>
    /// <param name="layer">The layer to use for collision detection.</param>
    public override void Initialize(EquippedWeapon wep, MovementController source, int layer)
    {
        mom = wep;
        this.source = source;
        hitter.enabled = false;
        gameObject.layer = layer;
    }

    /// <summary>
    /// Attack using this sword.
    /// </summary>
    /// <param name="damage">Damage to deal with this attack.</param>
    public override void Attack(float damage)
    {
        this.damage = damage;
        atk = StartCoroutine(BigStick());
    }

    /// <summary>
    /// Uses this <c>CQCSword</c>'s collider to hit enemies.
    /// </summary>
    private IEnumerator BigStick()
    {
        hitter.enabled = true;
        yield return new WaitForSeconds(1);
        hitter.enabled = false;
        mom.EndSwing();
    }

    /// <summary>
    /// Cancel the currently running attack.
    /// </summary>
    public override void CancelAttack()
    {
        if (atk != null)
        {
            StopCoroutine(atk);
            hitter.enabled = false;
            mom.EndSwing();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MovementController>() != null)
        {
            bool destroyed = collision.GetComponent<MovementController>().OnHitReceived(source, damage);
            source.OnHitDealt(collision.GetComponent<MovementController>(), destroyed);
        }
    }
}
