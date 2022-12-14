using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PangMinigame : MonoBehaviour

{
    public static PangMinigame Instance;

    [SerializeField]
    MainMenuController MainMenu;

    [SerializeField]
    InGameController IngameUI;


    GameObject CurrentLevel;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void LoadLevel(string levelKey)
    {
        DisposeCurrentSession();

        ResourcesManager.Instance.LoadObject(levelKey,
        (GameObject levelObject) =>
        {
            CurrentLevel = Instantiate(levelObject);
            MainMenu.gameObject.SetActive(false);
            IngameUI.gameObject.SetActive(true);
            IngameUI.ResetData();
        });
    }

    public void BackToMainMenu()
    {
        IngameUI.gameObject.SetActive(false);

        DisposeCurrentSession();

        MainMenu.gameObject.SetActive(true);
    }

    void DisposeCurrentSession()
    {
        Destroy(CurrentLevel);
    }




}
