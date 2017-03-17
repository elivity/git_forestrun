using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneScript {
	int laneCount = 4; //

	static int currentLane = 1;
	GameObject mapPref;
	public float[] lanesX = new float[4];

	public LaneScript(GameObject mpf) {
		mapPref = mpf;
		lanesX = calcLanesMiddles(mapPref.transform.position.x, mapPref.GetComponent<MeshRenderer>().bounds.size.x);
		Debug.Log(lanesX[1]);
	}

	public float[] calcLanesMiddles(float trackPosX, float trackWidth) {
		float laneWidth = trackWidth / 8;
		float laneMedian = laneWidth / 2;
		float startTrack = (trackWidth / 4);
		float[] arr = new float[4];
		arr [0] = trackPosX-startTrack + laneMedian;
		arr [1] = trackPosX-startTrack + 1*laneWidth + laneMedian;
		arr [2] = trackPosX-startTrack + 2*laneWidth + laneMedian;
		arr [3] = trackPosX-startTrack + 3*laneWidth + laneMedian;
		return arr;
	}

	public float switchLaneLeft() {
		int newLane = currentLane;
		if (currentLane > 0) {
			currentLane--;
			newLane = currentLane;
		}
		return lanesX[newLane];
	}

	public float switchLaneRight() {
		int newLane = currentLane;
		if (currentLane < laneCount-1) {
			currentLane++;
			newLane = currentLane;
		}
		//Debug.Log (lanesX [newLane] + " " + newLane);
		return lanesX[newLane];
	}

}
