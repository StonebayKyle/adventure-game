using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterReloadState : IBlasterState
{
    public void EnterState(BlasterController blaster)
    {

    }

    public void ExitState(BlasterController blaster)
    {
        
    }

    public void Update(BlasterController blaster)
    {
    }

    public void FixedUpdate(BlasterController blaster)
    {
        if (!blaster.BlasterFiredInAir)
        {
            blaster.ChangeState(new BlasterReadyState());
        }
        
    }

    public void OnBlasterFire(BlasterController blaster)
    {
        
    }
}
