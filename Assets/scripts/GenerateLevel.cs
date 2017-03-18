using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour {

	//hier kommen alle Map-Segmente rein
	public GameObject[] prefabs;
	//hier kommen die Hindernisse rein(bitte auf die Reihenfolge achten), jedoch ist nur das 0-te besonders
	public GameObject[] obsPrefs;
	//Hier kommen die im Bogen verlaufenden und gerade coins rein, je ein Gameobject mit den Münzen
	public GameObject[] coinPrefs;
	//Das Mapgenerator-Objekt
	MapGenerator mapGen;
	public GameObject playerObj;
	Transform playerPos;



	// Use this for initialization
	void Start () {
		//Übergeben der Mapstücke, Hindernisse und aufgereihten Coins
		mapGen = new MapGenerator (prefabs, obsPrefs, coinPrefs);
		mapGen.buildMap (0);

		playerPos = playerObj.GetComponent<Transform> ();


	}
	
	float tmpPos = 0;
	float tmpPos2 = 0;
	int dstrCnt = 0;
	// Update is called once per frame
	void FixedUpdate () {
		tmpPos2 = playerPos.position.z;
		if (tmpPos2 >= mapGen.generatedMapTiles[5].transform.position.z) {
			//mapGen.buildMap (tmpPos + 167.5199f/2);
			mapGen.buildMapNext (tmpPos2-32);
			//Destroy (mapGen.generatedMapTiles [dstrCnt++]);
			tmpPos = tmpPos2+ 16.71599f;
			Debug.Log ("destryed");

			//mapGen.destroyElementsAt (playerPos.position.z-10);
		}

	}



}

class MapGenerator {
	//wieviele mapstücke sollen generiert werden?
	public int mapLength = 15;

	//Hier werden alle Mapstücke, die generiert wurden, gespeichert
	public List<GameObject> generatedMapTiles;
	//Hier werden alle hindernisse, die generiert wurden, gespeichert
	public List<GameObject> generatedObstacles;
	//Hier werden die übergebenen Mapstücke gespeichert
	GameObject[] allLevelPrefabs;
	//Hier werden die übergebenen Hindernisse gespeichert
	GameObject[] allObstaclePrefabs;
	//Hier werden die übergebenen Coins gespeichert
	GameObject[] allCoinPrefabs;
    //In diesem Script wird v.a. ermittelt, wie breit eine Spur sein soll, und auf welcher x-position und auf welcher 
	//Spur der Spieler sein soll, wenn z.B. laneScriptObj.SwitchLaneLeft ausgeführt wird
	public LaneScript laneScriptObj;

	int prefCnt = 0;
	//Init
	public MapGenerator(GameObject[] gotAllLevelPrefabs, GameObject[] gotAllObsctaclesPrefs, GameObject[] gotAllCoinPrefabs) {
		allLevelPrefabs = gotAllLevelPrefabs;
		allObstaclePrefabs = gotAllObsctaclesPrefs;
		allCoinPrefabs = gotAllCoinPrefabs;
		generatedMapTiles = new List<GameObject> ();
		generatedObstacles = new List<GameObject> ();

		laneScriptObj = new LaneScript (allLevelPrefabs[0]);
	}

	public void buildMapNext(float whereToBuild) {
		prefCnt++;
		destroyElementsAt (whereToBuild);
		GameObject.Destroy (generatedMapTiles[0]);
		generatedMapTiles.RemoveAt (0);
		float prefSize = allLevelPrefabs[0].GetComponentInChildren<MeshRenderer>().bounds.size.z;

		int genMapLast = generatedMapTiles.Count - 1;
		generatedMapTiles.Add(GameObject.Instantiate(allLevelPrefabs[0], new Vector3(0, 0, generatedMapTiles[genMapLast].transform.position.z+prefSize), allLevelPrefabs[0].transform.rotation));
		Debug.Log ("gennr" + generatedMapTiles.Count);
		//Es soll nur jeden 2. Mapabschnitt ein Hinderniss generiert werden
		if (prefCnt%3==0) {
			int rndLane = Random.Range (0, 3);
			int[] laneConstel = createObstacles(rndLane, generatedMapTiles[genMapLast].transform.position.z);
			createCoins(laneConstel, generatedMapTiles[genMapLast].transform.position.z);
		}

	}

