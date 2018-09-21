using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarController : MonoBehaviour {
    private float current;
    private float max;
    public GameObject background;
    public GameObject foreground;
    private Sprite fgSprite;
    private Sprite bgSprite;
    public Character boundCharacter;

	// Use this for initialization
	void Start () {
        fgSprite = foreground.GetComponent<SpriteRenderer>().sprite;
        bgSprite = background.GetComponent<SpriteRenderer>().sprite;
        Vector3 pos = background.transform.position;
        foreground.transform.position = new Vector3(pos.x - bgSprite.bounds.size.x, pos.y, pos.z);
	}
	
	// Update is called once per frame
	void Update ()
    {
        current = boundCharacter.CurrentHealth;
        max = boundCharacter.MaxHealth;
        foreground.transform.localScale = new Vector3(current / max, foreground.transform.localScale.y, foreground.transform.localScale.z);
	}
}
