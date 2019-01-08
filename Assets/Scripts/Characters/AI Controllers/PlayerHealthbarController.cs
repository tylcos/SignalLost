using UnityEngine;

public class PlayerHealthbarController : MonoBehaviour
{
    //public GameObject background;
    public GameObject foreground;

    public MovementController boundCharacter;

    private float current;
    private float max;

    private Sprite fgSprite;
    private Sprite bgSprite;



    /*void Start ()
    {
        fgSprite = foreground.GetComponent<SpriteRenderer>().sprite;
        bgSprite = background.GetComponent<SpriteRenderer>().sprite;

        Vector3 pos = background.transform.position;
        foreground.transform.position = new Vector3(pos.x - bgSprite.bounds.size.x, pos.y, pos.z);

        max = boundCharacter.MaxHitPoints;
        background.SetActive(false);
    }*/



    /*void Update ()
    {
        current = boundCharacter.CurrentHitPoints / max;

        if (Mathf.Abs(current - 1f) > .01f)
        {
            background.SetActive(true);
            foreground.transform.localScale = new Vector3(current, foreground.transform.localScale.y, foreground.transform.localScale.z);
        }
    }*/

    private void Start()
    {
        max = boundCharacter.MaxHitPoints;
    }

    private void Update()
    {
        current = boundCharacter.CurrentHitPoints / max;
        foreground.transform.localScale = new Vector3(current, foreground.transform.localScale.y, foreground.transform.localScale.z);
    }
}
