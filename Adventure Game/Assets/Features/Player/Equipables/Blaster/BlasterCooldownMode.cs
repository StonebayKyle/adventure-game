using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlasterCooldownMode
{
    /// <summary>
    /// No cooldown.
    /// </summary>
    None = 0,
    /// <summary>
    /// Time-based cooldown
    /// </summary>
    Time = 1,
    /// <summary>
    /// Single-fire while in the air.
    /// </summary>
    GroundTouch = 2
}
