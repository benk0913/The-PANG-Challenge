using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBase : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D Rigid;

    [SerializeField]
    float JumpHeight = 10f;

    [SerializeField]
    float MovementSpeed = 5f;

    [SerializeField]
    public float GroundCheckDistance = 1f;

    [SerializeField]
    public LayerMask GroundCheckLayermask;

    [SerializeField]
    protected Animator ActorAnimator;
    Vector2 MovementDirection;

    [SerializeField]
    string DeathEffect;

    [SerializeField]
    string HurtSound = "hurt";

    [SerializeField]
    string DeathSound = "death";

    public bool IsGrounded { private set; get; }

    public virtual void Jump()
    {
        if (!IsGrounded) return;

        Hop();
    }

    public virtual void Hop()
    {
        Rigid.velocity = new Vector3(Rigid.velocity.x, 0f);
        Rigid.AddForce(Vector3.up * JumpHeight, ForceMode2D.Impulse);
    }

    public virtual void MoveLeft()
    {
        MovementDirection = -Vector2.right * Time.deltaTime * MovementSpeed;
        Rigid.position += MovementDirection;
    }

    public virtual void MoveRight()
    {
        MovementDirection = Vector2.right * Time.deltaTime * MovementSpeed;
        Rigid.position += MovementDirection;
    }

    void LateUpdate()
    {

        if (ActorAnimator != null)
        {
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
        }

        MovementDirection = Vector3.zero;

        IsGrounded = Physics2D.Raycast(transform.position, -Vector3.up, GroundCheckDistance, GroundCheckLayermask);

    }

    public virtual void Hurt(GameObject targetHit)
    {
        if (ActorAnimator != null) ActorAnimator.SetTrigger("Hurt");

        if(!string.IsNullOrEmpty(HurtSound)) SoundManager.Instance.PlaySound(HurtSound);
    }

    public virtual void Death()
    {
        this.gameObject.SetActive(false);

        if(!string.IsNullOrEmpty(DeathSound)) SoundManager.Instance.PlaySound(DeathSound);

        if (!string.IsNullOrEmpty(DeathEffect))
        {
            ResourcesManager.Instance.LoadFromPool(DeathEffect, (GameObject effect) =>
            {
                effect.transform.position = transform.position;
            });
        }

    }
}
