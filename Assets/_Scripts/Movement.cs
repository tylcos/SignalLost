using UnityEngine;



public class Movement : MonoBehaviour
{
    public float speed = 5f;

    public CharacterController cc;



    void FixedUpdate()
    {
        Vector3 move = new Vector3();

        move += Vector3.up    * Input.GetAxisRaw("Vertical");
        move += Vector3.right * Input.GetAxisRaw("Horizontal");

        cc.Move(move.normalized * speed * Time.deltaTime);
    }
}
