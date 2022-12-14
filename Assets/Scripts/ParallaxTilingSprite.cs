using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTilingSprite : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer Renderer;

    [SerializeField]
    Transform ReferenceObject;

    [SerializeField]
    float Speed = 1f;

    [SerializeField]
    bool AutoMove = false;

    Vector3 LastPos;

    float acceleration = 0f;
    float LastDir =0f;

    void Update()
    {
        if(AutoMove)
        {
            Renderer.material.mainTextureOffset = new Vector2(Renderer.material.mainTextureOffset.x + (Time.deltaTime * Speed), Renderer.material.mainTextureOffset.y);
            return;
        }

        float deltaMovement = ((ReferenceObject.position - LastPos).x * Speed* Time.deltaTime);

        if((LastDir > 0f && deltaMovement < 0f) || (LastDir < 0f && deltaMovement > 0f))
        {
            acceleration = 0f;
        }
        LastDir = deltaMovement;

        if(deltaMovement != 0f)
        {
            acceleration = Mathf.Lerp(acceleration, 1f, Time.deltaTime * 0.5f);
        }
        else
        {
            acceleration = Mathf.Lerp(acceleration, 0f, Time.deltaTime * 0.5f);
        }

        deltaMovement *= acceleration;



        Renderer.material.mainTextureOffset = new Vector2(Renderer.material.mainTextureOffset.x + deltaMovement, Renderer.material.mainTextureOffset.y);

        LastPos = ReferenceObject.position;
    }
}
