using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterStateMachine
{

    private IBlasterState currentState;

    private BlasterController blaster;

    public BlasterStateMachine(BlasterController blaster)
    {
        this.blaster = blaster;
    }

    public void ChangeState(IBlasterState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(blaster);
            //Debug.Log("Player exited [" + currentState + "]");
        }

        currentState = newState;
        currentState.EnterState(blaster);
        //Debug.Log("Player entered [" + currentState + "]");
    }


    public void Update()
    {
        if (currentState != null) currentState.Update(blaster);
    }

    public void FixedUpdate()
    {
        if (currentState != null) currentState.FixedUpdate(blaster);
    }

    public void OnBlasterFire(BlasterController blaster)
    {
        if (currentState != null) currentState.OnBlasterFire(blaster);
    }
}
