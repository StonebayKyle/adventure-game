using UnityEngine;

public class PlayerJumpingState : IPlayerMovementState
{
    private bool jumpHeld;
    private float initialGravityScale;

    public void EnterState(PlayerController player)
    {
        jumpHeld = true;
        player.RigidBody.AddForce(new Vector2(0, player.jumpForce * Time.fixedDeltaTime),ForceMode2D.Impulse);
        initialGravityScale = player.RigidBody.gravityScale;
    }

    public void ExitState(PlayerController player)
    {
        player.RigidBody.gravityScale = initialGravityScale;
    }

    public void Update(PlayerController player)
    {
        jumpHeld = Input.GetButton("Jump");
        //Debug.Log(this + " updated");
    }

    public void FixedUpdate(PlayerController player)
    {
        if (!jumpHeld)
        {
            // increase gravity so player doesn't jump as high
            player.RigidBody.gravityScale *= player.lowJumpMultiplier;
        }

        if (player.IsFalling())
        {
            player.ChangeState(new PlayerFallingState());
        }
    }

    public void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
      
    }

    public void OnCollisionExit2D(PlayerController player, Collision2D collision)
    {
        
    }
}
