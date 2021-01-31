using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpwardState : IPlayerMovementState
{
    private bool jumpHeld;
    private float initialGravityScale;
    private float lowJumpGravityScale;

    public void EnterState(PlayerController player)
    {
        jumpHeld = player.jumpInitiated; // this is true on Enter when the player jumped into UpwardState.
        player.blasterFiredInAir = !jumpHeld; // if upward entered by anything other than jump, assume it was blaster.

        //Debug.LogWarning("jumpHeld: " + jumpHeld);
        initialGravityScale = player.RigidBody.gravityScale;
        lowJumpGravityScale = initialGravityScale * player.lowJumpMultiplier;

        if (!player.jumpInitiated)
        {
            //Debug.LogWarning("Grav to low jump on ENTER");
            // sets gravity to low-jump whenever upward is entered by anything but a jump, as a "high-jump" should only be allowed during an actual jump.
            player.SetGravityScale(lowJumpGravityScale);
        }
    }

    public void ExitState(PlayerController player)
    {
        // resets gravity scale.
        player.SetGravityScale(initialGravityScale);
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
            // increase gravity so player doesn't jump as high when the jump button is released 
            player.SetGravityScale(lowJumpGravityScale);
        }

        if (!player.blasterFiredInAir && player.NoHorizontalInput())
        {
            // have no input deceleration if the player hasn't fired blaster while in air (and isn't inputting).
            player.NoInputAirDecelerate();
        }

        //Debug.LogWarning("Current Gravity Scale: " + player.RigidBody.gravityScale);

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

    public void OnBlasterFire(PlayerController player, BlasterController blaster)
    {
        player.blasterFiredInAir = true;
        //Debug.LogWarning("Blaster fire detected!");
        // increase gravity to low-jump gravity so the player can't launch themselves with the blaster and also high-jump
        player.SetGravityScale(lowJumpGravityScale);
    }
}
