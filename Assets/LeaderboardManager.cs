using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



public static class LeaderboardManager
{
    public static readonly string savePath = Path.Combine(Application.persistentDataPath, "highscores.dat");



    private static readonly BinaryFormatter formatter = new BinaryFormatter();



    public static List<LeaderboardEntry> GetLeaderboardEntries()
    {
        if (!File.Exists(savePath))
            return new List<LeaderboardEntry>();

        try
        {
            using (FileStream fs = new FileStream(savePath, FileMode.Open))
                return (List<LeaderboardEntry>)formatter.Deserialize(fs);
        }
        catch (Exception)
        {
            Debug.Log("Error opening leaderboard file at " + savePath);
            return new List<LeaderboardEntry>();
        }
    }

    public static void SetLeaderboardEntries(List<LeaderboardEntry> leaderboardEntries)
    {
        File.Delete(savePath);

        using (FileStream fs = new FileStream(savePath, FileMode.CreateNew))
            formatter.Serialize(fs, leaderboardEntries);
    }
}



[Serializable]
public readonly struct LeaderboardEntry
{
    readonly string Name;
    readonly long Score;
    readonly long Date;



    public LeaderboardEntry(string name, long score)
    {
        Name = name;
        Score = score;
        Date = DateTime.UtcNow.Ticks;
    }



    public override string ToString()   
    {
        return $"[LeaderboardEntry] {Name} achieved {Score} score on {new DateTime(Date).ToString("dddd MM/dd/yyyy HH:mm:ss")}";
    }
}
