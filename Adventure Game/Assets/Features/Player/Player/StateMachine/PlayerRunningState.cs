using UnityEngine;

public class PlayerRunningState : IPlayerMovementState
{
    private bool jumpPressed;

    public void EnterState(PlayerController player)
    {
        jumpPressed = false;
    }

    public void ExitState(PlayerController player)
    {
        
    }

    public void Update(PlayerController player)
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
            //Debug.LogWarning(this + "detected jump button!");
        }
    }

    public void FixedUpdate(PlayerController player)
    {
        if (jumpPressed)
        {
            player.Jump();
            return;
        }
        if (player.IsFalling())
        {
            player.ChangeState(new PlayerFallingState());
            return;
        }
        if (player.IsMovingUpward())
        {
            player.ChangeState(new PlayerUpwardState());
            return;
        }

        if (player.NoHorizontalInput())
        {
            player.NoInputGroundDecelerate();
            //Debug.LogWarning("Decelerating!");
        }

        if (player.NoHorizontalInput() && !player.IsHorizontallyMoving())
        {
            player.ChangeState(new PlayerIdleState());
            return;
        }
    }

    public void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
        
    }

    public void OnCollisionExit2D(PlayerController player, Collision2D collision)
    {
        
    }

    public void OnBlasterFire(PlayerController player, BlasterController blaster)
    {
        
    }
}
