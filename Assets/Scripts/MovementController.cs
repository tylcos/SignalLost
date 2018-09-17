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
        Vector2 bcSize = rb2d.gameObject.GetComponent<BoxCollider2D>().size;

        float vert = moveVector.y;
        float horz = moveVector.x;

        
        RaycastHit2D hitVert = Physics2D.Raycast(origin, new Vector2(0, moveVector.y), moveVector.magnitude + bcSize.y, LayerMask.GetMask("Walls"));
        if (hitVert.collider != null)
        {
            vert = hitVert.distance - bcSize.y;
            if(vert < 0)
            {
                vert = 0;
            }
        }

        RaycastHit2D hitHorz = Physics2D.Raycast(origin, new Vector2(moveVector.x, 0), moveVector.magnitude + bcSize.x, LayerMask.GetMask("Walls"));
        if (hitHorz.collider != null)
        {
            horz = hitHorz.distance - bcSize.x;
            if (horz < 0)
            {
                horz = 0;
            }
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
