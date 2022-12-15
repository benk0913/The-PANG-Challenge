using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Action OnHit;

    [SerializeField]
    string appearanceSound = "woosh";

    public void DisableSelf()
    {
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            OnHit?.Invoke();
            collider.GetComponent<ActorBase>().Hurt(this.gameObject);
            DisableSelf();
        }
    }

    public void SetInfo(Action onHit)
    {
        OnHit = onHit;
        
        if (!string.IsNullOrEmpty(appearanceSound)) SoundManager.Instance.PlaySound(appearanceSound);
    }
}
