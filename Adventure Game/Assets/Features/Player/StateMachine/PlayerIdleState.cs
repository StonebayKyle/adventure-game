using UnityEngine;

public class PlayerIdleState : IPlayerMovementState
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
        //Debug.Log(this + " updated");

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
            //Debug.LogWarning(this + "detected jump button!");
        }

    }
    public void FixedUpdate(PlayerController player)
    {
        if (jumpPressed) player.ChangeState(new PlayerJumpingState());

        //Debug.Log("Player y velocity: " + player.RigidBody.velocity.y);
        if (player.IsFalling())
        {
            player.ChangeState(new PlayerFallingState());
        }

        if (player.horizontalMovementAxis != 0 || player.IsHorizontallyMoving())
        {
            player.ChangeState(new PlayerRunningState());
        }
    }

    public void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {

    }

    public void OnCollisionExit2D(PlayerController player, Collision2D collision)
    {
        
    }
}
