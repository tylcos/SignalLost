using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Collider2D hitbox;
    private Coroutine atk;
    private EquippedWeapon mom;
    private MovementController source;
    private float damageToDealThisFrame;

    public void Initialize(EquippedWeapon e, int layer, MovementController parent)
    {
        mom = e;
        hitbox.enabled = false;
        gameObject.layer = layer;
        source = parent;
    }

    public void CancelAttack()
    {
        if (atk != null)
            StopCoroutine(atk);
    }

    public void Attack(float damage)
    {
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

    // for some reason source and damagetodeal don't exist here
    // give movement controller a tostring that prints out if its a player or enemy
    void OnTriggerEnter2D(Collider2D collEvent)
    {
        if (collEvent.gameObject.GetComponent<MovementController>() != null)
        {
            MovementController man = collEvent.gameObject.GetComponent<MovementController>();
            print("hit: " + damageToDealThisFrame);
            bool killedCharacterHit = man.OnHitReceived(source, damageToDealThisFrame);
            source.OnHitDealt(man, killedCharacterHit);
        }
    }
}
