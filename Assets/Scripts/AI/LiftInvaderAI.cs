using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftInvaderAI : MonoBehaviour {


    public LiftInvaderStates invaderState;
    public Transform[] wayPoints;

    [HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;
    [HideInInspector] public WalkTo walkToState;
    [HideInInspector] public OpenLift openLiftState;
    [HideInInspector] public ILiftInvader currentState;

    private void Awake()
    {
        walkToState = new WalkTo(this);
        openLiftState = new OpenLift(this);

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Start ()
    {
        switch (invaderState)
        {
            case LiftInvaderStates.WalkTo:
                currentState = walkToState;
                break;
            case LiftInvaderStates.OpenLift:
                currentState = openLiftState;
                break;
            default:
                break;
        }
	}
	
	void Update ()
    {
        if (currentState.ToString() != invaderState.ToString())
        {
            switch (invaderState)
            {
                case LiftInvaderStates.WalkTo:
                    invaderState = LiftInvaderStates.WalkTo;
                    currentState = walkToState;
                    break;
                case LiftInvaderStates.OpenLift:
                    invaderState = LiftInvaderStates.OpenLift;
                    currentState = openLiftState;
                    navMeshAgent.isStopped = true;
                    break;
                default:
                    break;
            }
        }
        
        currentState.UpdateState();
    }

    public void OpenLift()
    {
        invaderState = LiftInvaderStates.OpenLift;
        navMeshAgent.isStopped = true;
    }
}


