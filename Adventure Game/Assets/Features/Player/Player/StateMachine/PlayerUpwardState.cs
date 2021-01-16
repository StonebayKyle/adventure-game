using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpwardState : IPlayerMovementState
{
    private bool jumpHeld;
    private float initialGravityScale;

    public void EnterState(PlayerController player)
    {
        jumpHeld = true; // jump set to true initially (even if entered by something other than jump) to ensure the gravity isn't reset the same moment this state is entered, disallowing them from doing a high jump. TODO: Test this
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
        if (jumpHeld)
        {
            // resets gravity back to intital when the player holds jump
            //player.RigidBody.gravityScale = initialGravityScale;
        }
        else
        {
            // increase gravity so player doesn't jump as high
            player.RigidBody.gravityScale = initialGravityScale * player.lowJumpMultiplier;
        }

        if (player.IsFalling())
        {
            player.ChangeState(new PlayerFallingState());
        }


        // TODO: set the gravity scale to low-jump or an inbetween value when the laser is fired (and don't let them go back to high-jump)
    }

    public void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {

    }

    public void OnCollisionExit2D(PlayerController player, Collision2D collision)
    {

    }
}
