using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTrigger : MonoBehaviour {

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
            cc.isTimerRunning = true;
        }
    }
}
