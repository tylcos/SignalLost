using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PushingCollider : MonoBehaviour {

    public float strength = 0.1f;
    public Rigidbody2D parent;

    // better idea. have a fixedupdate or something that uses overlap to determine what colliders its in then moves away on a net vector
    // ontriggerstay should actually accomplish this
    private void OnTriggerStay2D(Collider other)
    {
        Vector2 translate;
        Transform opponent = other.transform;
        translate = transform.position - opponent.position;
        translate.Normalize();
        parent.MovePosition(translate * strength);
    }
}
