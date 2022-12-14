using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Player1Score;

    [SerializeField]
    TextMeshProUGUI Player2Score;

    SessionData Session;

    public void LeaveSession()
    {
        PangMinigame.Instance.BackToMainMenu();
    }

    public void ResetData()
    {
        Session = new SessionData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) LeaveSession();
    }

    public void Player1GainsScore()
    {
        Session.Player1Data.Score++;
    }

    public void Player2GainsScore()
    {
        Session.Player2Data.Score++;
    }

    public class SessionData
    {
        public PlayerSessionData Player1Data;
        public PlayerSessionData Player2Data;

        public SessionData()
        {
            Player1Data = new PlayerSessionData();
            Player2Data = new PlayerSessionData();
        }
    }

    public class PlayerSessionData
    {
        public int Score = 0;
        public int LivesLeft = 3;
    }
}
