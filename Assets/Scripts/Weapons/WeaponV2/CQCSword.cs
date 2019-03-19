using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CQCSword : CQC
{
    public Collider2D hitter;
    private EquippedWeapon mom;
    private MovementController source;
    private Coroutine atk = null;

    public override void Initialize(EquippedWeapon wep, MovementController source, int layer)
    {
        mom = wep;
        this.source = source;
        hitter.enabled = false;
        gameObject.layer = layer;
    }

    public override void Attack()
    {
        atk = StartCoroutine(BigStick());
    }

    private IEnumerator BigStick()
    {
        hitter.enabled = true;
        yield return new WaitForSeconds(1);
        hitter.enabled = false;
    }

    public override void CancelAttack()
    {
        if (atk != null)
        {
            StopCoroutine(atk);
            hitter.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<MovementController>() != null)
        {
            bool destroyed = collision.GetComponent<MovementController>().OnHitReceived(source, 10);
            source.OnHitDealt(collision.GetComponent<MovementController>(), destroyed);
        }
    }
}
