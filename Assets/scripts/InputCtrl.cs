using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputCtrl : MonoBehaviour {

    private Vector2 startPos;
    private Vector2 movedPos;
    private Vector2 endPos;

    private bool touchNotUsed;
    private short pointCounter = 0;

    private float hypotenuse;

	public byte direction = 0; // 0-right / 1-left / 2-up / 3-down / 4-tap

    void Start () {

    }

    void Update()
    {
		direction = 0;
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
						Debug.Log("xDelta, yDelta: "+xDelta+", "+yDelta);

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
							direction = 2;
                        }

                        //Q2-Q4 DOWN
                        else if (movedPos.y < startPos.y && yDelta > xDelta && touchNotUsed == true)
                        {
                            touchNotUsed = false;
							direction = 4;
                        }
                        //Q1-Q3 UP
                        else if (movedPos.y > startPos.y && yDelta > xDelta && touchNotUsed == true)
                        {
                            touchNotUsed = false;

                            pointCounter++;//unnötig

                            string countStr = pointCounter.ToString();//unnötig
							direction = 3;
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


    }
}
