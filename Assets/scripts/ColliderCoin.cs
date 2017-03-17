using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCoin : MonoBehaviour {

    public GameObject coinParticle;

	void OnTriggerEnter(Collider collider) {
		Debug.Log ("Coin collieded");
			if (collider.gameObject.tag == "Player") {
				GameStateManager.Instance.collectCoin ();
            //Particle Effekt 
                GameObject particleClone = Instantiate(coinParticle, transform.position, Quaternion.identity);
                Destroy(particleClone, 0.8f);

                Destroy (gameObject);
			}

	}
}
