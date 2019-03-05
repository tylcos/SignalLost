using System;
using UnityEngine.SceneManagement;



public static class LevelManager
{
    public static string startingLevelName = "Release";



    public static int startingLevels = 6;
    public static int currentLevel = 0;

    public static long timeAtLevelLoad;
    public static int[] timeExpectedPerLevel = { };
    public static int[] baseScorePerLevel = { };
    


    static LevelManager()
    {

    }



    public static void LoadStartingLevel()
    {
        SceneManager.LoadSceneAsync(startingLevelName);

        timeAtLevelLoad = DateTime.UtcNow.Ticks;
    }

    public static void LoadNewLevel()
    {
        long timeTaken = (DateTime.UtcNow.Ticks - timeAtLevelLoad) / TimeSpan.TicksPerSecond;

        
    }
}
