using System.Linq;
using TMPro;
using UnityEngine;



public class MenuManager : MonoBehaviour
{
    public GameObject[] menuItems;
    public Canvas canvas;



    private UIRect[] bounds;
    private TextMeshProUGUI[] textItems;



    private int currentSelectedItem = -1;



    void Start()
    {
        bounds = menuItems.Select(m => new UIRect(m.GetComponent<RectTransform>())).ToArray();
        textItems = menuItems.Select(m => m.GetComponent<TextMeshProUGUI>()).ToArray();



        LeaderboardManager.LoadLeaderboardEntries();
        // Update some ui thingy that shows leaderboard
        // Possibly subscribe some thingy to call AddCurrentRun when player dies
    }



    void Update()
    {
        Vector2 mousePos = RectTransformUtility.PixelAdjustPoint(Input.mousePosition, transform, canvas);
        int selected = -1;

        for (int i = 0; i < menuItems.Length; i++)
        {
            if (bounds[i].Contains(mousePos))
            {
                selected = i;
                textItems[i].color = Color.gray;
            }
            else
                textItems[i].color = Color.white;
        }
        currentSelectedItem = selected;



        if (Input.GetAxis("Fire1") > 0)
            MenuSelect();

        float menuMove = GameController.GetMovementVector().y + GameController.GetAimingVector().y; Debug.Log((int)menuMove);
        if (menuMove > 0)
            currentSelectedItem = (currentSelectedItem + (int)menuMove) % menuItems.Length;
    }



    public void MenuSelect()
    {
        switch (currentSelectedItem)
        {
            // Start new game
            case 0:
                // Trigger load level
                break;

            // Options
            case 1:
                break;

            // Exit Game
            case 2:
                GameController.QuitApplication();
                break;
        }
    }
}



public readonly struct UIRect
{
    public readonly Vector2 Min;
    public readonly Vector2 Max;



    public UIRect(RectTransform rect)
    {
        Min = rect.anchoredPosition;
        Max = Min + rect.rect.size;
    }



    public bool Contains(Vector2 point)
    {
        return point.x < Max.x && point.y < Max.y && point.x > Min.x && point.y > Min.y;
    }
}