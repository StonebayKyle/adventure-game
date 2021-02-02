using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterReadyState : IBlasterState
{

    private bool firePressed;

    public void EnterState(BlasterController blaster)
    {
        blaster.ChangeAnimationState(BlasterController.RELOAD_ANIMATION); // opposite because reload looks like when it becomes reloaded, not while it is reloading or not able to fire.
        firePressed = false;
    }

    public void ExitState(BlasterController blaster)
    {
        
    }

    public void Update(BlasterController blaster)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firePressed = true;
        }
    }

    public void FixedUpdate(BlasterController blaster)
    {
        if (firePressed)
        {
            //Debug.LogWarning("Fire activated in FixedUpdate");
            firePressed = false;
            blaster.Fire();
            if (blaster.cooldownMode != BlasterCooldownMode.None)
            {
                blaster.ChangeState(new BlasterReloadState());
                return;
            }
        }
    }

    public void OnBlasterFire(BlasterController blaster)
    {
        
    }

}
