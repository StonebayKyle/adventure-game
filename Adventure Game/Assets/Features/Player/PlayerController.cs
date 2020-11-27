using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PhysicsCheck))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RigidBody { get; private set; }

    private BoxCollider2D boxCollider;

    private PhysicsCheck physicsCheck;

    private PlayerStateMachine stateMachine;

    // input captures
    [System.NonSerialized]
    public float horizontalMovementAxis; // horizontal input
    [System.NonSerialized]
    public bool leftHeld;
    [System.NonSerialized]
    public bool rightHeld;

    [Header("Movement", order = 0)]
    [Tooltip("The max horizontal speed a player can reach through movement inputs.")]
    public float maxSpeed = 5f;
    [Tooltip("How much time it takes to reach max speed horizontally.")]
    public float accelerationTime = 1f;
    [Tooltip("How much time it takes to stop from max speed horizontally when there is input in the opposite direction.")]
    public float oppositeInputDecelerationTime = 1f;
    [Tooltip("How much time it takes to stop from max speed horizontally when there is no input.")]
    public float noInputDecelerationTime = 1f;
    private float xVelocityRef = 0; // used as reference in velocity damping

    [Header("Idle", order = 1)]

    [Header("Running", order = 2)]

    [Header("Jumping", order = 3)]

    [Tooltip("Vertical force applied to the Rigidbody on jump.")]
    public float jumpForce = 100f;
    [Tooltip("Gravity multiplier while jumping after the jump button is released.")]
    [Range(0, 10)] 
    public float lowJumpMultiplier = 2f;

    [Header("Falling")]

    [Tooltip("Gravity multiplier while falling.")]
    [Range(0, 10)]
    public float fallMultiplier = 1f;
    [Tooltip("Time the player can queue a jump before a collision with the ground")]
    public float forgiveJumpSeconds; // can help make jumping more responsive


    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();

        boxCollider = GetComponent<BoxCollider2D>();

        physicsCheck = GetComponent<PhysicsCheck>();

        stateMachine = new PlayerStateMachine(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(new PlayerIdleState());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();

        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        Move();
        stateMachine.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        stateMachine.OnCollisionEnter(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        stateMachine.OnCollisionExit(collision);
    }
    public void ChangeState(IPlayerMovementState newState)
    {
        stateMachine.ChangeState(newState);
    }

    private void UpdateInputs()
    {
        horizontalMovementAxis = Input.GetAxis("Horizontal");
        leftHeld = horizontalMovementAxis < 0;
        rightHeld = horizontalMovementAxis > 0;
    }

    private void Move()
    {
        if (horizontalMovementAxis == 0)
        {
            StopMoving(noInputDecelerationTime);
        } else if (HeadingOppositeDirection())
        {
            StopMoving(oppositeInputDecelerationTime);
        }

        float targetVelocity = horizontalMovementAxis * maxSpeed;

        float smoothedVelocity = Mathf.SmoothDamp(RigidBody.velocity.x, targetVelocity, ref xVelocityRef, accelerationTime, Mathf.Infinity, Time.fixedDeltaTime);
        RigidBody.velocity = new Vector2(smoothedVelocity, RigidBody.velocity.y);
        Debug.LogWarning("x velocity: " + RigidBody.velocity.x + "\tref velocity: " + xVelocityRef + "\tSmoothed Velocity:" + smoothedVelocity);

    }

    // decelerate
    private void StopMoving(float decelerationTime)
    {
        float xVelocity = 0.0f;
        float stopSpeed = 0f;
        float move = Mathf.SmoothDamp(RigidBody.velocity.x, stopSpeed, ref xVelocity, decelerationTime, Mathf.Infinity, Time.fixedDeltaTime);
        RigidBody.velocity = new Vector2(move, RigidBody.velocity.y);
    }

    private bool HeadingOppositeDirection()
    {
        return (leftHeld && RigidBody.velocity.x > 0) ||
                (rightHeld && RigidBody.velocity.x < 0);
    }

    public bool HorizontalMovementUnderMax(float maxSpeed)
    {
        return (rightHeld && RigidBody.velocity.x < maxSpeed) ||
            (leftHeld && RigidBody.velocity.x > -maxSpeed);
    }

    public bool IsFalling()
    {
        return physicsCheck.IsFalling();
    }

    public bool IsGrounded()
    {
        return physicsCheck.IsGrounded();
    }

    public bool IsHorizontallyMoving()
    {
        return physicsCheck.IsHorizontallyMoving();
    }

}
