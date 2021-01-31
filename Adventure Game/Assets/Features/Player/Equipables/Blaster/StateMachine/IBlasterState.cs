// TODO inheritance for base state machine functionality.
public interface IBlasterState
{
    /// <summary>
    /// Runs when entering the state
    /// </summary>
    void EnterState(BlasterController blaster);

    /// <summary>
    /// Runs when exiting the state
    /// </summary>
    void ExitState(BlasterController blaster);

    /// <summary>
    /// Called every Update. Use to detect inputs and do frame-based actions
    /// </summary>
    void Update(BlasterController blaster);

    /// <summary>
    /// Called every FixedUpdate. Use for physics
    /// </summary>
    void FixedUpdate(BlasterController blaster);

    /// <summary>
    /// Called every time the blaster is fired.
    /// </summary>
    void OnBlasterFire(BlasterController blaster);
}
