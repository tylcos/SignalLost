using System;
using UnityEngine;



public class DungeonGameManager : MonoBehaviour
{
    [HideInInspector]
    public static InputMethodType InputMethod = InputMethodType.Keyboard;
    


    [HideInInspector]
    public static int CurrentScore;
    


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        var inputArg = GetArg("-inputMethod");
        switch(inputArg)
        {
            case "keyboard":
                InputMethod = InputMethodType.Keyboard;
                Cursor.visible = true;
                break;
            case "arcade":
                InputMethod = InputMethodType.Arcade;
                Cursor.visible = false;
                break;
            default:
                InputMethod = InputMethodType.Keyboard;
                Cursor.visible = true;
                break;
        }
    }



    void Update()
    {
        if (Input.GetAxis("ArcadeExit") > 0)
            QuitApplication();

        if (Input.GetKeyDown(KeyCode.O)) // <o/
            LevelManager.LoadNewLevel();
    }



    public static string GetArg(string name)
    {
        var args = Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
            if (args[i] == name && args.Length > i + 1)
                return args[i + 1];

        return null;
    }



    public static Vector2 GetMovementVector()
    {
        if (InputMethod == InputMethodType.Keyboard)
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        else if (InputMethod == InputMethodType.Arcade)
            return new Vector2(Input.GetAxisRaw("HorizontalArcade"), Input.GetAxisRaw("VerticalArcade"));

        return Vector2.zero;
    }

    public static Vector2 GetAimingVector()
    {
        if (InputMethod == InputMethodType.Keyboard)
            return new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));
        else if (InputMethod == InputMethodType.Arcade)
            return new Vector2(Input.GetAxisRaw("HorizontalKeysArcade"), Input.GetAxisRaw("VerticalKeysArcade"));

        return Vector2.zero;
    }

    public static bool ReloadPressed()
    {
        return (InputMethod == InputMethodType.Keyboard) ? Input.GetKeyDown(KeyCode.R) : false;
    }

    public static bool SwapPressed()
    {
        if (InputMethod == InputMethodType.Keyboard)
            return Input.GetKeyDown(KeyCode.Tab);
        else if (InputMethod == InputMethodType.Arcade)
            return Input.GetKeyDown(KeyCode.Minus);

        return false;
    }



    public static void QuitApplication()
    {
        LeaderboardManager.SaveLeaderboardEntries();

        Debug.Log("Exiting Game");
        Application.Quit();
    }



    public enum InputMethodType
    {
        Keyboard, Arcade
    }
}
