using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO make it play an animation and do more interesting things with environment.
        if (collision.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
