using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    public float damage;
    public float maxCollisions;
    private float currentPenetration = 0;
    [HideInInspector]
    public MovementController source = null;

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
        if (collEvent.gameObject.GetComponent<MovementController>() != null)
        {
            //collEvent.gameObject.GetComponent<MovementController>().Damage(damage);
            collEvent.gameObject.GetComponent<MovementController>().OnHitByOpponent(source, damage);
            source.OnHitOpponent(collEvent.gameObject.GetComponent<MovementController>());
        }
        currentPenetration++;
        if(currentPenetration >= maxCollisions)
            Destroy(gameObject);

        // damaging should be in an onhittaken method
        // and then landing a hit could be in an onhitdealt method
        // yeah this needs to be entirely rewritten
    }
}
