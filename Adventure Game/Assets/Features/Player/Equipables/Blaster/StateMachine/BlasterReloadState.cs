using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterReloadState : IBlasterState
{
    private Timer timer;
    private bool timerCompleted;

    public void EnterState(BlasterController blaster)
    {
        blaster.ChangeAnimationState(BlasterController.IDLE_ANIMATION); // opposite because reload looks like when it becomes reloaded, not while it is reloading or not able to fire.
        timer = new Timer(blaster.cooldownTime);
        timerCompleted = false;
    }

    public void ExitState(BlasterController blaster)
    {
        
    }

    public void Update(BlasterController blaster)
    {
        if (blaster.cooldownMode == BlasterCooldownMode.Time)
        {
            timer.Tick(Time.deltaTime);
            if (timer.Completed())
            {
                timerCompleted = true;
            }
        }
    }

    public void FixedUpdate(BlasterController blaster)
    {
        if (blaster.cooldownMode == BlasterCooldownMode.Time && timerCompleted)
        {
            blaster.ChangeState(new BlasterReadyState());
            return;
        }

        if (blaster.cooldownMode == BlasterCooldownMode.GroundTouch && !blaster.BlasterFiredInAir)
        {
            blaster.ChangeState(new BlasterReadyState());
            return;
        }
        
    }

    public void OnBlasterFire(BlasterController blaster)
    {
        
    }
}
