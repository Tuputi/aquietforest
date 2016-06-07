using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {



	float timeToWait = 0f; //how long before the button is active again
	public string buttonName; //text displayed on button
	Timer buttonTimer; //the timerobject for this one
	public GameObject timerObj;
	public Image buttonBar;
	//TextControl textControl;


	//editor slider
	[Range(0.0f, 20.0f)]
	public float WaitTime;

//	[TextArea(3,10)]
//	public string reactionText = "Feed reaction text here.";

	// Use this for initialization
	void Start () {
		timeToWait = WaitTime;
		Text tempText = this.transform.FindChild ("Text").GetComponent<Text> ();
		tempText.text = buttonName;
		buttonTimer = GameObject.Instantiate (GameObject.Find ("TimerObject").GetComponent<Timer>()) as Timer;
	//	Image buttonBar = this.transform.FindChild ("Mask").transform.FindChild ("barImg").GetComponent<Image> ();
		buttonTimer.assignBar (buttonBar);
	//	textControl = GameObject.Find ("GameController").GetComponent<TextControl> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (buttonTimer.isDone ()) {
			this.GetComponent<Button>().interactable = true;
		}
	}

	//what happens when clicked
	public void actionOnClick(){
		this.GetComponent<Button>().interactable = false;
		buttonTimer.startTimer (timeToWait);
	//	textControl.writeLine (reactionText);

	}
}
