using UnityEngine;

public class HealthbarController : MonoBehaviour
{
    private float current;
    private float max;
    public GameObject background;
    public GameObject foreground;
    private Sprite fgSprite;
    private Sprite bgSprite;
    public Character boundCharacter;
    public Weapon data;



    void Start ()
    {
        fgSprite = foreground.GetComponent<SpriteRenderer>().sprite;
        bgSprite = background.GetComponent<SpriteRenderer>().sprite;

        Vector3 pos = background.transform.position;
        foreground.transform.position = new Vector3(pos.x - bgSprite.bounds.size.x, pos.y, pos.z);
	}



	void Update ()
    {
        current = boundCharacter.Health;
        max = boundCharacter.MaxHealth;
        foreground.transform.localScale = new Vector3(current / max, foreground.transform.localScale.y, foreground.transform.localScale.z);
    }
}
