public class Timer
{
    /// <summary>
    /// How much time is left in the timer.
    /// </summary>
    public float TimeLeft { get; private set; }

    /// <summary>
    /// Timer won't tick while paused.
    /// </summary>
    public bool IsPaused { get; set; }

    /// <summary>
    /// How long the timer is.
    /// </summary>
    private float timerLength;

    /// <summary>
    /// Creates a new timer.
    /// </summary>
    /// <param name="timerLength">Starting time of the timer</param>
    public Timer(float timerLength)
    {
        this.timerLength = timerLength;
        TimeLeft = timerLength;
        IsPaused = false;
    }

    /// <summary>
    /// Ticks the timer when not paused.
    /// </summary>
    /// <param name="timePassed">How much to reduce TimeLeft by. For example, Time.deltaTime for Update</param>
    /// <returns>true if the timer is completed.</returns>
    /// <value>On timer completion, the timer is paused.</value>
    public bool Tick(float timePassed)
    {
        if (!IsPaused) TimeLeft -= timePassed;

        if (Completed())
        {
            IsPaused = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Resets TimeLeft to timerLength, preserving <see cref="IsPaused"/> status
    /// </summary>
    /// /// <param name="timerLength"></param>
    public void Reset(float timerLength)
    {
        this.timerLength = timerLength;
        TimeLeft = timerLength;
    }

    /// <summary>
    /// Resets TimeLeft to its previous timerLength, preserving <see cref="IsPaused"/> status
    /// </summary>
    public void Reset()
    {
        Reset(timerLength);
    }

    /// <summary>
    /// Resets TimeLeft to timerLength and starts timer again.
    /// </summary>
    /// <param name="timerLength"></param>
    public void Restart(float timerLength)
    {
        Reset(timerLength);
        IsPaused = false;
    }

    /// <summary>
    /// Resets TimeLeft to its previous timerLength and starts the timer again.
    /// </summary>
    public void Restart()
    {
        Restart(timerLength);
    }

    /// <summary>
    /// Whether or not the timer is completed.
    /// </summary>
    /// <returns>true if the timer is completed</returns>
    public bool Completed()
    {
        return TimeLeft <= 0;
    }

}
