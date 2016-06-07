using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class userEvent : MonoBehaviour {

	//Canvas eventCanvas;
	string name;
	string statekey1 = "";
	string state1= "";
	string statekey2 ="";
	string state2= "";
	int triggersFound = 0;
	public GameObject eventBase;
	public GameState gs;
	string evText;
	EventSaveFile esf;
	public Timer timer;
	float cooldown;
	bool needstobeinrange = false;


	void Start(){
		//eventBase = GameObject.Find ("spawnEvent");
		//gs = GameObject.Find ("GameController").GetComponent<GameState> ();
		//
//		evText = GameObject.Find ("eventTextInput").GetComponentInChildren<Text> ().text;

		float rando = Random.Range (0.9f, 1.8f);
		cooldown = 100f * rando;
		timer.startTimer (cooldown);
	}

	void Update(){
	
	}

	public void confirmEventCreation(){
		//eventCanvas = GameObject.Find ("createEventCanvas").GetComponent<Canvas> ();
		bool everythingOkay = true;

		
		if (checkTimeOfDay()) {
			triggersFound++;
		}
		if (checkLightness()) {
			triggersFound++;
		}
		if (checkWeather ()) {
			triggersFound++;
		}
		if (checkFire ()) {
			triggersFound++;
			needstobeinrange = true;
		}

		evText = GameObject.Find ("EventEffectText").GetComponent<Text> ().text;
		if (evText.Equals ("")) {
			GameObject.Find ("CommentText").GetComponent<Text> ().text = "There needs to be an event description.";
			everythingOkay = false;
		}

		if (triggersFound == 0) {
			GameObject.Find ("CommentText").GetComponent<Text> ().text = "Select at least one condition.";
			everythingOkay = false;
		}
		if(triggersFound > 2){
			GameObject.Find ("CommentText").GetComponent<Text> ().text = "Too many conditions selected.";
			everythingOkay = false;
		}
		name = GameObject.Find ("EventNameText").GetComponent<Text> ().text;
		if(name.Equals("")){
			GameObject.Find ("CommentText").GetComponent<Text> ().text = "The Event needs a name.";
			everythingOkay = false;
		}
		if(!(name.Equals("")) && !(isNameUnique(name))){
			GameObject.Find ("CommentText").GetComponent<Text> ().text = "The event name needs to be unique.";
			everythingOkay = false;
		}
		if (everythingOkay) {
			GameObject evBase = Instantiate(eventBase) as GameObject;
			Events ev = evBase.GetComponent<Events> ();
			ev.stateTriggerKey = statekey1;
			ev.state = state1;
			ev.stateTriggerKey2 = statekey2;
			ev.state2 = state2;
			ev.cooldownTime = cooldown;

			ev.effect = evText;
			ev.name = name;
			ev.inrangeneed = needstobeinrange;
			gs.eventList.Add (ev);
			GameObject.Find ("CommentText").GetComponent<Text> ().text = "Event created!";
			timer.startTimer(10f);
			Debug.Log ("event created");
			triggersFound = 0;
			saveEvent ();
			gs.saveEventNames();
		}
	
	}


	private bool isNameUnique(string name){
		List<EventSaveFile> esfList = SaveLoad.getSavedFiles();
		bool isUnique = true;
		foreach (EventSaveFile esf in esfList) {
			if((esf.eventName.ToLower()).Equals(name.ToLower())){
				isUnique = false;
			}
		}
		return isUnique;
	}



	private void saveEvent(){
		esf = new EventSaveFile ();
		esf.effect = evText;
		esf.stateTriggerkey = statekey1;
		esf.state = state1;
		esf.stateTriggerKey2 = statekey2;
		esf.state2 = state2;
		esf.coolDownTime = cooldown;
		esf.eventName = name;
		esf.inrangeneed = needstobeinrange;

		SaveLoad.SaveEvent (esf);
	}

	public void loadEvents(){
		SaveLoad.LoadEvent ();
		if (SaveLoad.savedEvents.Count > 0) {
			foreach (EventSaveFile ev in SaveLoad.savedEvents) {
				GameObject evBase = Instantiate (eventBase) as GameObject;
				Events myEvent = evBase.GetComponent<Events> ();
				myEvent.stateTriggerKey = ev.stateTriggerkey;
				myEvent.state = ev.state;
				myEvent.stateTriggerKey2 = ev.stateTriggerKey2;
				myEvent.state2 = ev.state2;
				myEvent.cooldownTime = ev.coolDownTime;
				myEvent.effect = ev.effect;
				myEvent.inrangeneed = ev.inrangeneed;
				gs.eventList.Add (myEvent);
				Debug.Log("Added an event");
			}
		}else{
			Debug.Log("no events found.");
		}
	}



	private bool checkTimeOfDay(){
		Toggle t = GameObject.Find ("timeOfDayToggle").GetComponent<Toggle> ();
		if (t.isOn) {
			Toggle dawn = GameObject.Find("ToD_morning").GetComponent<Toggle> ();
			if(dawn.isOn){
				statekey1 = "stateTime";
				state1 = "dawn";
				return true;
			}
			else{
				Toggle day = GameObject.Find("ToD_day").GetComponent<Toggle> ();
				if(day.isOn){
					statekey1 = "stateTime";
					state1 = "midday";
					return true;
				}
				else{
					Toggle evening = GameObject.Find("ToD_evening").GetComponent<Toggle> ();
					if(evening.isOn){
						statekey1 = "stateTime";
						state1 = "evening";
						return true;
					}
					else{
						Toggle night = GameObject.Find("ToD_night").GetComponent<Toggle> ();
						if(night.isOn){
							statekey1 = "stateTime";
							state1 = "night";
							return true;
						}
						else{
							Debug.Log ("Time of Day selected - no specifier selected");
							return false;
						}
					}
				}
			}
		}
		return false;
	}

	bool checkLightness(){
		Toggle t = GameObject.Find ("LightlevelToggle").GetComponent<Toggle> ();
		if (t.isOn) {
			Toggle dark = GameObject.Find ("light_dark").GetComponent<Toggle> ();
			if (dark.isOn) {
				if (triggersFound < 1) {
					statekey1 = "statelight";
					state1 = "dark";
					return true;
				} 
				else {
					statekey2 = "statelight";
					state2 = "dark";
					return true;
				}
			}
			else{
				Toggle bright = GameObject.Find ("light_bright").GetComponent<Toggle> ();
				if (bright.isOn) {
					if (triggersFound < 1) {
						statekey1 = "statelight";
						state1 = "bright";
						return true;
					}
					else {
						statekey2 = "statelight";
						state2 = "bright";
						return true;
					}
				}
				else{
					Debug.Log ("Lightness selected - no specifier selected");
					return false;
				}
			}
		}
		return false;
	}

	bool checkWeather(){
		Toggle t = GameObject.Find ("weatherToggle").GetComponent<Toggle> ();
		if (t.isOn) {
			Toggle clear = GameObject.Find ("weather_clear").GetComponent<Toggle> ();
			if (clear.isOn) {
				if (triggersFound < 1) {
					statekey1 = "statehumidity";
					state1 = "dry";
					return true;
				} 
				else {
					statekey2 = "statehumidity";
					state2 = "dry";
					return true;
				}
			}
			else{
				Toggle cloudy = GameObject.Find ("weather_cloudy").GetComponent<Toggle> ();
				if (cloudy.isOn) {
					if (triggersFound < 1) {
						statekey1 = "statehumidity";
						state1 = "damp";
						return true;
					}
					else {
						statekey2 = "statehumidity";
						state2 = "damp";
						return true;
					}
				}
				else{
					Toggle rain = GameObject.Find ("weather_rain").GetComponent<Toggle> ();
					if (rain.isOn) {
						if (triggersFound < 1) {
							statekey1 = "statehumidity";
							state1 = "rain";
							return true;
						} 
						else {
							statekey2 = "statehumidity";
							state2 = "rain";
							return true;
						}
					}
					else{
						Debug.Log ("weather selected - no specifier selected");
						return false;
					}
				}
			}
		}
		return false;
	}

	bool checkFire(){
		Toggle t = GameObject.Find ("fireToggle").GetComponent<Toggle> ();
		if (t.isOn) {
			Toggle fout = GameObject.Find ("fire_out").GetComponent<Toggle> ();
			if (fout.isOn) {
				if (triggersFound < 1) {
					statekey1 = "statefire";
					state1 = "out";
					return true;
				} 
				else {
					statekey2 = "statefire";
					state2 = "out";
					return true;
				}
			}
			else{
				Toggle burning = GameObject.Find ("fire_burning").GetComponent<Toggle> ();
				if (burning.isOn) {
					if (triggersFound < 1) {
						statekey1 = "statefire";
						state1 = "burning";
						return true;
					}
					else {
						statekey2 = "statefire";
						state2 = "burning";
						return true;
					}
				}
				else{
					Toggle dying = GameObject.Find ("fire_dying").GetComponent<Toggle> ();
					if (dying.isOn) {
						if (triggersFound < 1) {
							statekey1 = "statefire";
							state1 = "dying";
							return true;
						} 
						else {
							statekey2 = "statefire";
							state2 = "dying";
							return true;
						}
					}
					else{
						Debug.Log ("fire selected - no specifier selected");
						return false;
					}
				}
			}
		}
		return false;
	}
	

}
