using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EntityMovement : MonoBehaviour
{
    protected Rigidbody2D rbody;


    [SerializeField]
    protected float maxVelocity = 5f;

    protected virtual void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }
}
