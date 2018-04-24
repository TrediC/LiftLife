using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource music;
    public AudioSource buttonClick;
    public AudioSource doorClose;
    public AudioSource dingSound;
    public AudioSource elevatorBgNoise;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayButtonSound()
    {
        buttonClick.Stop();
        buttonClick.Play();
    }

    public void PlayDoorClosingSound()
    {
        doorClose.Play();
    }

    public void PlayDingSound()
    {
        dingSound.Play();
    }

    public void PlayElevatorNoise(float delay)
    {
        elevatorBgNoise.PlayDelayed(delay);
    }

    public void StopElevatorNoise()
    {
        elevatorBgNoise.Stop();
    }
}
