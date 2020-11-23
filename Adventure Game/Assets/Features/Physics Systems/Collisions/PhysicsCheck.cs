using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsCheck : MonoBehaviour
{
    private Rigidbody2D rbody;

    [Header("Ground Check")]

    [SerializeField]
    public LayerMask groundLayer;
    [Tooltip("Distance below the object to check for the ground.")]
    [SerializeField]
    private float checkDistance = 1f;
    [Header("Fall Check")]

    [Tooltip("How fast the object is moving downward before the object is considered falling.")]
    [SerializeField]
    private float fallVelocityThreshold = -.0001f;


    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, Vector2.down);
    }

    public bool IsFalling()
    {
        return rbody.velocity.y < fallVelocityThreshold;
    }
}
