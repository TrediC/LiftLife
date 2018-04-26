using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public Transform Button;
    public Vector3 offset;
	void Start () {
        offset = transform.position - Button.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Button.transform.position + offset;
	}
}
