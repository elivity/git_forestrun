using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bearParticle : MonoBehaviour {

    public GameObject bearSkelett;
    public GameObject starParticle;

    //Bear Anim Event
    public void createParticle()
    {
        //instantiate particle at bearskelett position
        GameObject clone = Instantiate(starParticle, new Vector3(bearSkelett.transform.position.x - 84,
            bearSkelett.transform.position.y, bearSkelett.transform.position.z), Quaternion.identity);

        Destroy(clone, 1.5f);

        //will also destroy all children (ex. the first particle we parented to this object in 'bearScript.cs') 
        Destroy(gameObject);
    }
}
