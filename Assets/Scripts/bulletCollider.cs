using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCollider : MonoBehaviour {

    private float time;
    public float life;
	// Use this for initialization
	void Start () {
        time = Time.time;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Time.time - time >= life)
        {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        Destroy(gameObject);
    }
}
