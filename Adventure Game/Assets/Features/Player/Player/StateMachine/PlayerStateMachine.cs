using UnityEngine;

// code inspired by user KelsoMRK on Unity Forums
public class PlayerStateMachine
{
    private IPlayerMovementState currentState;

    private PlayerController player;

    // TODO make a list of static states that have no attributes to easily reuse for performance (only works without attributes since values would be object-based)

    public PlayerStateMachine(PlayerController player)
    {
        this.player = player;
    }

    public void ChangeState(IPlayerMovementState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(player);
            Debug.Log("Player exited [" + currentState + "]");
        }

        currentState = newState;
        currentState.EnterState(player);
        Debug.Log("Player entered [" + currentState + "]");
    }

    public void Update()
    {
        if (currentState != null) currentState.Update(player);
    }

    public void FixedUpdate()
    {
        if (currentState != null) currentState.FixedUpdate(player);
    }

    public void OnCollisionEnter(Collision2D collision)
    {
        if (currentState != null) currentState.OnCollisionEnter2D(player, collision);
    }

    public void OnCollisionExit(Collision2D collision)
    {
        if (currentState != null) currentState.OnCollisionExit2D(player, collision);
    }

    public void OnBlasterFire(BlasterController blaster)
    {
        if (currentState != null) currentState.OnBlasterFire(player, blaster);
    }
}
