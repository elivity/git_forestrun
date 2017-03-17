using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    public LaneScript laneScriptObj;
	Vector3 moveDirection;
	
	Animator playerAnimator;
	CharacterController charContr;
	public float speed;
    public float gravity = 20;
	public GameObject mapPrefab;
    int frameCounter = 0;
    int frameCounterForKick = 0;

	//Die zentrale Variable zum bewegen des Spielers(gibt an wo sich der spieler befinden soll)
	Vector3 playerShouldPos;

    public bool kickFrame7Event = false;  //false : before Frame 7, true : after Frame 7 

	void Start () {
		//speed = GameStateManager.Instance.getSpeed ();
		charContr = GetComponent<CharacterController>();
		playerAnimator = GetComponent<Animator> ();

		playerShouldPos = Vector3.zero;
		playerShouldPos.y = -3.3f;

		laneScriptObj = new LaneScript (mapPrefab);
		playerShouldPos.x = laneScriptObj.switchLaneRight ();

        moveDirection = Vector3.forward * speed;

    }

	short touchDetect=0;
	// Update is called once per frame
	float tmpDistance = 0;
	float tmpDistance2 = 0;
	void Update () {

        //Shadow off in air
        if (!charContr.isGrounded) {
			gameObject.transform.GetChild (2).gameObject.SetActive (false);
		} else {
			gameObject.transform.GetChild (2).gameObject.SetActive (true);
		}

		tmpDistance = playerShouldPos. z;

		if (tmpDistance2+200 <= tmpDistance) {
			Debug.Log ((tmpDistance2 + 200) + " " + tmpDistance);
			//GameStateManager.Instance.setDistance ((int)playerShouldPos.z/10);
			tmpDistance2 = tmpDistance;
		}
		touchDetect = touchCtrl ();

		if (Input.GetKeyDown ("s" )|| touchDetect == 3) {
            moveDirection = Vector3.forward * speed;
            //set frameCounter
            playerAnimator.SetTrigger("TriggerDown");
            frameCounter = 38;


        } else if (Input.GetKeyDown ("a") || touchDetect == 0) {
			playerShouldPos.x = laneScriptObj.switchLaneLeft ();

            //if frameCounter>0 (roll is playing && strafe left) then dont play strafe left animation, else play
            if (frameCounter == 0)
            {
                playerAnimator.SetTrigger("TriggerLeft");
            }
        } else if (Input.GetKeyDown ("d")|| touchDetect == 1) {
			playerShouldPos.x = laneScriptObj.switchLaneRight ();

            //if frameCounter>0 (roll is playing && strafe right)  then dont play strafe right animation, else play
            if (frameCounter == 0)
            {
                playerAnimator.SetTrigger("TriggerRight");
            }
        }
		else if (Input.GetKeyDown ("w")|| touchDetect == 2) {
            if (Random.Range(0, 10) > 5)
            {
                moveDirection = Vector3.forward * speed + Vector3.up * 15;
                //wenn "w" gedrückt wurde wird Jump-Animation ausgeführt und 48 Frames lang keine weitere Animation ausgeführt/akzeptiert
                frameCounter = 48;
                playerAnimator.SetTrigger("TriggerUpAlt"); 
            } else
            {
                moveDirection = Vector3.forward* speed + Vector3.up * 15;
                frameCounter = 48;
                playerAnimator.SetTrigger("TriggerUp"); //playerAnimator.SetTrigger("TriggerUp");
            }
		}
        else if (Input.GetKeyDown(KeyCode.F) || touchDetect == 4)
        {
            playerAnimator.SetTrigger("TriggerKick");
        }

        playerShouldPos.z += 2f;

        //MoveCharacter
        charContr.Move(moveDirection * Time.deltaTime);
        //gravity(down) in each frame, if Character not grounded. Hint: combine with Shadow off
        if (!charContr.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //Switsch lane(x-Axis) only
        gameObject.transform.position = new Vector3(Mathf.Lerp(gameObject.transform.position.x,playerShouldPos.x,Time.fixedDeltaTime*4),gameObject.transform.position.y,gameObject.transform.position.z);

        //decrement frameCounter
        if (frameCounter > 0)
        {
            frameCounter--;
        }

        //after 7 Frames set kickFrame10Event = false
        if (frameCounterForKick > 0)
        {
            frameCounterForKick--;
        }
        else if (frameCounterForKick == 0)
        {
            kickFrame7Event = false;
        }
    }

    //Kick Animation Event (Frame 7) ruft die Funktion auf
    private void kick35Event()
    {
        kickFrame7Event = true;
        frameCounterForKick = 9;   //prevent bearAnim1 for 9 Frames
        Debug.Log("Frame 7 Anim Event");
    }

	private Vector2 startPos;
	private Vector2 movedPos;
	private Vector2 endPos;

	private bool touchNotUsed;
	private short pointCounter = 0;

	private float hypotenuse;

	public short direction = -1; // 0-left / 1-right / 2-up / 3-down / 4-tap

	short touchCtrl() {
		direction = -1;
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			switch (touch.phase)
			{
			//BEGAN
			case TouchPhase.Began:
				startPos = touch.position;
				//**enable touchNotUsed
				touchNotUsed = true;
				break;

				// MOVED
			case TouchPhase.Moved:
				movedPos = touch.position;
				//Hypotenuse a²+b²=c². a= delta-y, b= delta-x
				float a = movedPos.y - startPos.y;
				float b = movedPos.x - startPos.x;
				float cQuadrat = ((a)* (a)) + ((b) * (b));
				hypotenuse = Mathf.Sqrt(cQuadrat);
				if(hypotenuse >= 6)
				{
					float yDelta = Mathf.Abs(movedPos.y - startPos.y);
					float xDelta = Mathf.Abs(movedPos.x - startPos.x);

					//Q1-Q2 RIGHT
					if (movedPos.x > startPos.x && yDelta < xDelta && touchNotUsed==true)
					{
						//**disable
						touchNotUsed = false;
						direction = 1;
					}
					//Q3-Q4 LEFT
					else if (movedPos.x < startPos.x && yDelta < xDelta && touchNotUsed == true)
					{
						touchNotUsed = false;
						direction = 0;
					}

					//Q2-Q4 DOWN
					else if (movedPos.y < startPos.y && yDelta > xDelta && touchNotUsed == true)
					{
						touchNotUsed = false;
						direction = 3;
					}
					//Q1-Q3 UP
					else if (movedPos.y > startPos.y && yDelta > xDelta && touchNotUsed == true)
					{
						touchNotUsed = false;

						pointCounter++;//unnötig

						string countStr = pointCounter.ToString();//unnötig
						direction = 2;
					}
				}
				break;
				//STATIONARY
			case TouchPhase.Stationary:
				break;

				//ENDED
			case TouchPhase.Ended:
				Debug.Log("Ended: ");
				endPos = touch.position;
				//Hypotenuse a²+b²=c². a= delta-y, b= delta-x   
				float aa = endPos.y - startPos.y;
				float bb = endPos.x - startPos.x;
				float ccQuadrat = ((aa) * (aa)) + ((bb) * (bb));
				hypotenuse = Mathf.Sqrt(ccQuadrat);
				//***Falls die Hypotenuse zwischen [startPos & endPos] kleiener als 6px ist -> so war dieser Touch ein 'Tap' und kein 'Swipe'
				if (hypotenuse < 6 && touchNotUsed==true)
				{
					//TAP
					direction = 4;
				}
				break;
			}
		}

		return direction;
	}




}