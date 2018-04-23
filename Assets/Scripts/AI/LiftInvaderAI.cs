using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftInvaderAI : MonoBehaviour {

    [HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;

    public LiftInvaderStates invaderState;
    public Transform[] wayPoints;
    public GameObject lift;

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
        currentState.UpdateState();

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lift"))
            currentState = openLiftState;
    }
}


