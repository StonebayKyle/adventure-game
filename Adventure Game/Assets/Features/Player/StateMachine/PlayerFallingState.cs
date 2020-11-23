using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : IPlayerMovementState
{
    private float initialGravityScale;
    public void EnterState(PlayerController player)
    {
        Debug.LogWarning("Falling!");
        initialGravityScale = player.RigidBody.gravityScale;
        // increase gravity to get a more responsive jump
        player.RigidBody.gravityScale *= player.fallMultiplier;
        //Debug.Log("Initial gravity scale: " + initialGravityScale + "\tActual gravity scale: " + player.RigidBody.gravityScale);
    }

    public void ExitState(PlayerController player)
    {
        player.RigidBody.gravityScale = initialGravityScale;
    }

    public void Update(PlayerController player)
    {
        //Debug.Log(this + " updated");
    }

    public void FixedUpdate(PlayerController player)
    {
        if (player.IsGrounded()) player.ChangeState(new PlayerIdleState());
    }

    public void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
    }

    public void OnCollisionExit2D(PlayerController player, Collision2D collision)
    {
        
    }

}
