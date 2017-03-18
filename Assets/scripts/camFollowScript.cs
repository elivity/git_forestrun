using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollowScript : MonoBehaviour {
    public Transform playerTransform;
    public float camMoveSpeed = 1;

	void Start () {
		
	}

	void Update () {
        //wie bei Subway Surfer , die Cam nicht zu weit nach links/rechts positionieren
        float x = playerTransform.position.x;

        if (x<-18)  //ca. -27 ist ganz links
        {
            x = -18;
        }
        else if (x>18) //ca. 27 ist ganz rechts
        {
            x = 18; 
        }

        transform.position = Vector3.Lerp(transform.position,
            new Vector3(x,playerTransform.position.y+8,playerTransform.position.z-7), camMoveSpeed*Time.deltaTime);
	}

}
