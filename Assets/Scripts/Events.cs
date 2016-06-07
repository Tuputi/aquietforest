using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
//[System.Serializable]
public class Events : MonoBehaviour {

	GameState gameState;
	Timer cooldownTimer;
	public float cooldownTime;
//	Dictionary<string,float> rangeVariables;
//	Dictionary<string,string> stateVariables;

	[TextArea(1,10)]
	public string name = "NameOfEvent";

	[TextArea(3,10)]
	public string effect = "Feed effect text here.";

	[TextArea(1,1)]
	public string rangeTriggerKey = "humidity";

	[Range(-1f,100f)]
	public float rangeTriggerMax = -1f;

	[Range(-1f,100f)]
	public float rangeTriggerMin = -1f;

	//

	[TextArea(1,1)]
	public string rangeTriggerKey2 = "brightness";
	
	[Range(-1f,100f)]
	public float rangeTriggerMax2 = -1f;
	
	[Range(-1f,100f)]
	public float rangeTriggerMin2 = -1f;

	//

	[TextArea(1,1)]
	public string stateTriggerKey;

	[TextArea(1,1)]
	public string state;

	//

	[TextArea(1,1)]
	public string stateTriggerKey2 = "";
	
	[TextArea(1,1)]
	public string state2 = "";

	//

	[TextArea(1,1)]
	public string rangeEffectKey;
	
	[Range(-50f,50f)]
	public float rangeEffect = 0f;
	
	//
	
	[TextArea(1,1)]
	public string rangeEffectKey2;
	
	[Range(-50f,50f)]
	public float rangeEffect2 = 0f;

	public bool inrangeneed;

	//public string effect2 = "Feed effect text here.";






	// Use this for initialization
	void Start () {
		gameState = GameObject.Find ("GameController").GetComponent<GameState> ();
		cooldownTimer = GameObject.Instantiate (GameObject.Find ("TimerObject").GetComponent<Timer>()) as Timer;
		cooldownTimer.startTimer (1f);
	//	rangeVariables = gameState.getRangeVariables();
		//stateVariables = gameState.getStateVariables();
		//rangeVariables = gameState.getRangeVariables();

//		foreach(KeyValuePair<string,float> pair in rangeVariables){
//
//		}
	}
	

	// Update is called once per frame
	void Update () {
	
	}

	public bool checkTrigger(Dictionary<string,float> rangeDict, Dictionary<string,string> stateDict, Dictionary<string,bool> boolDict){

		if(!(cooldownTimer.isDone())){
			return false;
		}

		string code;
		List<string> myKeys = new List<string>();
		if (!(rangeTriggerKey.Equals (""))) {
			myKeys.Add (rangeTriggerKey);
		}
		if (!(rangeTriggerKey2.Equals (""))) {
			myKeys.Add (rangeTriggerKey2);
		}


		for (int i = 0; i <= myKeys.Count-1; i++) {
			code = myKeys[i].ToLower();
			if(rangeDict.ContainsKey(code)){
				float value = rangeDict[code];
				if(value<rangeTriggerMax&&value>rangeTriggerMin){

					//trigger[i] okay
				}
				else return false;//trigger[i] not okay >:(
			}
			else {
				Debug.Log("Couldn't find the key '"+code+"'. Check the spelling and if it has been added to rangedVariables");
				return false;}
		}


		List<string> myStateKeys = new List<string> ();
		if (!(stateTriggerKey.Equals (""))) {
			myStateKeys.Add (stateTriggerKey);
		}
		if (!(stateTriggerKey2.Equals (""))) {
			myStateKeys.Add (stateTriggerKey2);
		}

		//no values whatsoever?
		if (myStateKeys.Count == 0 && myKeys.Count == 0) {
			return false;
		}


		string testState;
		List<string> myStates = new List<string> ();
		if (!(state.Equals (""))) {
			myStates.Add (state);
		}
		if (!(state2.Equals (""))) {
			myStates.Add (state2);
		}


		for (int i = 0; i <= myStateKeys.Count-1; i++) {
			code = myStateKeys[i].ToLower();
			testState = myStates[i].ToLower();
			if(stateDict.ContainsKey(code)){
				if(stateDict[code].Equals(testState)){
					if(testState.Equals("statefire")){
						if(boolDict["inRange"]){
							Debug.Log("close enough to the fire");
						}
						else{
							Debug.Log("not close enough to the fire");
						}
					}
				}
				else {
					Debug.Log(stateDict[code]+", "+testState);
					return false;
				}
			}
			else {
				Debug.Log("Couldn't find the key '"+code+"'. Check the spelling and if it has been added to stateVariables");
				return false;
			}
		}

		return true;
	}

	private bool checkEffectValidity(Dictionary<string,float> rangeDict){
		string code = "notvalid";
		string code2 = "notvalid";


		//problem: doesn't read the keys correctly
		Dictionary<string,float> myKeys = new Dictionary<string,float>();
		if (!(rangeEffectKey.Equals (""))) {
			myKeys.Add (rangeEffectKey, rangeEffect);
			code = rangeEffectKey;
		}
		if (!(rangeEffectKey2.Equals (""))) {
			myKeys.Add (rangeEffectKey2, rangeEffect2);
			code2 = rangeEffectKey2;
		}

		if (!(myKeys.Count <= 0)) {
			if (rangeDict.ContainsKey (code)) {
				float value = rangeDict [rangeEffectKey];
				value = value + myKeys [rangeEffectKey];
				rangeDict[rangeEffectKey] = value;
			} else {
				Debug.Log ("Couldn't find the rangeEffectKey '"+code+"'. Check the spelling and if it has been added to rangedVariables");
				if (rangeDict.ContainsKey (code2)) {
					float value = rangeDict [rangeEffectKey2];
					value = value + myKeys [rangeEffectKey2];
					rangeDict[rangeEffectKey] = value;
				} else {
					Debug.Log ("Couldn't find the rangeEffectKey2 '"+code2+"'. Check the spelling and if it has been added to rangedVariables");
				}
			}
		}
	
		return true;
	}

	public string getEffect(Dictionary<string,float> rangeVariable){
		cooldownTimer.startTimer (cooldownTime);
		checkEffectValidity (rangeVariable);
		return effect;
	}



}
