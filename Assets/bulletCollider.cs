using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCollider : MonoBehaviour {

    private float time;
	// Use this for initialization
	void Start () {
        time = Time.time;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Time.time - time >= 2.0f)
        {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        Destroy(gameObject);
    }
}
