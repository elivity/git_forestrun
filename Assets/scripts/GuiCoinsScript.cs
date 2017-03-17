using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiCoinsScript : MonoBehaviour {


	public GameObject coinsPicked;
	Text textCoins;
	// Use this for initialization
	void Start () {
		textCoins = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		textCoins.text = GameStateManager.Instance.getCollectedCoins ().ToString();
	}
}
