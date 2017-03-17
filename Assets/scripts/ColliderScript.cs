using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour {
	public Animator animOfPlayer;

	void OnTriggerEnter (Collider collider) {
		GameStateManager.Instance.deleteOneLife ();
        Debug.Log("obstacle collided with: "+ collider.gameObject.tag);

        //Nur Collider am Skelett beachten. Nur Skelett hat "Player" tag
		if (collider.tag.Equals("Player"))
        {
            //I am Stone
			if (gameObject.name.Equals("colliderOfStone"))
            {
                if (animOfPlayer.GetCurrentAnimatorStateInfo(0).IsName("Base.roll"))
                {
                    //play player Hit-Wall Animation
                }
                else
                {
                    animOfPlayer.SetTrigger("TriggerStumbleStone");
                }
            }
            else
            {
                animOfPlayer.SetTrigger("TriggerStumbleStone");
                //GameStateManager.Instance.setSpeed (0);
                //StartCoroutine (waitAndReset());
            }
        }
	}

	IEnumerator waitAndReset() {
		yield return new WaitForSeconds (1);
		GameStateManager.Instance.restartLevel ();
	}
}
