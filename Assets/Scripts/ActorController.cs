using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorView))]
public class ActorController : MonoBehaviour
{
    [SerializeField]
    ActorInputPreset ActorInputModel;

    [SerializeField]
    ActorView ControlledActor;

    void Reset()
    {
        ControlledActor = GetComponent<ActorView>();
    }


    void Update()
    {
        if (Input.GetKey(ActorInputModel.MoveLeftKey))
        {
            ControlledActor.MoveLeft();
        }
        else if (Input.GetKey(ActorInputModel.MoveRightKey))
        {
            ControlledActor.MoveRight();
        }

        if (Input.GetKeyDown(ActorInputModel.JumpKey))
        {
            ControlledActor.Jump();
        }

        if (Input.GetKeyDown(ActorInputModel.HookKey))
        {
            ControlledActor.Hook();
        }
    }
}

[System.Serializable]
public class ActorInputPreset
{
    [SerializeField]
    public KeyCode MoveLeftKey;
    [SerializeField]
    public KeyCode MoveRightKey;
    [SerializeField]
    public KeyCode JumpKey;
    [SerializeField]
    public KeyCode HookKey;
}
