using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



public static class LeaderboardManager
{
    public static readonly string leaderboardPath = Path.Combine(Application.persistentDataPath, "highscores.dat");



    private static readonly BinaryFormatter formatter = new BinaryFormatter();



    public static List<LeaderboardEntry> GetLeaderboardEntries()
    {
        FileStream fs = new FileStream(leaderboardPath, FileMode.Open);
        return (List<LeaderboardEntry>)formatter.Deserialize(fs);
    }

    public static void SetLeaderboardEntries(List<LeaderboardEntry> leaderboardEntries)
    {
        File.Delete(leaderboardPath);
        FileStream fs = new FileStream(leaderboardPath, FileMode.CreateNew);
        formatter.Serialize(fs, leaderboardEntries);
    }
}



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
}
