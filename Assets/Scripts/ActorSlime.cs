using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSlime : ActorBase
{
    [SerializeField]
    string OnDeathSlime;

    void OnEnable()
    {
        InGameController.Instance.EnemySpawned(this);
    }

    public override void Hurt(GameObject targetHit)
    {
        base.Hurt(targetHit);
        if (ActorAnimator != null) ActorAnimator.SetTrigger("Hurt");

        if (!string.IsNullOrEmpty(OnDeathSlime))
        {
            SpawnDeathSlime(false);
            SpawnDeathSlime(true);
        }

        Death();
    }

    void SpawnDeathSlime(bool movingLeft = false)
    {
        ResourcesManager.Instance.LoadFromPool(OnDeathSlime, (GameObject slime) =>
        {
            slime.transform.position = transform.position;
            slime.GetComponent<SlimeAI>().IsMovingLeft = movingLeft;
        });
    }

    public override void Death()
    {
        base.Death();
        InGameController.Instance.EnemyDefeated(this);
    }
}
