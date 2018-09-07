﻿using UnityEngine;

public class BulletCollider : MonoBehaviour
{
    public float lifeTime;



    private float time;



    void Start()
    {
        time = Time.time;
	}


	
	void LateUpdate()
    {
		if (Time.time - time >= lifeTime)
            Destroy(gameObject);
	}

    void OnCollisionEnter2D(Collision2D collEvent)
    {
        if (collEvent.gameObject.tag != "NoDestroy")
            Destroy(gameObject);
    }
}
