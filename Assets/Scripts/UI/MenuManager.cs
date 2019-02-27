using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuManager : MonoBehaviour
{
    public Transform menuItems;
    public string SceneLoadName;
    public Canvas canvas;



    private TextMeshProUGUI[] textItems;



    private int currentSelectedItem;
    private bool keyFirstPressed;
    private bool keyReleased = true;



    void Start()
    {
        textItems = menuItems.GetComponentsInChildren<TextMeshProUGUI>();
        textItems[0].color = Color.gray;



        LeaderboardManager.LoadLeaderboardEntries();
        // Update some ui thingy that shows leaderboard
        // Possibly subscribe some thingy to call AddCurrentRun when player dies



        for (int i = 9; i >= LeaderboardManager.leaderboardEntries.Count; i--)
            textItems[i].enabled = false;

        for (int i = 0; i < LeaderboardManager.leaderboardEntries.Count; i++)
            textItems[i].text = LeaderboardManager.leaderboardEntries[i].ToString();
    }



    void Update()
    {
        int menuMove = -(int)(GameController.GetMovementVector().y + GameController.GetAimingVector().y);
        keyFirstPressed = menuMove != 0 && keyReleased;
        keyReleased = menuMove == 0;

        if (keyFirstPressed)
        {
            textItems[currentSelectedItem].color = Color.white;

            currentSelectedItem = (currentSelectedItem + menuMove) % textItems.Length;
            if (currentSelectedItem < 0)
                currentSelectedItem = textItems.Length - 1;

            textItems[currentSelectedItem].color = Color.gray;
        }



        if (Input.GetAxis("Fire1") > 0 || Input.GetKeyDown(KeyCode.Return))
            MenuSelect();
    }



    public void MenuSelect()
    {
        switch (currentSelectedItem)
        {
            // Start new game
            case 0:
                SceneManager.LoadSceneAsync(SceneLoadName);
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
