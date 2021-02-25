using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PhysicsCheck))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RigidBody { get; private set; }

    private AudioSource audioSource;
    private Animator animator;
    private string currentAnimationState;

    // eventually convert into an enum
    [System.NonSerialized]
    public const string IDLE_ANIMATION = "PlayerIdleAnimation";
    [System.NonSerialized]
    public const string RUN_ANIMATION = "PlayerRunAnimation";
    [System.NonSerialized]
    public const string WALK_ANIMATION = "PlayerWalkAnimation";
    [System.NonSerialized]
    public const string JUMP_ANIMATION = "PlayerJumpAnimation";

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
    [System.NonSerialized]
    public bool jumpInitiated; // true when Jump() is called (about to switch into Upward state from a jump), false AFTER the state was entered.
    [System.NonSerialized]
    public bool blasterFiredInAir; // true after blaster was fired to enter upward state, or after fired while in upward or falling states. Reset to false on hitting the ground.
    private bool prevleftHeld;
    private bool prevRightHeld;

    [Header("Movement", order = 0)]
    [Tooltip("The max horizontal speed a player can reach through movement inputs.")]
    public float maxSpeed = 5f;
    private float targetVelocity; // maxSpeed * horizontalMovementAxis, used as a directional target velocity for current movement.


    [Header("Ground", order = 1)]
    [Tooltip("How much time it takes to reach max speed horizontally while on the ground.")]
    public float groundAccelerationTime = 1f;
    [Tooltip("How much time it takes to stop from max speed horizontally when there is input in the opposite direction while on the ground. This number is effectively half  because it targets the opposite max speed.")]
    public float oppositeInputGroundDecelerationTime = 1f;
    [Tooltip("How much time it takes to stop from max speed horizontally when there is no input while on the ground.")]
    public float noInputGroundDecelerationTime = 1f;

    [Header("Air", order = 2)]
    [Tooltip("How much time it takes to reach max speed horizontally while in the air.")]
    public float airAccelerationTime = 1f;
    [Tooltip("How much time it takes to stop from max speed horizontally when there is input in the opposite direction while in the air. This number is effectively half  because it targets the opposite max speed.")]
    public float oppositeInputAirDecelerationTime = 1f;
    [Tooltip("How much time it takes to stop from max speed horizontally when there is no input while in the air.")]
    public float noInputAirDecelerationTime = 1f;

    [Header("Movement Multipliers", order = 3)]
    // these values can represent a "kind" of friction
    [Tooltip("Multiplier for acceleration time. 0 means no acceleration, 1 means normal acceleration. 0-1 means reduced time, 1+ means increased time.")]
    [Range(0,5)]
    public float accelerationTimeMultiplier = 1;
    [Tooltip("Multiplier for deceleration times (opposite and no input). 0 means no deceleration, 1 means normal deceleration. 0-1 means reduced time, 1+ means increased time.")]
    [Range(0, 5)]
    public float decelerationTimeMultiplier = 1;

    [Header("Idle", order = 4)]

    [Header("Running", order = 5)]

    [Header("Jumping", order = 6)]

    [Tooltip("Vertical force applied to the Rigidbody on jump.")]
    public float jumpForce = 100f;
    [Tooltip("Gravity multiplier while jumping after the jump button is released.")]
    [Range(0, 10)] 
    public float lowJumpMultiplier = 2f;

    [Header("Falling", order = 7)]

    [Tooltip("Gravity multiplier while falling.")]
    [Range(0, 10)]
    public float fallMultiplier = 1f;
    [Tooltip("Time the player can queue a jump before a collision with the ground")]
    public float forgiveJumpSeconds; // can help make jumping more responsive

    [Header("Sound", order = 8)]
    [Tooltip("Sound that is played when the player jumps.")]
    public AudioClip jumpSound;
    [Tooltip("Sound that is played when the player lands on the environment (becomes grounded).")]
    public AudioClip landSound;



    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        boxCollider = GetComponent<BoxCollider2D>();

        physicsCheck = GetComponent<PhysicsCheck>();

        jumpInitiated = false;

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

    public void OnBlasterFire(BlasterController blaster)
    {
        stateMachine.OnBlasterFire(blaster);
    }

    public void ChangeState(IPlayerMovementState newState)
    {
        stateMachine.ChangeState(newState);
    }

    // function inspired by Youtube user  "Lost Relic Games"
    public void ChangeAnimationState(string newState)
    {
        // stop the same animation from interrupting itself
        if (currentAnimationState == newState) return;

        // play the animation
        animator.Play(newState);

        // reassign the current state.
        currentAnimationState = newState;
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
        targetVelocity = horizontalMovementAxis * maxSpeed;
    }

    // applies jumping force and switches to the upward state
    public void Jump()
    {
        SoundUtils.PlaySound(audioSource, jumpSound);
        ChangeAnimationState(PlayerController.JUMP_ANIMATION);
        jumpInitiated = true;
        RigidBody.AddForce(new Vector2(0, jumpForce * Time.fixedDeltaTime), ForceMode2D.Impulse);
        ChangeState(new PlayerUpwardState());
        jumpInitiated = false;
    }

    // called when the player hits the ground and switches into a ground state.
    public void OnGrounded()
    {
        SoundUtils.PlaySound(audioSource, landSound);
    }

    public void GroundInputMove()
    {
        if (NoHorizontalInput())
        {
            return;
        }

        if (HeadingTowardsCurrentDirection())
        {
            TowardsInputGroundAccelerate();
        } else
        {
            OppositeInputGroundDecelerate();
        }
    }

    public void AirInputMove()
    {
        if (NoHorizontalInput())
        {
            return;
        }

        if (HeadingTowardsCurrentDirection())
        {
            TowardsInputAirAccelerate();
        } else
        {
            OppositeInputAirDecelerate();
        }
    }


    // speed up to maxSpeed
    private void TowardsInputGroundAccelerate()
    {
        PhysicsUtils.ApplyForceTowards(RigidBody, targetVelocity, groundAccelerationTime, accelerationTimeMultiplier);
    }

    // slow to stop. Target velocity is used instead of 0 because the curve is too slow when at 0.
    private void OppositeInputGroundDecelerate()
    {
        PhysicsUtils.ApplyForceTowards(RigidBody, targetVelocity, oppositeInputGroundDecelerationTime, decelerationTimeMultiplier);
    }

    public void NoInputGroundDecelerate()
    {
        PhysicsUtils.ApplyForceTowards(RigidBody, 0, noInputGroundDecelerationTime, decelerationTimeMultiplier);
    }

    private void TowardsInputAirAccelerate()
    {
        PhysicsUtils.ApplyForceTowards(RigidBody, targetVelocity, airAccelerationTime, accelerationTimeMultiplier);
    }

    private void OppositeInputAirDecelerate()
    {
        PhysicsUtils.ApplyForceTowards(RigidBody, targetVelocity, oppositeInputAirDecelerationTime, decelerationTimeMultiplier);
    }

    public void NoInputAirDecelerate()
    {
        PhysicsUtils.ApplyForceTowards(RigidBody, 0, noInputAirDecelerationTime, decelerationTimeMultiplier);
    }

    public void StopHorizontalMovement()
    {
        RigidBody.velocity.Set(0, RigidBody.velocity.y);
        //Debug.LogWarning("Stopped!");
    }

    public void SetGravityScale(float gravityScale)
    {
        RigidBody.gravityScale = gravityScale;
    }

    private void Flip()
    {
        //Debug.LogWarning("Flip!");
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

    public bool IsMovingUpward()
    {
        return physicsCheck.IsMovingUpward();
    }

    public bool IsFalling()
    {
        return physicsCheck.IsFalling();
    }

    public bool IsGrounded()
    {
        return physicsCheck.IsGrounded();
    }

    public bool NoHorizontalInput()
    {
        return horizontalMovementAxis == 0;
    }

    public bool IsHorizontallyMoving()
    {
        return physicsCheck.IsHorizontallyMoving();
    }


    // this update (due to how prevHorizontalMovementAxis is assigned)
    public bool ChangedInputMovementDirection()
    {
        if (NoHorizontalInput()) return false; // no change (in direction) when input stops.

        return rightHeld != isFacingRight;
    }


}
