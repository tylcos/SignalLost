using UnityEngine;

<<<<<<< HEAD
public class bulletCollider : MonoBehaviour
{
=======
public class BulletCollider : MonoBehaviour
{
    [HideInInspector]
>>>>>>> 8d6d3db636889f4893d8002191cc2f8ebccc2912
    public float lifeTime;



<<<<<<< HEAD
    private float time;
=======
    private float startTime;
>>>>>>> 8d6d3db636889f4893d8002191cc2f8ebccc2912



    void Start()
    {
<<<<<<< HEAD
        time = Time.time;

=======
        startTime = Time.time;
>>>>>>> 8d6d3db636889f4893d8002191cc2f8ebccc2912
    }


	
	void LateUpdate()
    {
<<<<<<< HEAD
		if (Time.time - time >= lifeTime)
=======
		if (Time.time - startTime >= lifeTime)
>>>>>>> 8d6d3db636889f4893d8002191cc2f8ebccc2912
            Destroy(gameObject);
	}

    void OnCollisionEnter2D(Collision2D collEvent)
    {
<<<<<<< HEAD
            Destroy(gameObject);
=======
        Destroy(gameObject);
>>>>>>> 8d6d3db636889f4893d8002191cc2f8ebccc2912
    }
}
