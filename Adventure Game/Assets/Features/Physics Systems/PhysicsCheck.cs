using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsCheck : MonoBehaviour
{
    private Rigidbody2D rbody;

    [Header("Ground Check")]

    public LayerMask groundLayer;
    [Tooltip("Distance below the object to check for the ground.")]
    [SerializeField]
    private float checkDistance = 1f;
    [Header("Fall Check")]

    [Tooltip("Minimum vertical velocity downward(must be negative) for the object to be considered falling.")]
    [SerializeField]
    private float fallVelocityThreshold = -.0001f;
    [Header("Move Check")]

    [Tooltip("Minimum horizontal velocity (absolute, must be positive) for the object to be considered moving.")]
    [SerializeField]
    private float moveVelocityThreshold = .001f;


    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    public bool IsGrounded()
    {
        // TODO make this use colliders
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, groundLayer);
        return hit.collider != null;
    }

    private void Update()
    {
        // IsGrounded debug line
        Debug.DrawRay(transform.position, Vector3.down * checkDistance, Color.green);
    }

    public bool IsFalling()
    {
        return rbody.velocity.y < fallVelocityThreshold;
    }

    public bool IsHorizontallyMoving()
    {
        return rbody.velocity.x < -moveVelocityThreshold || // left direction
            rbody.velocity.x > moveVelocityThreshold; // right direction
    }

}
