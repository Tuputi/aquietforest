using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveFile {

	public static SaveFile current;
	public List<string> completeStory;
	public Dictionary<string,float> rangedVariablesSave;
	public Dictionary<string, string> stateVariablesSave;
	public Dictionary<string, bool> boolVariablesSave;

	//public string name = "temp";
	//add variables here later

	public SaveFile(){
		completeStory = new List<string>();
		rangedVariablesSave = new Dictionary<string, float>();
		stateVariablesSave = new Dictionary<string, string>();
		boolVariablesSave = new Dictionary<string, bool>();
	}
}
