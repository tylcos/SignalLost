using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void Move(Rigidbody2D rb2d, Vector2 origin, Vector2 moveVector)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, moveVector.normalized, moveVector.magnitude, LayerMask.GetMask("Walls"));
        if(hit.collider != null && (hit.fraction != 0 || hit.normal == -moveVector.normalized))
        {
            Vector2 subtractVector = (moveVector.normalized * hit.distance);
            rb2d.MovePosition(subtractVector + origin);
        } else
        {
            rb2d.MovePosition(moveVector + origin);
        }
    }
}
