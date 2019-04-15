using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public static class LevelManager
{
    public static string gameSceneName = "Release";
    public static string startSceneName = "Menu";



    public static readonly int startingRooms = 6;
    [HideInInspector]
    public static readonly int difficultyFactor = 4;
    [HideInInspector]
    public static int currentLevel = 0;



    private static readonly float timeForFade = 2f;

    private static long timeAtLevelLoad;

    private static readonly int baseTimeScore = 500;
    private static readonly int timeExpectedPerRoom = 15;
    private static readonly int timeExpectedPerBoss = 30;
    private static readonly int timeMaxPerRoom = 30;
    private static readonly int timeMaxPerBoss = 60;



    private static UIController uiParent;
    private static RoomSpawner roomSpawner;
    private static Transform player;
    private static PlayerController playerPC;



    public static void InitializeLevelManager(UIController uiParent, RoomSpawner roomSpawner, Transform player)
    {
        LevelManager.uiParent = uiParent;
        LevelManager.roomSpawner = roomSpawner;
        LevelManager.player = player;

        playerPC = player.GetComponent<PlayerController>();

        roomSpawner.InitializeRoomSpawner();
    }



    public static void LoadStartingLevel()
    {
        timeAtLevelLoad = DateTime.UtcNow.Ticks;

        SceneManager.LoadScene(gameSceneName);
    }

    public static void LoadNewLevel()
    {
        if (DungeonGameManager.LoadingNewLevel == true)
            return;



        Cursor.visible = false;
        DungeonGameManager.LoadingNewLevel = true;



        long timeTakenS = (DateTime.UtcNow.Ticks - timeAtLevelLoad) / TimeSpan.TicksPerSecond;
        timeAtLevelLoad = DateTime.UtcNow.Ticks;

        DungeonGameManager.CurrentScore += ScoreFromTime(timeTakenS);

        ++currentLevel;


        
        uiParent.StartCoroutine(ResetLevel(timeForFade)); // Borrowing the ui monobehavior
    }

    public static IEnumerator<WaitForSeconds> ResetLevel(float time)
    {
        uiParent.StartFadeBlind(0f, 1f, time, false); // Cover up camera
        yield return new WaitForSeconds(time);

        

        roomSpawner.Reset();
        roomSpawner.SpawnRooms();

        player.position = Vector3.zero;
        playerPC.CurrentHitPoints = playerPC.MaxHitPoints;
        // Reload all gunz 



        yield return new WaitForSeconds(1f); // Used to cover up the camera moving across the level
        uiParent.StartFadeBlind(1f, 0f, time, true); // Uncover camera
        yield return new WaitForSeconds(time);

        DungeonGameManager.LoadingNewLevel = false;
    }



    // Selected at death screen
    public static void LoadStartScene()
    {
        SceneManager.LoadScene(startSceneName);
    }



    private static int ScoreFromTime(long timeTaken)
    {
        long timeExpected = timeExpectedPerRoom * RoomsToSpawn + timeExpectedPerBoss;
        long timeMax = timeMaxPerRoom * RoomsToSpawn + timeMaxPerBoss;
        int scoreGained = baseTimeScore * DifficultyLevel;
        
        if (timeTaken <= timeExpected)
            return scoreGained;
        else if (timeTaken >= timeMax)
            return 0;
        else
            return (int)(scoreGained * ((double)(timeMax - timeTaken) / (timeMax - timeExpected)));
    }

    public static int DifficultyLevel => (currentLevel + 1) * difficultyFactor; // Multiplier for score
    public static int RoomsToSpawn => startingRooms + DifficultyLevel;
}
