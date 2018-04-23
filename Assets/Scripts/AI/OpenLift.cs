using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLift : ILiftInvader
{
    private readonly LiftInvaderAI liftInvaderAI;

    public OpenLift(LiftInvaderAI liftInvaderAI)
    {
        this.liftInvaderAI = liftInvaderAI;
    }

    public void UpdateState()
    {
        
    }

    public void WalkTo()
    {
        liftInvaderAI.currentState = liftInvaderAI.walkToState;
    }

    void ILiftInvader.OpenLift()
    {

    }
}
