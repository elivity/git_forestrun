using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiDistanceScript : MonoBehaviour {

	public GameObject distanceRun;
	Text textDistance;
	// Use this for initialization
	void Start () {
		textDistance = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		//textDistance.text = GameStateManager.Instance.getDistance ().ToString();
	}
}
