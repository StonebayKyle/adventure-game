using UnityEngine;

public interface IPlayerMovementState
{
    /// <summary>
    /// Runs when entering the state
    /// </summary>
    void EnterState(PlayerController player);

    /// <summary>
    /// Runs when exiting the state
    /// </summary>
    void ExitState(PlayerController player);

    /// <summary>
    /// Called every Update. Use to detect inputs and do frame-based actions
    /// </summary>
    void Update(PlayerController player);

    /// <summary>
    /// Called every FixedUpdate. Use for physics or when something phyiscs-related happens in EnterState (i.e. jumping)
    /// </summary>
    void FixedUpdate(PlayerController player);

    /// <summary>
    /// Called every time player enters a collision
    /// </summary>
    void OnCollisionEnter2D(PlayerController player, Collision2D collision);

    /// <summary>
    /// Called every time player exits a collision
    /// </summary>
    void OnCollisionExit2D(PlayerController player, Collision2D collision);

}
