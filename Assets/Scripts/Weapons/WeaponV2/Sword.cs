using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Collider2D hitbox;
    private Coroutine atk;
    private EquippedWeapon mom;
    private MovementController owner;
    private float damageToDealThisFrame;

    public void Initialize(EquippedWeapon e, int layer, MovementController parent)
    {
        mom = e;
        hitbox.enabled = false;
        gameObject.layer = layer;
        owner = parent;
        print("Intialized to " + owner.ToString());
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
        print("Attacking as " + owner.ToString());
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
            print(mom);
            /*MovementController man = collEvent.gameObject.GetComponent<MovementController>();
            print("Object hit: " + man);
            print("This sword's parent: " + owner);
            print("hit: " + damageToDealThisFrame);
            bool killedCharacterHit = man.OnHitReceived(owner, damageToDealThisFrame);
            owner.OnHitDealt(man, killedCharacterHit);*/
        }
    }
}
