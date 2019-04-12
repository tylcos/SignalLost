using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public static class LevelManager
{
    public static string gameSceneName = "Release";
    public static string startSceneName = "Menu";



    public static readonly int startingRooms = 2;
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



    public static void LoadStartingLevel()
    {
        SceneManager.LoadScene(gameSceneName);

        timeAtLevelLoad = DateTime.UtcNow.Ticks;
    }

    public static void LoadNewLevel()
    {
        if (DungeonGameManager.LoadingNewLevel == true)
            return;

        DungeonGameManager.MouseOn = Cursor.visible;
        DungeonGameManager.LoadingNewLevel = true;



        long timeTakenS = (DateTime.UtcNow.Ticks - timeAtLevelLoad) / TimeSpan.TicksPerSecond;
        timeAtLevelLoad = DateTime.UtcNow.Ticks;

        DungeonGameManager.CurrentScore += ScoreFromTime(timeTakenS);

        ++currentLevel;



        UIController ui = GameObject.FindGameObjectWithTag("UI Parent").GetComponent<UIController>();
        ui.StartFadeBlind(0f, 1f, timeForFade, false);
        ui.StartCoroutine(LoadNewLevelWait(timeForFade));
    }

    private static IEnumerator<WaitForSeconds> LoadNewLevelWait(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(gameSceneName);
        // Spawn rooms with LevelsToSpawn
        // Remove blind from camera *DONE*
    }

    public static void LoadDeathScreen()
    {
        // Load death ui not a scene
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
