using UnityEngine;



public class Spin : MonoBehaviour 
{
    public float spinSpeed = 360;



    void Start()
    {
        spinSpeed *= (Random.Range(0, 2) * 2) - 1;
    }

    void Update() 
	{
        transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
    }
}
