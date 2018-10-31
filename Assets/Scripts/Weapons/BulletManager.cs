using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float lifeTime;
    public float damage;
    public float maxCollisions;
    private float currentPenetration = 0;

    //public string[] collideLayers;
    //protected int collideLayerMask;
    //public string[] targetTags = { "Player", "Enemy", "Walls" };

    private float startTime;

    void Start()
    {
        //collideLayerMask = LayerMask.GetMask(collideLayers);
        startTime = Time.time;
    }
	
	void LateUpdate()
    {
		if (Time.time - startTime >= lifeTime)
            Destroy(gameObject);
	}

    void OnTriggerEnter2D(Collider2D collEvent)
    {
        /*foreach(string tag in targetTags)
        {
            if (collEvent.gameObject.CompareTag(tag))
            {
                //collEvent.gameObject.GetComponent<MovementController>().Damage(damage);
                currentPenetration++;
                break;
            }
        }*/
        currentPenetration++;
        if(currentPenetration >= maxCollisions)
        {
            Destroy(gameObject);
        }
    }
}
