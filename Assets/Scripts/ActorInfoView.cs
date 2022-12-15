using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActorInfoView : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI TitleLabel;
    [SerializeField]
    TextMeshProUGUI ScoreLabel;
    [SerializeField]
    TextMeshProUGUI LivesLabel;
    [SerializeField]
    TextMeshProUGUI WeaponLabel;

    ActorPlayer ObservedActor;
    string Name;

    public void RefreshUI()
    {
        TitleLabel.text = Name;
        ScoreLabel.text = "Score: " + ObservedActor.State.Score;
        LivesLabel.text = "Lives: " + ObservedActor.State.Lives;
        WeaponLabel.text = "Weapon: " + ObservedActor.CurrentWeapon.name;
    }

    public void SetInfo(ActorPlayer actor, string actorName)
    {
        Name = actorName;
        ObservedActor = actor;
        actor.State.OnStateChanged?.AddListener(RefreshUI);
    }

}
