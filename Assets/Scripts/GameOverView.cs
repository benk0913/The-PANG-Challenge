using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameOverView : MonoBehaviour
{
    public static GameOverView Instance;

    [SerializeField]
    TMP_InputField NameField;

    [SerializeField]
    TextMeshProUGUI ScoreLabel;

    int Score;

    Action OnSubmit;

    void Awake()
    {
        Instance = this;
        this.gameObject.SetActive(false);
    }

    public void Show(int score, Action onSubmit = null)
    {
        this.gameObject.SetActive(true);
        NameField.Select();
        NameField.ActivateInputField();
        Score = score;
        ScoreLabel.text = Score.ToString();

        OnSubmit = onSubmit;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Submit();
        }
    }

    public void Submit()
    {
        if(string.IsNullOrEmpty(NameField.text)) NameField.text = "Anon";
        LeaderboardController.Instance.AddLeaderboardEntry(NameField.text + " | " + System.DateTime.Now.ToString(), Score);
        this.gameObject.SetActive(false);
        OnSubmit?.Invoke();
    }
}
