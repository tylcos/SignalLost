using Prime31;
using UnityEngine;



[RequireComponent(typeof(CircleCollider2D))]
public class PushingCollider : MonoBehaviour {

    public float strength = 0.1f;
    public CharacterController2D parent;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        Vector2 translate;
        Transform opponent = other.transform;
        translate = transform.position - opponent.position;
        translate.Normalize();
        parent.move(translate * strength);
    }
}
