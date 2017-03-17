using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bearScript : MonoBehaviour {

    public GameObject particlePrefab;
    private Animator bearAnimCtrl;
    int frameCounter = 0;
    Vector3 bearPosition;

	void Start () {
        bearAnimCtrl = GetComponentInParent<Animator>();
	}
	
	void Update () {
        if (frameCounter == 5)
        {
            frameCounter = 0;

        }
        else
        {
            frameCounter++;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bär Trigger");
        //get Player Animator and check if Kick-Anim is active
        Animator playerAnimCtrl = other.gameObject.GetComponentInParent<Animator>();
        if (playerAnimCtrl.GetCurrentAnimatorStateInfo(0).IsName("Base.kick"))
            {
            Debug.Log("player kickt gerade:  Base.kick true");
            PlayerController playerCtrlScript = other.gameObject.GetComponentInParent<PlayerController>();
            //****check if Frame 7 Event was triggered-> if yes : play bearAnim2 (fliegt langsam), else play bearAnim1 
            if (playerCtrlScript.kickFrame7Event)
            {
                bearAnimCtrl.SetTrigger("TriggerBearAnim2");
                Debug.Log("Kick after Frame 7");
            }
            else
            {
                //play bearAnim1
                bearAnimCtrl.SetTrigger("TriggerBearAnim1");
                Debug.Log("Kick BEFORE Frame 7");
                //beim Kick Aufprall 1.Partikel erzeugen
                bearPosition = transform.parent.gameObject.transform.position;
                bearPosition.z +=5;
                GameObject clone = Instantiate(particlePrefab, bearPosition, Quaternion.identity);
                //clone an Vater kleben
                clone.transform.parent = transform.parent.gameObject.transform;
            }
          }
          else
          {
                //if Kick-Anim not active -> play bearAnim2
                bearAnimCtrl.SetTrigger("TriggerBearAnim2"); 
          }
    }
}
