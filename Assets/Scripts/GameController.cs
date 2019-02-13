using System;
using UnityEngine;



public class GameController : MonoBehaviour
{
    [HideInInspector]
    public static string inputMethod = "keyboard";



    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        var inputArg = GetArg("-inputMethod");
        switch(inputArg)
        {
            case "keyboard":
            case "arcade":
                inputMethod = inputArg;
                break;
        }
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



    public static Vector2 GetMovementVector()
    {
        if (inputMethod == "keyboard")
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        else if (inputMethod == "arcade")
            return new Vector2(Input.GetAxisRaw("HorizontalArcade"), Input.GetAxisRaw("VerticalArcade"));
        
        return Vector2.zero;
    }

    public static Vector2 GetAimingVector()
    {
        if (inputMethod == "keyboard")
            return new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));
        else if (inputMethod == "arcade")
            return new Vector2(Input.GetAxisRaw("HorizontalKeysArcade"), Input.GetAxisRaw("VerticalKeysArcade"));
        
        return Vector2.zero;
    }
    



    public static void QuitApplication()
    {
        LeaderboardManager.SaveLeaderboardEntries();

        Debug.Log("Exiting Game");
        Application.Quit();
    }
}
