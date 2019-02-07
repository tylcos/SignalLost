using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



public class LeaderboardManager : MonoBehaviour
{
    private static readonly BinaryFormatter formatter = new BinaryFormatter();



    public LeaderboardEntry[] GetLeaderboardEntries()
    {
        FileStream fs = new FileStream(Application.persistentDataPath, FileMode.Open);
        return (LeaderboardEntry[]) formatter.Deserialize(fs);
    }

    public void SetLeaderboardEntries(LeaderboardEntry[] leaderboardEntries)
    {
        FileStream fs = new FileStream(Application.persistentDataPath, FileMode.O);
        return (LeaderboardEntry[])formatter.Deserialize(fs);
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
