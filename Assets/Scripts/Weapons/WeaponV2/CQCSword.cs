using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CQCSword : CQC
{
    public Collider2D hitter = null;
    public EquippedWeapon mom = null;
    public MovementController source = null;
    public Coroutine atk = null;
    public float damage = 0;

    public override void Initialize(EquippedWeapon wep, MovementController source, int layer)
    {
        mom = wep;
        this.source = source;
        hitter.enabled = false;
        gameObject.layer = layer;
    }

    public override void Attack(float damage)
    {
        atk = StartCoroutine(BigStick());
        this.damage = damage;
    }

    private IEnumerator BigStick()
    {
        hitter.enabled = true;
        yield return new WaitForSeconds(1);
        hitter.enabled = false;
        mom.EndSwing();
    }

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
        if(collision.GetComponent<MovementController>() != null)
        {
            bool destroyed = collision.GetComponent<MovementController>().OnHitReceived(source, damage);
            source.OnHitDealt(collision.GetComponent<MovementController>(), destroyed);
        }
    }
}
