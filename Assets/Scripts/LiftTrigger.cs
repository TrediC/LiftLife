using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Invader"))
            other.gameObject.GetComponent<LiftInvaderAI>().OpenLift();
    }
}
