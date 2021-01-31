using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterReadyState : IBlasterState
{

    private bool firePressed;

    public void EnterState(BlasterController blaster)
    {
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
            blaster.ChangeState(new BlasterReloadState());
            return;
        }
    }

    public void OnBlasterFire(BlasterController blaster)
    {
        
    }

}
