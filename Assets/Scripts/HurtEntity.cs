using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEntity : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            collider.GetComponent<ActorPlayer>().Hurt(this.gameObject);
        }
    }
}
