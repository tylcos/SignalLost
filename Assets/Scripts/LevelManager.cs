using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public static class LevelManager
{
    public static string gameSceneName = "Release";
    public static string startSceneName = "Menu";



    public static int startingRooms = 6;
    [HideInInspector]
    public static int currentLevel = 0;



    private static float timeForFade = 2f;

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
        UIController ui = GameObject.FindGameObjectWithTag("UI Parent").GetComponent<UIController>();
        DungeonGameManager.mouseOff = Cursor.visible;
        ui.StartFadeBlind(0f, 1f, 2f, false);
        ui.StartCoroutine(LoadNewLevelWait(2.1f));



        long timeTakenS = (DateTime.UtcNow.Ticks - timeAtLevelLoad) / TimeSpan.TicksPerSecond;
        timeAtLevelLoad = DateTime.UtcNow.Ticks;
        
        DungeonGameManager.CurrentScore += ScoreFromTime(timeTakenS);

        ++currentLevel;
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
        long timeExpected = timeExpectedPerRoom * LevelsToSpawn + timeExpectedPerBoss;
        long timeMax = timeMaxPerRoom * LevelsToSpawn + timeMaxPerBoss;

        if (timeTaken <= timeExpected)
            return baseTimeScore;
        else
            return 0; //(int)(baseTimeScore * (double)(timeMax - timeTaken) / (timeMax - timeExpected));
    }

    private static int LevelsToSpawn => startingRooms + currentLevel * 2;
}
