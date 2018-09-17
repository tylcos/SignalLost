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
        // recode this so that it first calculates how far the x goes then how far the y goes so that
        // if there's a wall right in front of me but i'm pressing w and d i still move to the right, just not forward

        float vert = 0f;
        float horz = 0f;

        RaycastHit2D hitVert = Physics2D.Raycast(origin, new Vector2(0, moveVector.y), moveVector.magnitude, LayerMask.GetMask("Walls"));
        if (hitVert.collider != null)
        {
            RaycastHit2D hitBack = Physics2D.Raycast(hitVert.point, -moveVector, moveVector.magnitude, LayerMask.GetMask("Player"));
            vert = hitBack.distance;
        }
        else
        {
            vert = moveVector.y;
        }
        RaycastHit2D hitHorz = Physics2D.Raycast(origin, new Vector2(moveVector.x, 0), moveVector.magnitude, LayerMask.GetMask("Walls"));
        if (hitHorz.collider != null)
        {
            RaycastHit2D hitBack = Physics2D.Raycast(hitHorz.point, -moveVector, moveVector.magnitude, LayerMask.GetMask("Player"));
            horz = hitBack.distance;
        }
        else
        {
            horz = moveVector.x;
        }

        Vector2 finalMoveVector = new Vector2(horz, vert);
        rb2d.MovePosition(finalMoveVector + origin);

        /*
        RaycastHit2D hit = Physics2D.Raycast(origin, moveVector, moveVector.magnitude, LayerMask.GetMask("Walls"));
        if(hit.collider != null)
        {
            RaycastHit2D hitBack = Physics2D.Raycast(hit.point, -moveVector, moveVector.magnitude, LayerMask.GetMask("Player"));
            Vector2 subtractVector = (moveVector.normalized * (hitBack.distance));
            rb2d.MovePosition(subtractVector + origin);
        } else
        {
            rb2d.MovePosition(moveVector + origin);
        }*/
    }
}
