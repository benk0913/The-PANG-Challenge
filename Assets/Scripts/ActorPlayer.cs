using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActorPlayer : ActorBase
{



    public WeaponData CurrentWeapon
    {
        get
        {
            if (State.CurrentWeapon == null) return DefaultWeapon;

            return State.CurrentWeapon;
        }
        set
        {
            State.CurrentWeapon = value;
        }

    }

    [SerializeField]
    WeaponData DefaultWeapon;

    [SerializeField]
    string GainScoreSound = "score";



    public PlayerState State;


    void OnEnable()
    {
        State = new PlayerState();
        InGameController.Instance.PlayerSpawned(this);
    }

    public void Hook()
    {
        if (State.CurrentHookCooldown > 0f) return;

        State.CurrentHookCooldown = CurrentWeapon.WeaponCooldown;

        if (ActorAnimator != null) ActorAnimator.SetTrigger("Hook");

        ResourcesManager.Instance.LoadFromPool(CurrentWeapon.ProjectilePrefabKey, (GameObject hookObject) =>
        {
            hookObject.transform.position = transform.position;
            hookObject.GetComponent<Projectile>().SetInfo(Hit);
            State.OnStateChanged?.Invoke();
        });
    }

    public override void Hurt(GameObject targetHit)
    {
        base.Hurt(targetHit);
        State.Lives--;
        State.OnStateChanged?.Invoke();

        if (State.Lives <= 0)
        {
            base.Death();
        }
    }

    public void Hit()
    {
        State.Score += 100;
        State.OnStateChanged?.Invoke();
        if (!string.IsNullOrEmpty(GainScoreSound)) SoundManager.Instance.PlaySound(GainScoreSound);

    }

    void Update()
    {
        if (State.CurrentHookCooldown > 0f) State.CurrentHookCooldown -= Time.deltaTime;
    }

    void OnDestroy()
    {
        State.OnStateChanged.RemoveAllListeners();
    }
}

public class PlayerState
{
    public UnityEvent OnStateChanged = new UnityEvent();
    public int Lives = 3;
    public int Score = 0;
    public WeaponData CurrentWeapon;
    public float CurrentHookCooldown;
}
