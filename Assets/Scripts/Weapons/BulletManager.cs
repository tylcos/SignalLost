using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float lifeTime;
    public float damage;


    
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

    void OnTriggerEnter2D(Collider2D collEvent)
    {
        Destroy(gameObject);
    }
}
