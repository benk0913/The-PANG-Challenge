using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    RaycastHit2D LeftRay;
    RaycastHit2D RightRay;


    [SerializeField]
    ActorBase ControlledActor;

    public bool IsMovingLeft = false;

    void OnEnable()
    {
        ControlledActor.Hop();
    }

    void Update()
    {

        if (ControlledActor.IsGrounded)
        {
            ControlledActor.Jump();
        }

        if (IsMovingLeft)
        {
            ControlledActor.MoveLeft();
        }
        else
        {
            ControlledActor.MoveRight();
        }


        LeftRay = Physics2D.Raycast(transform.position, Vector3.left, ControlledActor.GroundCheckDistance, ControlledActor.GroundCheckLayermask);
        RightRay = Physics2D.Raycast(transform.position, Vector3.right, ControlledActor.GroundCheckDistance, ControlledActor.GroundCheckLayermask);

        if (LeftRay) IsMovingLeft = false;
        else if (RightRay) IsMovingLeft = true;
    }
}
