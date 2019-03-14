using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Collider2D hitbox;
    private Coroutine atk;
    private EquippedWeapon mom;
    private MovementController source = null;
    private float damageToDealThisFrame;

    public void Initialize(EquippedWeapon e, int layer, MovementController source)
    {
        mom = e;
        hitbox.enabled = false;
        gameObject.layer = layer;
        this.source = source;
    }

    public void CancelAttack()
    {
        if (atk != null)
            StopCoroutine(atk);
    }

    public void Attack(float damage)
    {
        print(damage);
        damageToDealThisFrame = damage;
        atk = StartCoroutine(Attacking());
    }

    private IEnumerator Attacking()
    {
        hitbox.enabled = true;
        yield return new WaitForEndOfFrame();
        // we could also waituntil(animation.complete) or whatever the code would be when we throw and animation in here
        hitbox.enabled = false;
        mom.EndSwing();
    }

    void OnTriggerEnter2D(Collider2D collEvent)
    {
        if (collEvent.gameObject.GetComponent<MovementController>() != null)
        {
            print("hit" + damageToDealThisFrame);
            bool killedCharacterHit = collEvent.gameObject.GetComponent<MovementController>().OnHitByOpponent(source, damageToDealThisFrame);
            source.OnHitOpponent(collEvent.gameObject.GetComponent<MovementController>(), killedCharacterHit);
        }
    }
}
