using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameState : MonoBehaviour {



	//metare

	float brightness = 0f;
		string stateLight = "normal";
	float humidity = 0f;
		string stateHumidity = "dry";
	float timeOfDay = 0f; //0-24
		string stateTime = "evening";	
	float hunger = 100f;
	float warmth = 0f;
	float fireEnergy = 0f;
		string stateFire = "notlit";
	float stepsTaken = 0f;


	//timers
	Timer timeOfDayTimer;
	Timer weatherTimer;
	Timer brightnessTimer;
	Timer fireTimer;
	Timer randomEventTimer;
	//other stuff

	public TextControl textControl;
	public movementScript movScript;

	Events events;
	public List<Events> eventList;


	public Dictionary<string,float> rangeVariables;
	public Dictionary<string,string> stateVariables;
	public Dictionary<string, bool> boolVariables;
	public InputField inputField;
	string savefilename = "";
	public Canvas menuCanvas;
	public Canvas eventCreationCanvas;
	public Canvas wisdomCanvas;
	userEvent ue;
	public Text wisdomUText;
	public Text wisdomCText;
	public GameObject editorCanvas;

	//infostuff
	public Text firewoodInfo;

	//dynamic changes
	string celestialobject;
	string dyna_timeofday;




	// Use this for initialization
	void Start () {
		Debug.Log (Application.persistentDataPath);
		textControl = GameObject.Find ("GameController").GetComponent<TextControl> ();
		movScript = GameObject.Find ("GameController").GetComponent<movementScript> ();
	
		ue = GameObject.Find ("GameController").GetComponent<userEvent> ();
		ue.loadEvents ();

		timeOfDayTimer = GameObject.Instantiate (GameObject.Find ("TimerObject").GetComponent<Timer>()) as Timer;
		//timeOfDayTimer = GameObject.Find ("TimerObject").GetComponent<Timer>();
		timeOfDay = 0;
		timeOfDayTimer.startTimer (10f);

		weatherTimer = GameObject.Instantiate (GameObject.Find ("TimerObject").GetComponent<Timer>()) as Timer;
		//weatherTimer = GameObject.Find ("TimerObject").GetComponent<Timer>();
		weatherTimer.startTimer(10f);

		brightnessTimer = GameObject.Instantiate (GameObject.Find ("TimerObject").GetComponent<Timer>()) as Timer;
		//brightnessTimer = GameObject.Find ("TimerObject").GetComponent<Timer>();
		brightnessTimer.startTimer(1f);

		fireTimer = GameObject.Instantiate (GameObject.Find ("TimerObject").GetComponent<Timer>()) as Timer;
		//fireTimer = GameObject.Find ("TimerObject").GetComponent<Timer>();
		fireTimer.startTimer (1f);

		randomEventTimer = GameObject.Instantiate (GameObject.Find ("TimerObject").GetComponent<Timer>()) as Timer;
		//randomEventTimer = GameObject.Find ("timerObj").GetComponent<Timer>();
		randomEventTimer.startTimer (1f);
		//eventList = new List<Events>();
		hungerTimer = GameObject.Instantiate(GameObject.Find("TimerObject").GetComponent<Timer>()) as Timer;


		rangeVariables = new Dictionary<string, float> ();
		stateVariables = new Dictionary<string, string> ();
		boolVariables = new Dictionary<string, bool>();

		//range variables
		rangeVariables.Add ("brightness", brightness);
		rangeVariables.Add ("humidity", humidity);
		rangeVariables.Add ("timeofday", timeOfDay);
		rangeVariables.Add ("fireenergy", fireEnergy);
		rangeVariables.Add ("hunger", hunger);
		rangeVariables.Add ("warmth", warmth);
		rangeVariables.Add ("logs", logs);
		rangeVariables.Add ("food", food);
		rangeVariables.Add ("steps", stepsTaken);
		rangeVariables.Add ("whichLineLogs", 0f);

		//stateVariables

		stateVariables.Add ("statehumidity", stateHumidity);
		stateVariables.Add ("statefire", stateFire);
		stateVariables.Add ("statetime", stateTime);
		stateVariables.Add ("statelight", stateLight);
		stateVariables.Add ("inrangefire", "true");

		//bool variables

		boolVariables.Add ("inRangeFireB", true); //are we in range for fire effects?
		boolVariables.Add ("onceADay", true); //happens only once a day humidity
		boolVariables.Add ("onlyOnceFire", false);
		boolVariables.Add ("flickerOnce", false);
		boolVariables.Add ("firstTimeLogs", false);
		boolVariables.Add ("hasArrivedLogs", false);
		boolVariables.Add ("shouldArrive", false);
		boolVariables.Add ("firstTimeFood", true);




		SaveFile.current = new SaveFile ();
		savefilename = "Reset";
		save ();
		savefilename = "";
		SaveLoad.LoadAforismer ();
		aforismer = SaveLoad.aforismerSave;

	
	}
	
	// Update is called once per frame
	void Update () {

		if (weatherTimer.isDone ()) {
			rangeVariables["humidity"] += Random.Range (-4, 8);
			if(rangeVariables["humidity"]<0f){
				rangeVariables["humidity"] = 0f;
			}
			Debug.Log ("humidity: "+rangeVariables["humidity"]);
			weatherTimer.startTimer(10f);
		}

		if (timeOfDayTimer.isDone()) {
			Debug.Log ("brightness: "+rangeVariables["brightness"]);
			timeOfDay += 1f;
			if(timeOfDay == 24f){
				timeOfDay = 0f;
			}
			timeOfDayTimer.startTimer (5f);
		}

		if(stateVariables["statefire"].Equals("burning")){
			if(fireTimer.isDone()){
				rangeVariables["fireenergy"] -= 5;
				if(rangeVariables["fireenergy"]<0){
					rangeVariables["fireenergy"] = 0;
				}
				Debug.Log("Fire:"+rangeVariables["fireenergy"]);
				fireTimer.startTimer(5f);
			}
		}

		if(Input.GetKeyDown(KeyCode.E)){
			openEditorOption();
		}

		eventHandler ();
		
	}




	void eventHandler(){
		if(movScript.getCordintes ()[0] < -3 || movScript.getCordintes()[0] > 3 
		   || movScript.getCordintes()[1] < -3 || movScript.getCordintes()[1] > 3){
			stateVariables["inrangefire"] = "false";
			boolVariables["inRangeFireB"] = false;
		}
		else{
			stateVariables["inrangefire"] = "true";
			boolVariables["inRangeFireB"] = true;
		}
		if(movScript.getCordintes ()[0]==0 && movScript.getCordintes()[1]==0){
			logButton.gameObject.SetActive(false);
		}
		else{
			logButton.gameObject.SetActive(true);
		}

		randomEvent ();
		weatherEvents ();
		lightEvents ();
		fireEventsAi (boolVariables["inRangeFireB"]);
		logAi ();
		hungerAi ();
		stepEvents ();
		timeOfDayEvents ();
		updateInfo ();
	}






	public Dictionary<string,float> getRangeVariables(){
		return rangeVariables;
	}
	public Dictionary<string,string> getStateVariables(){
		return stateVariables;
	}


	public void openEditorOption(){
		bool status = !(editorCanvas.gameObject.activeSelf);
		editorCanvas.gameObject.SetActive (status);
		Debug.Log ("Editor opened");
	}
//	void cooldownHandler(){
//		
//	}


	void randomEvent(){
		if (randomEventTimer.isDone()) {
			int i = Random.Range (0, eventList.Count);

			if (eventList [i].checkTrigger (rangeVariables, stateVariables, boolVariables)) {
				textControl.writeLine (eventList [i].getEffect (rangeVariables));
			}
			randomEventTimer.startTimer(5f);
		}	
	}

	void updateInfo(){
		firewoodInfo.text = "Firewood: " + rangeVariables ["logs"];
		foodInfo.text = "Food: " + rangeVariables ["food"];
		hungerInfo.text = "Hunger: " + rangeVariables ["hunger"];
	}


	void timeOfDayEvents(){
		if (stateVariables["statetime"].Equals ("night")) {
			celestialobject = "moon";
			dyna_timeofday = "dawn";
			if(timeOfDay>5f&&timeOfDay<11f){
				rangeVariables["brightness"] += 10f;
				int i = Random.Range(1,3);
				switch(i){
				case 1: textControl.writeLine("The sun is rising.");
						;
						break;
				case 2: textControl.writeLine("It starts to dawn."); 
						
						break;
				case 3: textControl.writeLine("The stars begin to fade."); 
						break;
				default:textControl.writeLine("The skies are becoming brighter...");
						break;
				}

				stateVariables["statetime"] = "dawn";
				Debug.Log(stateVariables["statetime"]);
			}
		}
		else if(stateVariables["statetime"].Equals ("dawn")) {
			if(timeOfDay>12f){
				celestialobject = "sun";
				dyna_timeofday = "day";
				rangeVariables["brightness"] += 10f;
				textControl.writeLine("It is daytime.");
				stateVariables["statetime"] = "midday";
			}
		}
		else if (stateVariables["statetime"].Equals ("midday")) {
			if(timeOfDay>19f){
				rangeVariables["brightness"] += -10f;
				textControl.writeLine("It is evening.");
				stateVariables["statetime"] = "evening";
				dyna_timeofday = "evening";
			}
		}
		else if (stateVariables["statetime"].Equals ("evening")) {
			if(timeOfDay>22f||timeOfDay<4f){
				rangeVariables["brightness"] -=10f;
				textControl.writeLine("It is night.");
				rangeVariables["humidity"] += 2f;
				boolVariables["onceADay"] = true;
				stateVariables["statetime"] = "night";
				dyna_timeofday = "night";
			}
		}
	}

	//dry, damp, rain
	void weatherEvents(){

		if (stateVariables["statehumidity"].Equals ("dry") && stateVariables["statetime"].Equals("midday")&&boolVariables["onceADay"]) {
			textControl.writeLine("It's a warm day.");
			boolVariables["onceADay"] = false;
		}

		if (stateVariables["statehumidity"].Equals("damp")&&rangeVariables["humidity"] < 25f) {
			textControl.writeLine("The sky is clear.");
			stateVariables["statehumidity"] = "dry";	
		} else if ((stateVariables["statehumidity"].Equals("dry")||stateVariables["statehumidity"].Equals("rain"))
		           &&rangeVariables["humidity"] > 50 && rangeVariables["humidity"] < 65f){
			textControl.writeLine("Clouds have started to gather.");
			stateVariables["statehumidity"] = "damp";
		} else if (stateHumidity.Equals("damp")&&rangeVariables["humidity"] > 75) {
			textControl.writeLine("It is raining.");
			stateVariables["statehumidity"] = "rain";
		}
		if(stateHumidity.Equals("rain")){
			rangeVariables["humidity"] -= 1f;
		}
	}



	void lightEvents(){
		if ((stateVariables["statelight"].Equals("normal")||stateVariables["statelight"].Equals("bright"))
		    &&rangeVariables["brightness"] < 25) {
				textControl.writeLine ("The colours blend together in the darkness.");
				stateVariables["statelight"] = "dark";
				brightnessTimer.startTimer (30f);
		}
		else if ((stateVariables["statelight"].Equals("normal")||stateVariables["statelight"].Equals("dark"))
		         &&rangeVariables["brightness"] > 65) {
				stateVariables["statelight"] = "bright";
				if(fireEnergy>80){
					textControl.writeLine ("The fire chases the shadows away");
				}
				else{
					textControl.writeLine ("The "+celestialobject+" is shining brightly.");
				}
		}
	}
	


	//BUTTONACTIONS BELOW

	public void moveNorth(){
		rangeVariables ["steps"]++;
		movScript.move ("north");
		//textControl.writeLine (movScript.getCurrentLocation());
		movScript.getCurrentLocation ();
	}

	public void moveEast(){
		rangeVariables ["steps"]++;
		movScript.move ("east");
		//textControl.writeLine (movScript.getCurrentLocation());
		movScript.getCurrentLocation ();
	}

	public void moveSouth(){
		rangeVariables ["steps"]++;
		movScript.move ("south");
		//textControl.writeLine (movScript.getCurrentLocation());
		movScript.getCurrentLocation ();
	}

	public void moveWest(){
		rangeVariables ["steps"]++;
		movScript.move ("west");
		//textControl.writeLine (movScript.getCurrentLocation());
		movScript.getCurrentLocation ();
	}


	public void save(){
	//	string tempString = textControl.getAllText ();
	//	SaveFile savefile = new SaveFile (textControl.getAllText());
		SaveFile.current.completeStory = textControl.getAllText ();
		SaveFile.current.rangedVariablesSave = rangeVariables;
		SaveFile.current.stateVariablesSave = stateVariables;
		SaveFile.current.boolVariablesSave = boolVariables;
		if (!(savefilename.Equals ("")||savefilename.Equals("Reset"))) {
			SaveLoad.Save (savefilename);
			Debug.Log ("game saved");
			GameObject.Find ("SaveCommentText").GetComponent<Text> ().text = "Game Saved";
		}
		if(savefilename.Equals("Reset")){
			SaveLoad.Save (savefilename);
			Debug.Log ("game saved");
		}
		if(savefilename.Equals("")) {
			GameObject.Find ("SaveCommentText").GetComponent<Text> ().text = "Please specify a filename";
		}
	
		//System.IO.File.WriteAllText("C:\\users\\Tove\\Documents\\TheQuietWoods\\SavedGames\\save.txt", tempString);
	}

	public void reset(){
		savefilename = "Reset";
		load ();
		savefilename = "";

		//rest fire
//		onlyOnceFire = false;
//		flickerOnce = false;
//		firstTimeLogs = false;
//		hasArrivedLogs = false;
//		shouldArrive = false;
//		whichLineLogs = 0;
//		firstTimeFood = true;
		fireButton.transform.FindChild ("Text").GetComponent<Text>().text = "Light a fire";
	}
	//firetbuttontext doesn't reset - add to things that are saved?
	public void load(){

		if (!(savefilename.Equals (""))) {
		
			SaveLoad.Load (savefilename);
			SaveFile.current = SaveLoad.saveFiles [0];
			Debug.Log ("game loaded");
			if(!(savefilename.Equals("Reset"))){
				GameObject.Find ("SaveCommentText").GetComponent<Text> ().text = "Game Loaded";
			}

		} else {
			GameObject.Find ("SaveCommentText").GetComponent<Text> ().text = "Please specify a filename";
		}

		//Dumb mistake - now loads an list for each savefile, 
		//should instead go through the list and pick the corresponding savefile 
		//according to name, no need for several lists

		textControl.replaceAllText (SaveFile.current.completeStory);
		rangeVariables = SaveFile.current.rangedVariablesSave;
		stateVariables = SaveFile.current.stateVariablesSave;
		boolVariables = SaveFile.current.boolVariablesSave;

		//quickfixes
		if(!(boolVariables["firstTimeFood"])){
			foodInfo.gameObject.SetActive(true);
			hungerInfo.gameObject.SetActive(true);
			foodButton.gameObject.SetActive(true);
			eatButton.gameObject.SetActive(true);
			boolVariables["firstTimeFood"] = false;
			hungerTimer.startTimer(10f);
		}
		if (boolVariables["firstTimeLogs"]) {
			firewoodInfo.gameObject.SetActive(true);
			boolVariables["firstTimeLogs"] = true;
			logtimer =  GameObject.Instantiate(GameObject.Find("TimerObject").GetComponent<Timer>()) as Timer;
			logtimerAi = GameObject.Instantiate(GameObject.Find("TimerObject").GetComponent<Timer>()) as Timer;
			logtimerAi.startTimer(1f);
			logtimer.startTimer(1f);
		}

	}

	public void changeSaveFileName(){
		string filename = inputField.textComponent.text;
		savefilename = filename;
	}

	public void revealSaveMenu(){
		if(eventCreationCanvas.gameObject.activeSelf){
			GameObject.Find ("CommentText").GetComponent<Text> ().text = "";
			eventCreationCanvas.gameObject.SetActive(false);
		}
		if(wisdomCanvas.gameObject.activeSelf){
			GameObject.Find ("WisdomText").GetComponent<Text> ().text = "";
			wisdomCanvas.gameObject.SetActive(false);
		}
		if(menuCanvas.gameObject.activeSelf){
			GameObject.Find ("SaveCommentText").GetComponent<Text> ().text = "";
		}
		bool status = !(menuCanvas.gameObject.activeSelf);
		menuCanvas.gameObject.SetActive(status);
	}

	public void revealEventCreateMenu(){
		if(menuCanvas.gameObject.activeSelf){
			GameObject.Find ("SaveCommentText").GetComponent<Text> ().text = "";
			menuCanvas.gameObject.SetActive(false);
		}
		if(wisdomCanvas.gameObject.activeSelf){
			GameObject.Find ("WisdomText").GetComponent<Text> ().text = "";
			wisdomCanvas.gameObject.SetActive(false);
		}
		if(eventCreationCanvas.gameObject.activeSelf){
			GameObject.Find ("CommentText").GetComponent<Text> ().text = "";
		}

		bool status = !(eventCreationCanvas.gameObject.activeSelf);
		eventCreationCanvas.gameObject.SetActive(status);
	}

	public void revealWisdomMenu(){
		if(menuCanvas.gameObject.activeSelf){
			GameObject.Find ("SaveCommentText").GetComponent<Text> ().text = "";
			menuCanvas.gameObject.SetActive(false);
		}
		if(eventCreationCanvas.gameObject.activeSelf){
			GameObject.Find ("CommentText").GetComponent<Text> ().text = "";
			eventCreationCanvas.gameObject.SetActive(false);
		}
		if (wisdomCanvas.gameObject.activeSelf){
			GameObject.Find("WisdomText").GetComponent<Text>().text = "";
		}

		bool status = !(wisdomCanvas.gameObject.activeSelf);
		wisdomCanvas.gameObject.SetActive (status);
	}

	public void saveWisdom(){
		string wisdom = wisdomUText.text;
		if (!wisdom.Equals ("")) {
			//aforismer.Add (wisdom);
			SaveLoad.SaveWisdom(wisdom);
			wisdomCText.text = "Wisdom saved.";
			SaveLoad.LoadAforismer();
			aforismer = SaveLoad.aforismerSave;
			wisdomUText.text = "";

		} else {
			wisdomCText.text = "The wisdom is a bit too short."; 
		}
	}



	public void writeStoryToFile(){
		if (!(savefilename.Equals (""))) {
			textControl.writeToFile (savefilename);
			Debug.Log("Written to file");
			GameObject.Find ("SaveCommentText").GetComponent<Text> ().text = "Story saved as '"+savefilename+"'";
		} else {
			GameObject.Find ("SaveCommentText").GetComponent<Text> ().text = "Please specify a filename";
		}
	}

	public void writeAforismerToFile(){
		//SaveLoad.LoadAforismer ();
		textControl.writeToFileEvent (SaveLoad.getAforismerText(),"aforismer");
		Debug.Log("Written to file");
	}


	public void saveEventNames(){
		textControl.writeToFileEvent (SaveLoad.getEventNames ());
	}


	public void removeEvent(){
		string temp = GameObject.Find ("whatToRemoveText").GetComponent<Text> ().text;
		SaveLoad.removeEvent (temp);
		saveEventNames ();
	}
	public void removeWisdom(){
		string temp = GameObject.Find ("whatToRemoveText").GetComponent<Text> ().text;
		SaveLoad.removeWisdom (temp);
		writeAforismerToFile ();
	}

	public bool atTheseCordinates(int x, int y){
		return (movScript.getCordintes () [0] == x && movScript.getCordintes () [1] == y);
	}


	//step events

	public void stepEvents(){
		if (rangeVariables ["steps"] == 10) {
			rangeVariables["steps"]++;
			textControl.writeLine("The forest has started to feel more familiar.");
		}
	}

	//log collection events
	float logs = 0f;
	public Button logButton;
	public Button eatButton;
//	bool firstTimeLogs = false;
//	bool hasArrivedLogs = false;
//	bool shouldArrive = false;
//	int whichLineLogs = 0;
	Timer logtimer;
	Timer logtimerAi;

	public void collectLogs(){
		if (!boolVariables["firstTimeLogs"]) {
			firewoodInfo.gameObject.SetActive(true);
			boolVariables["firstTimeLogs"] = true;
			logtimer =  GameObject.Instantiate(GameObject.Find("TimerObject").GetComponent<Timer>()) as Timer;
			logtimerAi = GameObject.Instantiate(GameObject.Find("TimerObject").GetComponent<Timer>()) as Timer;
		}
		rangeVariables ["logs"]++;
		string text = "";
		int i = Random.Range (1,4);
		switch(i){
		case 1: text = "You pick some sticks from the ground."; break;
		case 2: text = "You find dry wood."; break;
		case 3: text = "You gather some wood from the ground."; break;
		}
		textControl.writeLine (text);
		Debug.Log (rangeVariables["logs"]);
	}


	//logs ai aka carzy log hobo
	void logAi(){
		if (rangeVariables ["logs"] > 5) {
			boolVariables["shouldArrive"] = true;
		}

		if (boolVariables["shouldArrive"] == true && atTheseCordinates(0,0)&&boolVariables["hasArrivedLogs"] == false && !stateVariables["statefire"].Equals("unlit")) {
			textControl.writeLine("A wanderer has seen the light of your fire.\n They've brought food with them.");
			logtimer.startTimer(5f);
			boolVariables["hasArrivedLogs"] = true;
			if(boolVariables["firstTimeFood"]){
				foodInfo.gameObject.SetActive(true);
				hungerInfo.gameObject.SetActive(true);
				foodButton.gameObject.SetActive(true);
				eatButton.gameObject.SetActive(true);
				boolVariables["firstTimeFood"] = false;
				hungerTimer.startTimer(10f); //wont be saved!
			}
		}

		if (boolVariables["hasArrivedLogs"] && logtimer.isDone()&& atTheseCordinates(0,0)) {
			int i = (int)rangeVariables["whichLineLogs"];
			Debug.Log("int i:"+i);

			switch(i){
			case 0: 
				textControl.writeLine("\"I can help you by gathering firewood\",\nthe stranger says."); 
				logtimer.startTimer(10f);
				logtimerAi.startTimer(5f);
				break; 
			case 1: 
				if(aforismer.Count>0){
				textControl.writeLine("\"I know a thing or two of the world\",\n the stranger says."); 
				logtimer.startTimer(10f);
				break;
				}
				else{
					textControl.writeLine("The stranger mumbles to himself\n and helps himself to some food."); 
					rangeVariables["food"]--;
					logtimer.startTimer(10f);
					break;
				}
			case 2: 
				if(rangeVariables["food"]>0 && aforismer.Count>0){
					textControl.writeLine("\""+getAforism()+"\","+"\nthe stranger says.");
					textControl.writeLine("The stranger takes something to eat.");
					rangeVariables["food"]--;
					logtimer.startTimer(40f);
					break;
				}
				else if(rangeVariables["food"]<0){
					textControl.writeLine("\"We seem to have run out of food\",\n the stranger says.");
					logtimer.startTimer(20f);
					break;
				}
				else{
					logtimer.startTimer(20f);
					break;
				}
			default: break;
			}
			if(i<2){
				rangeVariables["whichLineLogs"]+= 1f;
			}

		}
		if (boolVariables["hasArrivedLogs"] && logtimerAi.isDone() && rangeVariables["logs"]<1000) {
			rangeVariables["logs"]++;
			logtimerAi.startTimer(10f);
		}
	}

	List<string> aforismer;
	string getAforism(){
		int i = Random.Range (0, aforismer.Count);
		return aforismer [i];
	}


	float food = 10f;
	public Button foodButton;

	public Text foodInfo;
	public Text hungerInfo;
	//food events
	public void gatherFood(){
		if (atTheseCordinates (-1,0)) {
				rangeVariables ["food"] += 2;
				textControl.writeLine ("You gather some apples.");
		}
		else if(atTheseCordinates(-4,1)||atTheseCordinates(-3,1)||atTheseCordinates(-2,0)||atTheseCordinates(-2,2)
		   ||atTheseCordinates(-2,1)||atTheseCordinates(-1,-1)
		   ||atTheseCordinates(0,-2)||atTheseCordinates(1,-2)||atTheseCordinates(2,-2)||atTheseCordinates(3,-2)
		   ||atTheseCordinates(4,-2)||atTheseCordinates(5,-1)){
			rangeVariables["food"]+=3;
			textControl.writeLine("The river provides you with fish and berries.");
		}
		else {
			rangeVariables["food"]++;
			textControl.writeLine ("You find some mushrooms on the forest floor.");
		}
	}


	//fireevents
		
	public Button fireButton;
	//fireenergy at top
	//bool onlyOnceFire = false;
	//bool flickerOnce = false;

	
	public void fireEvents(){

		
		if(stateVariables["statefire"].Equals("notlit")||stateVariables["statefire"].Equals("out")){
			rangeVariables["brightness"] += 50;
			rangeVariables["fireenergy"] = 100;
			textControl.writeLine ("You light a fire.");
			fireButton.transform.FindChild ("Text").GetComponent<Text>().text = "Tend for the fire";
			boolVariables["flickerOnce"] = true;
			boolVariables["onlyOnceFire"] = true;
			stateVariables["statefire"] = "burning";
		}
		else if(stateVariables["statefire"].Equals("burning")||stateVariables["statefire"].Equals("dying")){
			if(rangeVariables["logs"] > 0){
				rangeVariables["fireenergy"] +=10;
				fireButton.transform.FindChild ("Text").GetComponent<Text>().text = "Tend for the fire";
				rangeVariables["logs"]--;
					if(rangeVariables["logs"] <0f){
						rangeVariables["logs"] = 0f;
					}
				textControl.writeLine ("The fire dances lively.");
			}
			else{
				textControl.writeLine ("You've run out of logs.");
			}
		}
	}

	void fireEventsAi(bool inRange){
		bool atCenter = (movScript.getCordintes()[0] == 0 && movScript.getCordintes()[1] == 0);
		fireButton.gameObject.SetActive (atCenter);


		if (stateVariables ["statefire"].Equals ("burning")) {
			fireButton.transform.FindChild ("Text").GetComponent<Text>().text = "Tend for the fire";
		}

		if (rangeVariables["fireenergy"]<90f&&boolVariables["flickerOnce"]) {
			if(inRange){
			textControl.writeLine("The fire chases the shadows away.");
			}
			fireButton.transform.FindChild ("Text").GetComponent<Text>().text = "Poke the fire";
			boolVariables["flickerOnce"] = false;
		}

		if (rangeVariables["fireenergy"] <= 0 && boolVariables["onlyOnceFire"]) {
			boolVariables["onlyOnceFire"] = false;
			rangeVariables["brightness"] -= 10f;
			stateVariables["statefire"] = "out";
			fireButton.transform.FindChild ("Text").GetComponent<Text>().text = "Light a fire";
			if(inRange){
			textControl.writeLine ("Only the last embers are glowing.");
			}
		}
		if (!(stateVariables["statefire"].Equals("dying"))&&(rangeVariables["fireenergy"] < 25&&rangeVariables["fireenergy"]>5)) {
			fireButton.transform.FindChild ("Text").GetComponent<Text>().text = "Poke the fire";
			stateVariables["statefire"] = "dying";
			rangeVariables["brightness"] -= 40;
			if(inRange){
			textControl.writeLine ("The fire is flickering slowly.");
			}
		}
	}

	//look at sky
	public void lookAtSky(){
		if(stateVariables["statehumidity"].Equals("dry")){
			textControl.writeLine("The "+celestialobject+" shines on a clear sky.");
		}
		if(stateVariables["statehumidity"].Equals("damp")){
			textControl.writeLine("It is a cloudy "+dyna_timeofday+".");
		}
		if(stateVariables["statehumidity"].Equals("rain")){
			textControl.writeLine("It's raining.");
		}
	}

	//eatfood
	
	public void eatFood(){
		if (rangeVariables ["food"] > 0) {
			if(rangeVariables["hunger"] == 100){
				textControl.writeLine("You don't feel the need to eat yet");
			}
			else{
				textControl.writeLine("You eat some of what you've gathered");
				rangeVariables["food"]--;
				rangeVariables["hunger"] +=2;
				if(rangeVariables["hunger"]>100){
					rangeVariables["hunger"] = 100;
				}
			}
		}
		else {
			textControl.writeLine("There's nothing to eat.");
		}
	}

	//hungerAi
	Timer hungerTimer;

	public void hungerAi(){
		if (hungerTimer.isDone ()) {
			rangeVariables["hunger"]--;
			if(rangeVariables["hunger"]<0){
				rangeVariables["hunger"] = 0;
			}
			if (rangeVariables ["hunger"] == 75) {
				textControl.writeLine("You are a bit peckish.");
			}
			if (rangeVariables ["hunger"] == 25) {
				textControl.writeLine("Your stomach rumbles.");
			}
			if(rangeVariables ["hunger"] < 10){
				textControl.writeLine("You feel faint.\n You should eat soon.");
			}
			hungerTimer.startTimer(10f);
		}
	}

}
