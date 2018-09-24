using UnityEngine;

public class HealthbarController : MonoBehaviour
{
    public GameObject background;
    public GameObject foreground;

    public CharacterController boundCharacter;



    private float current;
    private float max;

    private Sprite fgSprite;
    private Sprite bgSprite;



    void Start ()
    {
        fgSprite = foreground.GetComponent<SpriteRenderer>().sprite;
        bgSprite = background.GetComponent<SpriteRenderer>().sprite;

        Vector3 pos = background.transform.position;
        foreground.transform.position = new Vector3(pos.x - bgSprite.bounds.size.x, pos.y, pos.z);

        max = boundCharacter.MaxHealth;
        background.SetActive(false);
    }



	void Update ()
    {
        current = boundCharacter.Health / max;

        if (Mathf.Abs(current - 1f) > .01f)
        {
            background.SetActive(true);
            foreground.transform.localScale = new Vector3(current, foreground.transform.localScale.y, foreground.transform.localScale.z);
        }
    }
}
