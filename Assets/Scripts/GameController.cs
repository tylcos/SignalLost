using System;
using UnityEngine;



public class GameController : MonoBehaviour
{
    [HideInInspector]
    public string inputMethod;

    

    void OnEnable()
    {
        var inputArg = GetArg("-inputMethod");
        switch(inputArg)
        {
            case "keyboard":
            case "arcade":
                inputMethod = inputArg;
                break;
            default:
                inputMethod = "keyboard";
                break;
        }



        LeaderboardManager.LoadLeaderboardEntries();
        // Update some ui thingy that shows leaderboard
        // Possibly subscribe some thingy to call AddCurrentRun when player dies
    }

    void Update()
    {
        if (Input.GetAxis("ArcadeExit") > 0)
            QuitApplication();
    }



    public static string GetArg(string name)
    {
        var args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }

        return null;
    }



    public static void QuitApplication()
    {
        LeaderboardManager.SaveLeaderboardEntries();

        Debug.Log("Exiting Game");
        Application.Quit();
    }
}
