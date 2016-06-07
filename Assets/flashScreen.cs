using UnityEngine;
using System.Collections;

public class flashScreen : MonoBehaviour {

	// Use this for initialization
	public Timer timer;

	void Start () {
		timer.startTimer (10f);
	}

	// Update is called once per frame
	void Update () {
		if (timer.isDone ()) {
			UnityEngine.Application.LoadLevel(1);		
		}
	}

	public void nextScreen(){
		UnityEngine.Application.LoadLevel(1);
	}
}
