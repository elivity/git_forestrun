using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGenerateScene : MonoBehaviour {

	public GameObject[] prefabs;
	public GameObject[] obsPrefs;
	public GameObject[] coinPrefs;
	MenuMapGenerator mapGen;
	Vector3 playerPos;



	// Use this for initialization
	void Start () {
		mapGen = new MenuMapGenerator(prefabs, obsPrefs, coinPrefs);
		mapGen.buildMap ();


	}

	// Update is called once per frame
	void Update () {
		if (playerPos.z > 5) {

		}
	}



}

class MenuMapGenerator {
	public int mapLength = 100;

	GameObject[] generatedMapTiles;
	GameObject[] allLevelPrefabs;
	GameObject[] allObstaclePrefabs;
	GameObject[] allCoinPrefabs;
	public LaneScript laneScriptObj;

	public MenuMapGenerator(GameObject[] gotAllLevelPrefabs, GameObject[] gotAllObsctaclesPrefs, GameObject[] gotAllCoinPrefabs) {
		allLevelPrefabs = gotAllLevelPrefabs;
		allObstaclePrefabs = gotAllObsctaclesPrefs;
		allCoinPrefabs = gotAllCoinPrefabs;
		generatedMapTiles = new GameObject[100];

		laneScriptObj = new LaneScript (allLevelPrefabs[0]);
	}

	public void buildMap() {
		for(int i = 0; i < mapLength; i++){

			float prefSize = allLevelPrefabs[0].GetComponentInChildren<MeshRenderer>().bounds.size.z;
			float posZtoGen = (float)(prefSize * (float)i);
			generatedMapTiles[i] = GameObject.Instantiate(allLevelPrefabs[0], new Vector3(0, 0, posZtoGen), allLevelPrefabs[0].transform.rotation);

		}

	}


	public void createCoins(int[] laneConstel, float pos) {
		List<int> tmpList = new List<int>();
		for (int i = 0; i < laneConstel.Length; i++) {
			if (laneConstel [i] > 0 && laneConstel[i] < 4) {
				tmpList.Add (i);
			}
		}
		int rndCoinLane = Random.Range (0, tmpList.Count);
		if (laneConstel [rndCoinLane] == 1 || laneConstel [rndCoinLane] == -1) {
			GameObject.Instantiate (allCoinPrefabs [1], new Vector3 (laneScriptObj.lanesX [rndCoinLane], 5, pos), allCoinPrefabs [1].transform.rotation);

		} else if(laneConstel [rndCoinLane] != 0) {
			GameObject.Instantiate (allCoinPrefabs [0], new Vector3 (laneScriptObj.lanesX [rndCoinLane], 5, pos), allCoinPrefabs [0].transform.rotation);

		}
	}

}


