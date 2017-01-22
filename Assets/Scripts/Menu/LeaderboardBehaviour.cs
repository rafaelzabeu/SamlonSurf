using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardBehaviour : MonoBehaviour {

    public List<LeaderboardItem> infos;

    public void LoadLeaderboard()
    {
        Leaderboard lb = Leaderboard.LoadLeaderboard();
        int i = 0;
        foreach (var lbInfo in lb.infos)
        {
            infos[i].SetItem(lbInfo);
            i++;
        }
    }

}

[Serializable]
public class LeaderboardInfo : IComparable<LeaderboardInfo>
{
    public string Name { get; set; }
    public float Score { get; set; }

    public string formatedScore
    {
        get
        {
            return ((int)Score).ToString();
        }
    }

    public LeaderboardInfo()
    {
        Name = null;
        Score = -1;
    }

    public int CompareTo(LeaderboardInfo other)
    {
        if (Name == null && other.Name != null)
            return 1;
        else if (other.Name == null && Name == null)
            return 0;
        else if (Name != null && other.Name == null)
            return -1;

        float result = other.Score - Score;

        if (result < 0)
        {
            return -1;
        }
        else if (result > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }

    }
}

[Serializable]
public class Leaderboard
{
    private const string LEADERBOARD_KEY = "Leaderboard";

    public LeaderboardInfo[] infos;

    public Leaderboard()
    {
        infos = new LeaderboardInfo[10];
        for (int i = 0; i < infos.Length; i++)
        {
            infos[i] = new LeaderboardInfo();
        }
    }

    public static void AddScore(LeaderboardInfo info)
    {
        Leaderboard lb = LoadLeaderboard();

        int biggerPos = -1;

        for (int i = 0; i < lb.infos.Length; i++)
        {
            if(lb.infos[i].Name == null)
            {
                biggerPos = i;
                break;
            }

            if(biggerPos < 0)
            {
                if(lb.infos[i].Score < info.Score)
                {
                    biggerPos = i;
                }
            }
        }

        if(biggerPos > -1)
        {
            lb.infos[biggerPos] = info; 
        }

        lb.SortLeaderboard();
        lb.SaveLeaderboard();
    }

    public void SaveLeaderboard()
    {
        PlayerPrefs.SetString(LEADERBOARD_KEY, SimpleXMLSerializer.Serialize(this));
        PlayerPrefs.Save();
    }

    public void SortLeaderboard()
    {
        Array.Sort(infos);
    }

    public static Leaderboard LoadLeaderboard()
    {
        string xml = PlayerPrefs.GetString(LEADERBOARD_KEY,null);
        Debug.Log(xml);
        if(string.IsNullOrEmpty(xml))
        {
            return new Leaderboard();
        }
        return SimpleXMLSerializer.Deserialize<Leaderboard>(xml);
    }

}
