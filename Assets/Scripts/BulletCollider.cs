using UnityEngine;

public class BulletCollider : MonoBehaviour
{
    [HideInInspector]
    public float lifeTime;



    private float startTime;



    void Start()
    {
        startTime = Time.time;
    }


	
	void LateUpdate()
    {
		if (Time.time - startTime >= lifeTime)
            Destroy(gameObject);
	}

    void OnCollisionEnter2D(Collision2D collEvent)
    {
        Destroy(gameObject);
    }
}
