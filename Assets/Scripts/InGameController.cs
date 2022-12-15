using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameController : MonoBehaviour
{
    public static InGameController Instance;
    void Awake()
    {
        Instance = this;
        this.gameObject.SetActive(false);
    }


    List<ActorPlayer> LevelActors = new List<ActorPlayer>();

    List<ActorBase> LevelEnemies = new List<ActorBase>();

    [SerializeField]
    Transform ActorInfoContainer;

    [SerializeField]
    string VictoryJingle;

    public int SessionScore;

#if UNITY_ANDROID
    [SerializeField]
    GameObject MobileInput;

    [SerializeField]
    MobileButton MobileUpButton;

    [SerializeField]
    MobileButton MobileLeftButton;

    [SerializeField]
    MobileButton MobileRightButton;

    [SerializeField]
    MobileButton MobileHookButton;

    
#endif


    Coroutine DelayedLevelCompleteInstance;

    public void LeaveSession()
    {
        LevelActors.ForEach(x => SessionScore += x.State.Score);

        GameOverView.Instance.Show(InGameController.Instance.SessionScore, () => LeaderboardController.Instance.ShowLeaderboard());

#if UNITY_ANDROID
        MobileInput.SetActive(false);
#endif


        PangMinigame.Instance.BackToMainMenu();
    }

    public void NewSession()
    {
        SessionScore = 0;

#if UNITY_ANDROID
        MobileInput.SetActive(true);
#endif
    }

    public void DisposeSession()
    {
        LevelActors.Clear();

        for (int i = 0; i < ActorInfoContainer.childCount; i++) Destroy(ActorInfoContainer.GetChild(i).gameObject);

        LevelEnemies.Clear();
    }

    public void PlayerSpawned(ActorPlayer actor)
    {
        if (LevelActors.Count >= PangMinigame.Instance.CurrentPlayerCount)
        {
            actor.gameObject.SetActive(false);
            return;
        }

        LevelActors.Add(actor);

        ResourcesManager.Instance.LoadObject("ActorInfoView", (GameObject actorInfoViewObject) =>
        {
            actorInfoViewObject = Instantiate(actorInfoViewObject);
            actorInfoViewObject.transform.SetParent(ActorInfoContainer, false);
            actorInfoViewObject.transform.localScale = Vector3.one;
            actorInfoViewObject.transform.position = Vector3.zero;
            actorInfoViewObject.GetComponent<ActorInfoView>().SetInfo(actor, "Player " + LevelActors.Count);
        });
    }

    public void EnemySpawned(ActorBase actor)
    {
        LevelEnemies.Add(actor);
    }

    public void EnemyDefeated(ActorBase actor)
    {
        LevelEnemies.Remove(actor);
        if (LevelEnemies.Count == 0)
        {
            if (DelayedLevelCompleteInstance == null)
            {
                DelayedLevelCompleteInstance = StartCoroutine(DelayedLevelComplete());
            }
        }
    }

    IEnumerator DelayedLevelComplete()
    {
        yield return new WaitForSeconds(1f);

        if (LevelEnemies.Count == 0)
        {
            LevelActors.ForEach(x => SessionScore += x.State.Score);
            SoundManager.Instance.PlaySound(VictoryJingle);

            yield return new WaitForSeconds(2f);

            PangMinigame.Instance.NextLevel();
        }

        DelayedLevelCompleteInstance = null;
    }




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) LeaveSession();

        #if UNITY_ANDROID
        if (MobileLeftButton.IsPressed)
        {
             LevelActors.ForEach(x=>x.MoveLeft());
        }
        else if (MobileRightButton.IsPressed)
        {
             LevelActors.ForEach(x=>x.MoveRight());
        }

        if (MobileUpButton.IsPressed)
        {
             LevelActors.ForEach(x=>x.Jump());
        }

        if (MobileHookButton.IsPressed)
        {
             LevelActors.ForEach(x=>x.Hook());
        }
        #endif
    }


}
