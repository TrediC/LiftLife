using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController
{
    public class LiftTrigger : MonoBehaviour
    {

        ClickerController cc;

        private void Start()
        {
            cc = GameObject.Find("GameController").GetComponent<ClickerController>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Invader"))
            {
                other.gameObject.GetComponent<LiftInvaderAI>().OpenLift();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Invader"))
                cc.isTimerRunning = true;
            else
            {
                cc.isTimerRunning = false;
            }
        }

    }
}