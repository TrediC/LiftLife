using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LiftInvaderAI : MonoBehaviour {

    public ClickerController clicker;
    public LiftInvaderStates invaderState;
    public List<Transform> wayPoints;
    private float _Health = 5;
    public float iHealth
    {
        get
        {
            return _Health;
        }
        set
        {
            _Health -= value; if(_Health <= 0)
            {
                invaderState = LiftInvaderStates.WalkTo;
            }
        }
    }


    [HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;
    [HideInInspector] public WalkTo walkToState;
    [HideInInspector] public OpenLift openLiftState;
    [HideInInspector] public ILiftInvader currentState;

    private void Awake()
    {
        walkToState = new WalkTo(this);
        openLiftState = new OpenLift(this);

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Waypoint");
        for(int t = 0; t < temp.Length; t++)
        {
            wayPoints.Add(temp[t].transform);
        }
        wayPoints = wayPoints.OrderBy(
            x => Vector3.Distance(this.transform.position, x.transform.position)).ToList();

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Start ()
    {
        clicker = GameObject.Find("GameController").GetComponent<ClickerController>();
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
        clicker.AddEnemy(this.gameObject);
    }
}


