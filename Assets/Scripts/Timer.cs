using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {


	
	float timeToCount = 5.0f;
	float time;
	float remainingTime;
	bool timerOn = false;
	bool done = false;
	Image myBar;
	bool hasBar = false;

	float barTransformSpeed;
	float barTransformTime;
	Vector3 barCord;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//startedTime = Time.deltaTime;
		if (timerOn) {
			time += Time.deltaTime;

			if(hasBar){
				remainingTime -= Time.deltaTime;
				barTransformSpeed = ((timeToCount-1)/100); //this is the frequency of drops
				barTransformTime += Time.deltaTime; //this is the timer to go count from drop to drop
				if(barTransformTime >= barTransformSpeed){
					updateBar();
					barTransformTime = 0.0f;
				}
							}
			if (time > timeToCount) {
				if(hasBar){myBar.fillAmount = 0f;}
				time = 0.0f;
				timerOn = false;
				done = true;
			}
		}
	}

	public void assignBar(Image whichBar){
		myBar = whichBar;
		if (myBar != null) {
			hasBar = true;
			//barCord = myBar.transform.localPosition;
		}
	}

	public void startTimer(float howLong){
		timeToCount = howLong;
		remainingTime = timeToCount;
		if (hasBar) {
			myBar.gameObject.SetActive(true);
			//myBar.transform.localScale = new Vector3 (20f, 5f, 5f);
			myBar.fillAmount = 1.0f;
			//myBar.transform.localPosition = barCord;
		}
		timerOn = true;
		done = false;
	}

	public bool isDone(){
		return done;
	}

	public void stopTimer(){
		done = false;
		timerOn = false;
	}

	public void updateBar(){
		if (myBar.fillAmount < 0) {
			myBar.gameObject.SetActive(false);
		} 
		else {
			//myBar.transform.localScale -= new Vector3 (.5f, 0f, 0f);
			myBar.fillAmount -= 0.011f;
			//myBar.transform.localPosition -= new Vector3 (2.5f,0f,0f);
		}
	}

}
