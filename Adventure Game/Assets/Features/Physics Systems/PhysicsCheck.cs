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
    [SerializeField]
    [Tooltip("Offset for how far away from the left/right sides should the side rays be drawn and checked. A positive number means away from the center of the object, while a negative number means towards the center of the object.")]
    private float sideCheckOffset = 0f;
    private static Vector3 centerGroundRayOrigin;
    private static Vector3 leftGroundRayOrigin;
    private static Vector3 rightGroundRayOrigin;

    [Header("Vertical Checks")]
    [Tooltip("Minimum vertical velocity upward(must be positive) for the object to be considered moving upward.")]
    [SerializeField]
    private float upwardVelocityThreshold = 0.0001f;

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
        if (!IsFalling()) return true;

        // TODO make this use colliders
        RaycastHit2D[] hits = new RaycastHit2D[3];

        // TODO there is a better way to scale this
        hits[0] = Physics2D.Raycast(centerGroundRayOrigin, Vector2.down, checkDistance, groundLayer);
        if (hits[0].collider != null) return true;
        hits[1] = Physics2D.Raycast(leftGroundRayOrigin, Vector2.down, checkDistance, groundLayer);
        if (hits[1].collider != null) return true;
        hits[2] = Physics2D.Raycast(rightGroundRayOrigin, Vector2.down, checkDistance, groundLayer);
        if (hits[2].collider != null) return true;

        return false;
    }

    private void Update()
    {
        // IsGrounded debug lines
        Debug.DrawRay(centerGroundRayOrigin, Vector3.down * checkDistance, Color.green);
        Debug.DrawRay(leftGroundRayOrigin, Vector3.down * checkDistance, Color.green);
        Debug.DrawRay(rightGroundRayOrigin, Vector3.down * checkDistance, Color.green);
    }

    private void FixedUpdate()
    {
        centerGroundRayOrigin = transform.position;
        leftGroundRayOrigin = centerGroundRayOrigin + (Vector3.left * (transform.localScale.x/2 + sideCheckOffset));
        rightGroundRayOrigin = centerGroundRayOrigin + (Vector3.right * (transform.localScale.x/2 + sideCheckOffset));
    }

    public bool IsMovingUpward()
    {
        return rbody.velocity.y > upwardVelocityThreshold;
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
