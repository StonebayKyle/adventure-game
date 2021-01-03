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

    private bool isFacingRight = true;

    // input captures
    [System.NonSerialized]
    public float horizontalMovementAxis; // horizontal input
    [System.NonSerialized]
    public bool leftHeld;
    [System.NonSerialized]
    public bool rightHeld;
    private bool prevleftHeld;
    private bool prevRightHeld;

    [Header("Movement", order = 0)]
    [Tooltip("The max horizontal speed a player can reach through movement inputs.")]
    public float maxSpeed = 5f;
    [Tooltip("How much time it takes to reach max speed horizontally.")]
    public float accelerationTime = 1f;
    [Tooltip("How much time it takes to stop from max speed horizontally when there is input in the opposite direction. This number is effectively half  because it targets the opposite max speed.")]
    public float oppositeInputDecelerationTime = 1f;
    [Tooltip("How much time it takes to stop from max speed horizontally when there is no input.")]
    public float noInputDecelerationTime = 1f;

    // these values can represent a "kind" of friction
    [Tooltip("Multiplier for acceleration time. 0 means no acceleration, 1 means normal acceleration. 0-1 means reduced time, 1+ means increased time.")]
    [Range(0,5)]
    public float accelerationTimeMultiplier = 1;
    [Tooltip("Multiplier for deceleration times (opposite and no input). 0 means no deceleration, 1 means normal deceleration. 0-1 means reduced time, 1+ means increased time.")]
    [Range(0, 5)]
    public float decelerationTimeMultiplier = 1;

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
        prevleftHeld = leftHeld;
        prevRightHeld = rightHeld;
        horizontalMovementAxis = Input.GetAxisRaw("Horizontal");
        leftHeld = horizontalMovementAxis < 0;
        rightHeld = horizontalMovementAxis > 0;
    }

    private void Move()
    {
        if (ChangedInputMovementDirection())
        {
            Flip(); // make the player always face towards the direction they're running.
        }

        //Vector2 targetVelocity = new Vector2(horizontalMovementAxis * maxSpeed, 0);
        float targetVelocity = horizontalMovementAxis * maxSpeed;

        // with input
        if (horizontalMovementAxis != 0)
        {
            if (HeadingTowardsCurrentDirection())
            {
                // speed up to maxSpeed
                PhysicsUtils.ApplyForceTowards(RigidBody, targetVelocity, accelerationTime, accelerationTimeMultiplier);
            } else if (!HeadingTowardsCurrentDirection())
            {
                // slow to stop. Target velocity is used instead of 0 because the curve is too slow when at 0.
                PhysicsUtils.ApplyForceTowards(RigidBody, targetVelocity, oppositeInputDecelerationTime, decelerationTimeMultiplier);
            }
        } else
        {
            // no input decleration to stop
            PhysicsUtils.ApplyForceTowards(RigidBody, 0, noInputDecelerationTime, decelerationTimeMultiplier);
        }

        //Debug.LogWarning("x velocity: " + RigidBody.velocity.x + "\tref velocity: " + xVelocityRef + "\tSmoothed Velocity:" + smoothedVelocity);
        //Debug.LogWarning("x velocity: " + RigidBody.velocity.x + "\ttargetVelocity: " + targetVelocity + "\taxis: " + horizontalMovementAxis);
    }

    private void Flip()
    {
        Debug.LogWarning("Flip!");
        isFacingRight = !isFacingRight;

        transform.Rotate(0f, 180f, 0);
    }

    private bool HeadingTowardsCurrentDirection()
    {
        return (leftHeld && RigidBody.velocity.x <= 0) ||
                (rightHeld && RigidBody.velocity.x >= 0);
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

    // this update (due to how prevHorizontalMovementAxis is assigned)
    public bool ChangedInputMovementDirection()
    {
        if (horizontalMovementAxis == 0) return false; // no change (in direction) when input stops.

        return rightHeld != isFacingRight;
    }

}
