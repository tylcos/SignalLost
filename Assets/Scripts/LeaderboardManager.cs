using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



public static class LeaderboardManager
{
    public static List<LeaderboardEntry> leaderboardEntries;



    public static readonly string savePath = Path.Combine(Application.persistentDataPath, "highscores.dat");
    
    private static readonly BinaryFormatter formatter = new BinaryFormatter();



    public static void LoadLeaderboardEntries()
    {
        if (!File.Exists(savePath))
        {
            leaderboardEntries = new List<LeaderboardEntry>();
            SaveLeaderboardEntries();
        }



        try
        {
            using (FileStream fs = new FileStream(savePath, FileMode.Open))
                leaderboardEntries = (List<LeaderboardEntry>)formatter.Deserialize(fs);
        }
        catch (Exception)
        {
            Debug.LogError("Error opening leaderboard file at " + savePath);



            if (File.Exists(savePath))
            {
                string backupPath = savePath + ".bak";
                if (File.Exists(backupPath))
                    File.Delete(backupPath);

                File.Move(savePath, backupPath);
            }
            


            leaderboardEntries = new List<LeaderboardEntry>();
        }
    }

    public static void SaveLeaderboardEntries()
    {
        try
        {
            if (File.Exists(savePath))
                File.Delete(savePath);

            using (FileStream fs = new FileStream(savePath, FileMode.CreateNew))
                formatter.Serialize(fs, leaderboardEntries);
        }
        catch (Exception)
        {
            Debug.Log("Error saving leaderboard file at " + savePath);
        }
    }



    public static void AddCurrentRun(string name)
    {
        if (name.Length > LeaderboardEntry.nameLength)
            name = name.Substring(0, LeaderboardEntry.nameLength);



        // Find where to insert current run based on index (Lower index = higher score)
        int i = 0;
        while (i < leaderboardEntries.Count && leaderboardEntries[i++].Score > DungeonGameManager.CurrentScore);

        if (i < leaderboardEntries.Count)
            leaderboardEntries.Insert(i, new LeaderboardEntry(name, DungeonGameManager.CurrentScore));
        else
            leaderboardEntries.Add(new LeaderboardEntry(name, DungeonGameManager.CurrentScore));
    }
}



[Serializable]
public readonly struct LeaderboardEntry
{
    public readonly string Name;
    public readonly long Score;
    public readonly long Date;



    public static readonly int nameLength = 6;



    public LeaderboardEntry(string name, long score)
    {
        Name = name.PadRight(nameLength);
        Score = score;
        Date = DateTime.UtcNow.Ticks;
    }



    public string DebugToString()
    {
        return $"[LeaderboardEntry] {Name} achieved {Score} score on {new DateTime(Date).ToString("f")}";
    }

    public override string ToString()
    {
        return $"{Name} - {Score.ToString()} - {new DateTime(Date).ToString("f").PadLeft(25)}";
    }
}
