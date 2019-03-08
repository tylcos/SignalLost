using System;
using UnityEngine;
using UnityEngine.SceneManagement;



public static class LevelManager
{
    public static string levelName = "Release";


    
    public static int startingLevels = 6;
    public static int currentLevel = 0;

    public static long timeAtLevelLoad;

    public static int timeScore = 500;
    public static int[] timeExpected = { };             // Full score if player beats boss in this time
    public static int[] timeFalloff = { };              // After time expected is past, how much time till time score is zero
    public static int[] baseScorePerLevel = { };
    


    static LevelManager()
    {

    }



    public static void LoadStartingLevel()
    {
        SceneManager.LoadSceneAsync(levelName);

        timeAtLevelLoad = DateTime.UtcNow.Ticks;
    }

    public static void LoadNewLevel()
    {
        long timeTaken = (DateTime.UtcNow.Ticks - timeAtLevelLoad) / TimeSpan.TicksPerSecond;
        timeAtLevelLoad = DateTime.UtcNow.Ticks;
        // Score Stuff



        SceneManager.LoadScene(levelName);
        // Spawn rooms with LevelsToSpawn
        // Remove blind from camera
    }



    private static int ScoreFromTime(long timeTaken)
    {
        long expected = timeExpected[currentLevel];

        if (timeTaken <= expected)
            return timeScore;
        else
            return (int)(1);
    }

    private static int LevelsToSpawn => startingLevels + currentLevel * 2;
}
