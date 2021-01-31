using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : IPlayerMovementState
{
    private bool jumpPressedRecently;
    private float initialGravityScale;

    private Timer forgiveJumpTimer;

    public void EnterState(PlayerController player)
    {
        jumpPressedRecently = false;
        forgiveJumpTimer = new Timer(player.forgiveJumpSeconds);
        //Debug.LogWarning("Falling!");
        // increase gravity to get a more responsive jump
        initialGravityScale = player.RigidBody.gravityScale;
        player.RigidBody.gravityScale *= player.fallMultiplier;
        //Debug.Log("Initial gravity scale: " + initialGravityScale + "\tActual gravity scale: " + player.RigidBody.gravityScale);
    }

    public void ExitState(PlayerController player)
    {
        player.RigidBody.gravityScale = initialGravityScale;
    }

    public void Update(PlayerController player)
    {
        UpdateForgiveJump(player);
    }

    public void FixedUpdate(PlayerController player)
    {
        player.AirInputMove();

        if (player.IsMovingUpward())
        {
            player.ChangeState(new PlayerUpwardState());
            return;
        }

        if (player.IsGrounded())
        {
            ChangeToGroundState(player);
            return;
        }

        if (!player.blasterFiredInAir && player.NoHorizontalInput())
        {
            player.NoInputAirDecelerate();
        }
    }

    public void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
        if (player.IsGrounded())
        {
            if (jumpPressedRecently)
            {
                player.Jump();
                return;
            }
            ChangeToGroundState(player);
            return;
        }
    }

    public void OnCollisionExit2D(PlayerController player, Collision2D collision)
    {
        
    }

    private void UpdateForgiveJump(PlayerController player)
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressedRecently = true;
            forgiveJumpTimer.Restart();
        }

        if (jumpPressedRecently)
        {
            forgiveJumpTimer.Tick(Time.deltaTime);
            if (forgiveJumpTimer.Completed())
            {
                jumpPressedRecently = false;
            }
        }
    }

    private void ChangeToGroundState(PlayerController player)
    {
        player.blasterFiredInAir = false;
        if (player.IsHorizontallyMoving())
        {
            player.ChangeState(new PlayerRunningState());
        } else
        {
            player.ChangeState(new PlayerIdleState());
        }
    }

    public void OnBlasterFire(PlayerController player, BlasterController blaster)
    {
        player.blasterFiredInAir = true;
    }
}
