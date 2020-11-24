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
        if (jumpPressed) player.ChangeState(new PlayerJumpingState());
        if (player.IsFalling()) player.ChangeState(new PlayerFallingState());
        ExitOnStop(player);
    }

    public void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
        
    }

    public void OnCollisionExit2D(PlayerController player, Collision2D collision)
    {
        
    }


    private void ExitOnStop(PlayerController player)
    {
        if (player.horizontalMovementAxis == 0 && !player.IsHorizontallyMoving())
        {
            player.ChangeState(new PlayerIdleState());
        }
    }
}
