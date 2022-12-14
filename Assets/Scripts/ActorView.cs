using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorView : MonoBehaviour
{
    [SerializeField]
    Animator ActorAnimator;

    [SerializeField]
    Rigidbody2D Rigid;

    [SerializeField]
    float JumpHeight = 10f;

    [SerializeField]
    float MovementSpeed = 5f;

    [SerializeField]
    float GroundCheckDistance = 1f;

    [SerializeField]
    LayerMask GroundCheckLayermask;

    [SerializeField]
    float HookCooldown = 1f;

    [SerializeField]
    string ProjectileAssetKey = "KnifeProjectile";
    Vector2 MovementDirection;

    bool IsGrounded;

    float CurrentHookCooldown;

    public void Hook()
    {
        if (CurrentHookCooldown > 0f) return;

        CurrentHookCooldown = HookCooldown;

        ActorAnimator.SetTrigger("Hook");

        ResourcesManager.Instance.LoadObject("Hook", (GameObject hookObject) =>
        {
            hookObject = Instantiate(hookObject);
            hookObject.transform.position = transform.position;
        });
    }

    public void Jump()
    {
        if (!IsGrounded) return;

        Rigid.AddForce(Vector3.up * JumpHeight, ForceMode2D.Impulse);
    }

    public void MoveLeft()
    {
        MovementDirection = -Vector2.right * Time.deltaTime * MovementSpeed;
        Rigid.position += MovementDirection;
    }

    public void MoveRight()
    {
        MovementDirection = Vector2.right * Time.deltaTime * MovementSpeed;
        Rigid.position += MovementDirection;
    }

    void LateUpdate()
    {
        if (CurrentHookCooldown > 0f) CurrentHookCooldown -= Time.deltaTime;

        ActorAnimator.SetBool("InAir", Mathf.Abs(Rigid.velocity.y) > 0.1f);

        if (MovementDirection.x > 0)
        {
            ActorAnimator.SetBool("Walk", true);
            ActorAnimator.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (MovementDirection.x < -0)
        {
            ActorAnimator.SetBool("Walk", true);
            ActorAnimator.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            ActorAnimator.SetBool("Walk", false);
        }

        MovementDirection = Vector3.zero;

        IsGrounded = Physics2D.Raycast(transform.position, -Vector3.up, GroundCheckDistance, GroundCheckLayermask);

    }
}
