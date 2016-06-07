using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class TextControl : MonoBehaviour {

	public GameObject textObj;
	List<GameObject> textList;
	public Font myFont;
	float alpha;
	string allTheText;
	List<string> completeStoryText;
	List<GameObject> bufferList;
	StreamWriter sw;
	Timer timer;
	float timertime = 0f;
	// Use this for initialization
	void Start () {
		timer = GameObject.Instantiate (GameObject.Find ("TimerObject").GetComponent<Timer>()) as Timer;
		textList = new List<GameObject>();
		bufferList = new List<GameObject> ();
		timer.startTimer (timertime);
		for (int i = 0; i<9; i++) {
			writeLine("");	
		}
		timertime = 1f;
		completeStoryText = new List<string>();

	}
	
	// Update is called once per frame
	void Update () {
		if(timer.isDone()){
			writingBuffer ();
		}
	}

	void writingBuffer(){
		if (bufferList.Count > 0) {
			GameObject obj = bufferList[0];
			textList.Add(obj);
			bufferList.Remove(obj);
			if (textList.Count > 10) {
				GameObject tempDestrObj = textList[0];
				textList.RemoveAt(0);
				Destroy(tempDestrObj);
			}
			moveTextAround ();
			timer.startTimer(timertime);
		}
		else{
			timer.startTimer(0.1f);
		}
	}

	public void writeLine(string line){
		//timer.startTimer (1f);

		//allTheText = allTheText+"\r\n"+line;
		if (!line.Equals ("")) {
			completeStoryText.Add (line);
		}

		GameObject myText = GameObject.Instantiate (textObj) as GameObject;
		myText.transform.GetComponent<Text>().text = line;
		myText.transform.GetComponent<Text> ().font = myFont;
		myText.transform.GetComponent<Text> ().color = new Color (1f, 1f, 1f, 0f);
		myText.transform.SetParent (GameObject.Find ("Canvas").transform);
		bufferList.Add (myText);
//		textList.Add (myText);
//					if (textList.Count > 10) {
//						GameObject tempDestrObj = textList[0];
//						textList.RemoveAt(0);
//						Destroy(tempDestrObj);
//						
//					}
//					moveTextAround ();
		
	}
	
	void moveTextAround(){
		float x = Screen.width/2;
		float y = Screen.height-60;
		Vector3 tempVect3 = new Vector3 (x, y, 0f);
		float tempTop = y;
		int tempInt = 0;
		alpha = 0.1f;
		foreach (GameObject obj in textList){
			//alpha = 0.1f;
			//while(alpha < 1 ){
				obj.transform.GetComponent<Text> ().color = new Color (1f, 1f, 1f, alpha);
				alpha += 0.1f;
			//}
			textList[tempInt].transform.position = tempVect3;
			tempTop -= 65f;
			tempVect3 = new Vector3(x,tempTop,0f);
			tempInt++;
		}
	}

	public List<string> getAllText(){
		//return allTheText;
		return completeStoryText;
	}

	public void replaceAllText(List<string> lineList){
		ArrayList destroyThese = new ArrayList ();
		foreach(GameObject body in textList){
			int place = textList.IndexOf(body);
			destroyThese.Add(body);
			GameObject destBody = destroyThese[place] as GameObject;
			Destroy(destBody);
		}
		textList.Clear();

		if (lineList.Count < 10) {
			while(lineList.Count+textList.Count<10){
				GameObject myText = GameObject.Instantiate (textObj) as GameObject;
				myText.transform.GetComponent<Text> ().text = "";
				myText.transform.GetComponent<Text> ().font = myFont;
				myText.transform.GetComponent<Text> ().color = new Color (1f, 1f, 1f, 0f);
				myText.transform.SetParent (GameObject.Find ("Canvas").transform);
				textList.Add (myText);
			}
		}

		foreach (string line in lineList) {
				GameObject myText = GameObject.Instantiate (textObj) as GameObject;
				myText.transform.GetComponent<Text> ().text = line;
			myText.transform.GetComponent<Text> ().font = myFont;
			myText.transform.GetComponent<Text> ().color = new Color (1f, 1f, 1f, 0f);
			myText.transform.SetParent (GameObject.Find ("Canvas").transform);
			textList.Add (myText);
			if (textList.Count > 10) {
				GameObject tempDestrObj = textList[0];
				textList.RemoveAt(0);
				Destroy(tempDestrObj);
			}
			moveTextAround ();
		}

	}

	public void writeToFile(string NameOfFile){
		if (!Directory.Exists (Application.persistentDataPath + "/stories")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/stories");
		}
		sw = new StreamWriter (Application.persistentDataPath + "/stories/" + NameOfFile + ".txt");
		string writeThis = "";
		foreach (string line in completeStoryText) {
			if(!(line.Equals(""))){
				writeThis += line+"\r\n";
			}
		}
		sw.WriteLine (writeThis);
		sw.Close ();
	}

	public void writeToFileEvent(string allTheEvents){
		if (!Directory.Exists (Application.persistentDataPath + "/events")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/events");
		}
		sw = new StreamWriter (Application.persistentDataPath + "/events/event_names.txt");
		sw.WriteLine (allTheEvents);
		sw.Close ();
	}

	public void writeToFileEvent(string allTheEvents, string name){
		if (!Directory.Exists (Application.persistentDataPath + "/events")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/events");
		}
		sw = new StreamWriter (Application.persistentDataPath + "/events/"+name+".txt");
		sw.WriteLine (allTheEvents);
		sw.Close ();
	}
}
