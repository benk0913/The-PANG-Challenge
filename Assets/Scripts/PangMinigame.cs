using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PangMinigame : MonoBehaviour

{
    public static PangMinigame Instance;

    public List<string> CampaignLevels = new List<string>();

    [SerializeField]
    MainMenuController MainMenu;

    GameObject CurrentLevel;
    public int CurrentPlayerCount { private set; get;}

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void StartCampaign(int playerCount = 1)
    {
        CurrentPlayerCount = playerCount;

        LoadLevel(CampaignLevels[0]);
    }

    public void LoadLevel(string levelKey)
    {

        DisposeCurrentSession();
        
        MainMenu.gameObject.SetActive(false);

        ResourcesManager.Instance.LoadObject(levelKey,
        (GameObject levelObject) =>
        {
            CurrentLevel = Instantiate(levelObject);
            CurrentLevel.name = levelKey;
            InGameController.Instance.gameObject.SetActive(true);
            InGameController.Instance.NewSession();
        });
    }

    public void NextLevel()
    {
        int levelIndex = CampaignLevels.IndexOf(CurrentLevel.name);
        levelIndex++;

        if (levelIndex >= CampaignLevels.Count)
        {
            BackToMainMenu();
            GameOverView.Instance.Show(InGameController.Instance.SessionScore, LeaderboardController.Instance.ShowLeaderboard);
        }
        else
        {
            LoadLevel(CampaignLevels[levelIndex]);
        }
    }


    public void BackToMainMenu()
    {
        InGameController.Instance.gameObject.SetActive(false);

        DisposeCurrentSession();

        MainMenu.gameObject.SetActive(true);
    }

    void DisposeCurrentSession()
    {
        Destroy(CurrentLevel);
        InGameController.Instance.DisposeSession();
    }




}
