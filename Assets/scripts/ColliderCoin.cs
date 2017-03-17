using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderCoin : MonoBehaviour {

    public GameObject coinParticle;

	public Text textCoins;

	void OnTriggerEnter(Collider collider) {
		Debug.Log ("Coin collieded");
			if (collider.gameObject.tag == "Player") {
				GameStateManager.Instance.collectCoin ();
				textCoins.text = GameStateManager.Instance.getCollectedCoins ().ToString();
          		  //Particle Effekt 
                GameObject particleClone = Instantiate(coinParticle, transform.position, Quaternion.identity);
				
                Destroy(particleClone, 0.8f);

                Destroy (gameObject);

				
			}

	}
}