	public void buildMap(float whereToBuild) {


		for(int i = 0; i < mapLength; i++){
			float prefSize = allLevelPrefabs[0].GetComponentInChildren<MeshRenderer>().bounds.size.z;
			float posZtoGen = (float)(whereToBuild + prefSize * (float)i);
			generatedMapTiles.Add(GameObject.Instantiate(allLevelPrefabs[0], new Vector3(0, 0, posZtoGen), allLevelPrefabs[0].transform.rotation));
			//Es soll nur jeden 2. Mapabschnitt ein Hinderniss generiert werden
			if (i > 0 && i%3==0) {
				int rndLane = Random.Range (0, 3);
				int[] laneConstel = createObstacles(rndLane, posZtoGen);
				createCoins(laneConstel, posZtoGen);
			}

		}

	}

	//Hier wird die Bahn und die z-Position wo die Coins generiert werden sollen übergeben...ist noch etwas buggy bzw. unübersichtlich
	public void createCoins(int[] laneConstel, float pos) {
		List<int> tmpList = new List<int>();
		for (int i = 0; i < laneConstel.Length; i++) {
			if (laneConstel [i] > 0 && laneConstel[i] < 4) {
				tmpList.Add (i);
			}
		}
		int rndCoinLane = Random.Range (0, tmpList.Count);
		if (laneConstel [rndCoinLane] == 1 || laneConstel [rndCoinLane] == -1) {
			generatedObstacles.Add(GameObject.Instantiate (allCoinPrefabs [1], new Vector3 (laneScriptObj.lanesX [rndCoinLane], 0, pos), allCoinPrefabs [1].transform.rotation));

		} else if(laneConstel [rndCoinLane] != 0) {
			generatedObstacles.Add(GameObject.Instantiate (allCoinPrefabs [0], new Vector3 (laneScriptObj.lanesX [rndCoinLane], 0, pos), allCoinPrefabs [0].transform.rotation));

		}
	}

	//Hier werden die Hindernisse erzeugt
	public int[] createObstacles(int whichLane, float pos) {
		int whichObsOnFreeLane = Random.Range (1, 4);
		GameObject obs = allObstaclePrefabs [whichObsOnFreeLane];

		int[] laneConstellation = new int[4];
		if (whichObsOnFreeLane == 1) { // wenn whichObs = 'slide'
			int[] rndBorder = new int[3];
			for (int i = 0; i < 4; i++) { // 3 mal 'Stamm3' erzeugen
				
				if (i != whichLane) {
					generatedObstacles.Add(GameObject.Instantiate (allObstaclePrefabs [0], new Vector3 (laneScriptObj.lanesX [i], 0f, pos), allObstaclePrefabs [0].transform.rotation));	
					laneConstellation [i] = 0;
				} else {
					laneConstellation [i] = 1;
				}
			}
			generatedObstacles.Add(GameObject.Instantiate (obs, new Vector3 (laneScriptObj.lanesX [whichLane], 0f, pos), obs.transform.rotation));


		} else if(whichObsOnFreeLane > 1) {  //alles ausser 'Stamm3' und 'slide'
			for (int i = 0; i < 4; i++) {
				if (i != whichLane) {
					int whichObsOnOccLane = Random.Range (-1, 6);
					if (whichObsOnOccLane != -1) {
						GameObject obsOcc = allObstaclePrefabs [whichObsOnOccLane];
						generatedObstacles.Add(GameObject.Instantiate (obsOcc, new Vector3 (laneScriptObj.lanesX [i], 0f, pos), obsOcc.transform.rotation));

						laneConstellation [i] = whichObsOnOccLane;
					}
				} else {
					int whichObsOnOccLane = Random.Range (1, 6);
					GameObject obsOcc = allObstaclePrefabs [whichObsOnOccLane];
					generatedObstacles.Add(GameObject.Instantiate (obsOcc, new Vector3 (laneScriptObj.lanesX [i],0f, pos), obsOcc.transform.rotation));

					laneConstellation [i] = whichObsOnOccLane;
				}
			} 

		}

		return laneConstellation;
	}

	public void destroyElementsAt(float whereToDestroy) {
		/*GameObject[] tmpFPs = GameObject.FindGameObjectsWithTag ("ForestPart");
		for (int j = 0; j < tmpFPs.Length; j++) {
			if(j == whereToDestroy) {
				GameObject.Destroy (tmpFPs[j]);
				generatedMapTiles.RemoveAt (j);
			}
		}*/
		for(int j=0; j<generatedObstacles.Count;j++) {
			if(generatedObstacles[j].transform.position.z < whereToDestroy) {
				GameObject.Destroy (generatedObstacles[j]);
				generatedObstacles.RemoveAt (j);
			}
		}
	}
}


