using System;
using UnityEngine;
using UnityEngine.SceneManagement;



public static class LevelManager
{
    public static string levelName = "Release";


    
    public static int startingLevels = 6;
    public static int currentLevel = 0;

    public static long timeAtLevelLoad;

    private static readonly int baseTimeScore = 500;
    private static readonly int timeExpectedPerLevel = 15;
    private static readonly int timeMaxPerLevel = 30;



    public static void LoadStartingLevel()
    {
        SceneManager.LoadScene(levelName);

        timeAtLevelLoad = DateTime.UtcNow.Ticks;
    }

    public static void LoadNewLevel()
    {
        long timeTakenS = (DateTime.UtcNow.Ticks - timeAtLevelLoad) / TimeSpan.TicksPerSecond;
        timeAtLevelLoad = DateTime.UtcNow.Ticks;

        DungeonGameManager.CurrentScore += ScoreFromTime(timeTakenS);



        ++currentLevel;
        SceneManager.LoadScene(levelName);
        // Spawn rooms with LevelsToSpawn
        // Remove blind from camera
    }

    public static void LoadDeathScene()
    {

    }

    public static void LoadStartScene()
    {

    }



    private static int ScoreFromTime(long timeTaken)
    {
        long timeExpected = timeExpectedPerLevel * LevelsToSpawn;
        long timeMax = timeMaxPerLevel * LevelsToSpawn - timeExpected;

        if (timeTaken <= timeExpected)
            return baseTimeScore;
        else
            return (int)(baseTimeScore * (double)(timeTaken - timeExpected) / timeMax);
    }

    private static int LevelsToSpawn => startingLevels + currentLevel * 2;
}
