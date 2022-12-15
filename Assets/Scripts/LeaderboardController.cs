using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    public static LeaderboardController Instance;

    public List<LeaderBoardEntryData> Leaderboard = new List<LeaderBoardEntryData>();

    [SerializeField]
    LeaderboardView LeaderboardUI;

    void Awake()
    {
        Instance = this;
        this.gameObject.SetActive(false);
    }

    public void ShowLeaderboard()
    {
        this.gameObject.SetActive(true);
        LoadLeaderboard();
        LeaderboardUI.RefreshUI();
    }

    public void LoadLeaderboard()
    {
        Leaderboard.Clear();

        for (int entryIndex = 0; entryIndex < 10; entryIndex++)
        {
            string entryName = PlayerPrefs.GetString("lb_name_" + entryIndex, "-Empty-");
            int entryScore = PlayerPrefs.GetInt("lb_score_" + entryIndex, 0);

            Leaderboard.Add(new LeaderBoardEntryData(entryName, entryScore));
        }
    }

    public void SaveLeaderboard()
    {
        if (Leaderboard.Count == 0) return;

        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetString("lb_name_" + i, Leaderboard[i].Name);
            PlayerPrefs.SetInt("lb_score_" + i, Leaderboard[i].Score);
            PlayerPrefs.Save();
        }
    }

    public void AddLeaderboardEntry(string name, int score)
    {
        if (Leaderboard.Count == 0) LoadLeaderboard();

        for (int i = 0; i < 10; i++)
        {
            if (Leaderboard[i].Score < score)
            {
                Leaderboard.Insert(i, new LeaderBoardEntryData(name, score));
                Leaderboard.RemoveAt(Leaderboard.Count - 1);

                SaveLeaderboard();

                LeaderboardUI.RefreshUI();
                return;
            }
        }

        //Score not high enough prompt?
    }

    public class LeaderBoardEntryData
    {
        public string Name;
        public int Score;

        public LeaderBoardEntryData(string name, int score)
        {
            this.Name = name;
            this.Score = score;
        }
    }

}
