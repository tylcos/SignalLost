using System;
using UnityEngine;



public class DungeonGameManager : MonoBehaviour
{
    public bool menuScene = false;
    public UIController uiParent;
    public RoomSpawner roomSpawner;
    public Transform player;



    public GameObject[] enemies;
    public static int NumberOfEnemies = 0;

    public static GameObject[] Enemies;



    public static InputMethodType InputMethod = InputMethodType.Keyboard;



    public static int CurrentScore;

    public delegate void ScoreChangedHandler();
    public static event ScoreChangedHandler ScoreChanged;

    public static bool LoadingNewLevel = false;
    public static bool ApplicationQuit = false;



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
        }
    }



    void Start()
    {
        if (!menuScene)
        {
            Enemies = enemies;
            LoadingNewLevel = false;


            LevelManager.InitializeLevelManager(uiParent, roomSpawner, player);

            // Manually spawning the room for the first iteration
            roomSpawner.SpawnRooms();
            uiParent.StartFadeBlind(1f, 0f, 2f, true);
        }
    }

    void Update()
    {
        Debug.Log(NumberOfEnemies);

        if (Input.GetKeyDown(KeyCode.O) || NumberOfEnemies <= 0) // <o/
            LevelManager.LoadNewLevel();

        if (!ApplicationQuit && Input.GetAxis("ArcadeExit") > 0)
            QuitApplication();
    }




    public static void AddScore(int amount)
    {
        CurrentScore += amount;
        ScoreChanged();
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
        if (InputMethod == InputMethodType.Arcade)
            return new Vector2(Input.GetAxisRaw("HorizontalArcade"), Input.GetAxisRaw("VerticalArcade"));

        return Vector2.zero;
    }

    public static Vector2 GetAimingVector()
    {
        if (InputMethod == InputMethodType.Keyboard)
            return new Vector2(Input.GetAxisRaw("HorizontalKeys"), Input.GetAxisRaw("VerticalKeys"));
        if (InputMethod == InputMethodType.Arcade)
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
        ApplicationQuit = true;

        LeaderboardManager.SaveLeaderboardEntries();

        Debug.Log("Exiting Game");
        Application.Quit();
    }



    public enum InputMethodType
    {
        Keyboard, Arcade
    }
}
