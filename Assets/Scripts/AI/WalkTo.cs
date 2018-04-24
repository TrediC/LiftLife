using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkTo : ILiftInvader {
    private readonly LiftInvaderAI liftInvaderAI;
    private int nextWayPoint;

    public WalkTo(LiftInvaderAI liftInvaderAI)
    {
        this.liftInvaderAI = liftInvaderAI;
    }

    public void OpenLift()
    {
        liftInvaderAI.navMeshAgent.isStopped = true;
        liftInvaderAI.currentState = liftInvaderAI.openLiftState;
    }

    public void UpdateState()
    {
        Walk();
    }

    void Walk()
    {
        liftInvaderAI.navMeshAgent.destination = liftInvaderAI.wayPoints[nextWayPoint].position;
        liftInvaderAI.navMeshAgent.isStopped = false;
        if (liftInvaderAI.navMeshAgent.remainingDistance <= liftInvaderAI.navMeshAgent.stoppingDistance
            && !liftInvaderAI.navMeshAgent.pathPending)
        {
            nextWayPoint = (nextWayPoint + 1) % liftInvaderAI.wayPoints.Count;
        }
    }


    void ILiftInvader.WalkTo()
    {

    }
}
