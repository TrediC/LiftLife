using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace GameController
{
    public class LiftInvaderAI : MonoBehaviour
    {

        public ClickerController clicker;
        public LiftInvaderStates invaderState;
        public List<Transform> wayPoints;
        private float _Health = 5;
        bool _active = true;
        public float iHealth
        {
            get
            {
                return _Health;
            }
            set
            {
                _Health -= value; if (_Health <= 0)
                {
                    print("Enemy knocked down!");
                    invaderState = LiftInvaderStates.WalkTo;
                    DisableNavigation();
                    StartCoroutine(EnableNavigation(5f));
                    clicker.enemies.Remove(this.gameObject);
                    GetComponent<Rigidbody>().isKinematic = false;
                    GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1f, 1f) * 50f, ForceMode.VelocityChange);
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
            for (int t = 0; t < temp.Length; t++)
            {
                wayPoints.Add(temp[t].transform);
            }
            wayPoints = wayPoints.OrderBy(
                x => Vector3.Distance(this.transform.position, x.transform.position)).ToList();

            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        void Start()
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

        void Update()
        {
            if (_active)
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
        }

        public void OpenLift()
        {
            invaderState = LiftInvaderStates.OpenLift;
            navMeshAgent.isStopped = true;
            clicker.AddEnemy(this.gameObject);
        }

        public void Punched()
        {
            invaderState = LiftInvaderStates.WalkTo;
            clicker.enemies.Remove(this.gameObject);
        }

        void DisableNavigation()
        {
            GetComponent<NavMeshAgent>().enabled = false;
            _active = false;
        }

        IEnumerator EnableNavigation(float time)
        {
            yield return new WaitForSeconds(time);
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
            _active = true;
            _Health = 5f;
            print("Enemy woke up.");
        }
    }
}