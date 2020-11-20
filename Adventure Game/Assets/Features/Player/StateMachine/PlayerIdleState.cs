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
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
    }
    public void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {

    }

    public void OnCollisionExit2D(PlayerController player, Collision2D collision)
    {
        
    }

    public void FixedUpdate(PlayerController player)
    {
        if (jumpPressed) player.ChangeState(new PlayerJumpState());
    }

    public override string ToString()
    {
        return "Player Idle";
    }

}
