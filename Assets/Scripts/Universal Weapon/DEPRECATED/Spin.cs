using UnityEngine;


[System.Obsolete("No longer used in the game. Using this will not crash the game.", false)]
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
