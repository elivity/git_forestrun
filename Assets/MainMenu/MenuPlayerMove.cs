using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerMove : MonoBehaviour {
	Vector3 playerShouldPos;
	// Use this for initialization
	void Start () {
		playerShouldPos = gameObject.transform.position;
		playerShouldPos.y = -3.3f;
	}
	
	// Update is called once per frame
	void Update () {
		playerShouldPos.z += 2f;	

		if (playerShouldPos.z < 400f) { // Bounds
			gameObject.transform.position = (Vector3.Lerp (gameObject.transform.position, playerShouldPos, Time.deltaTime * 4));

		} else {
			playerShouldPos.z = -100f;
			gameObject.transform.position = playerShouldPos;

		}
	}
}
