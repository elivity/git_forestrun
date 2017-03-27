using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStartScript : MonoBehaviour {

	public Animator lumberAnimator;
	public Animator stickAnimator;
	public Animator sphereAnimator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void playLumber() {
		lumberAnimator.SetTrigger("started");
	}

	void playStick() {
		stickAnimator.SetTrigger("started");
	}

	void playSphere() {
		sphereAnimator.SetTrigger("started");
	}
}
