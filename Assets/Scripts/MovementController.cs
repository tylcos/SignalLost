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

    /// <summary>
    ///     Moves the rigidbody to position with wall collision.
    /// </summary>
    /// <remark>
    ///    This simply uses the Rigidbody2D.MovePosition function to move the gameObject after calculating collision.
    /// </remark>
    ///     <param name="rb2d">The RigidBody2D object.</param>
    ///     <param name="origin">The origin of the movement vector.</param>
    ///     <param name="moveVector">The movement vector to use.</param>
    protected void Move(Rigidbody2D rb2d, Vector2 origin, Vector2 moveVector)
    {
        Vector2 bcSize = rb2d.gameObject.GetComponent<BoxCollider2D>().size;

        float vert = moveVector.y;
        float horz = moveVector.x;

        RaycastHit2D[] hitVert = new RaycastHit2D[2];
        int hitVertCount = Physics2D.RaycastNonAlloc(origin, new Vector2(0, moveVector.y), hitVert, moveVector.magnitude + bcSize.y, LayerMask.GetMask("Walls", "Player", "Enemy"));
        foreach (RaycastHit2D hit in hitVert) {
            if (hit.collider != null && hit.fraction != 0)
            {
                if (moveVector.y > 0)
                {
                    vert = hit.distance - bcSize.y;
                    if (vert < 0)
                    {
                        vert = 0;
                    }
                }
                else if (moveVector.y < 0)
                {
                    vert = -hit.distance + bcSize.y;
                    if (vert > 0)
                    {
                        vert = 0;
                    }
                }
                break;
            }
        }

        RaycastHit2D[] hitHorz = new RaycastHit2D[2];
        int hitHorzCount = Physics2D.RaycastNonAlloc(origin, new Vector2(moveVector.x, 0), hitHorz, moveVector.magnitude + bcSize.x, LayerMask.GetMask("Walls", "Player", "Enemy"));
        foreach (RaycastHit2D hit in hitHorz)
        {
            if (hit.collider != null && hit.fraction != 0)
            {
                if (moveVector.x > 0)
                {
                    horz = hit.distance - bcSize.x;
                    if (horz < 0)
                    {
                        horz = 0;
                    }
                }
                else if (moveVector.x < 0)
                {
                    horz = -hit.distance + bcSize.x;
                    if (horz > 0)
                    {
                        horz = 0;
                    }
                }
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
