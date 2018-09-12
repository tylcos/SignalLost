using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float speed;
    public int damage;
    public int health;
    public float aggroRange;
    public Rigidbody2D rb2d;
    public float stunLength;

    private GameObject target = null;

    private float internalSpeed;
    private bool stunned;
    private float stunStart;

    // Use this for initialization
    void Start()
    {
        internalSpeed = speed * 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(stunned && Time.time-stunStart > stunLength)
        {
            stunned = false;
        }
        if (target != null)
        {
            CheckForTarget(target);
        }
        else
        {
            AttemptFindNewTarget();
        }
    }

    private void FixedUpdate()
    {
        if (target != null && !stunned)
        {
            Vector2 move = new Vector2(target.transform.position.x - gameObject.transform.position.x, target.transform.position.y - gameObject.transform.position.y);
            rb2d.velocity = move.normalized * internalSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collEvent)
    {
        if (collEvent.gameObject.tag == "Player")
        {
            rb2d.velocity = -rb2d.velocity;
            stunStart = Time.time;
            stunned = true;
        }
    }

    private void CheckForTarget(GameObject target)
    {
        float x = target.transform.position.x;
        float y = target.transform.position.y;
        if (Mathf.Sqrt((Mathf.Pow(x, 2) + Mathf.Pow(y, 2))) > aggroRange)
        {
            target = null;
        }
    }

    private void AttemptFindNewTarget()
    {
        Collider2D overlap = Physics2D.OverlapCircle(gameObject.transform.position, aggroRange, LayerMask.GetMask("Player"));
        target = overlap.gameObject;
    }


}
