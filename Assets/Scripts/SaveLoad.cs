using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad{

	public static List<SaveFile> saveFiles = new List<SaveFile>();
	public static List<EventSaveFile> savedEvents = new List<EventSaveFile>();
	public static List<string> aforismerSave = new List<string>();

	public static void Save(string name){
		if (!Directory.Exists (Application.persistentDataPath + "/save_files")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/save_files");
		}
		saveFiles.Add (SaveFile.current);
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/save_files/"+name+".sg");
		bf.Serialize (file, SaveLoad.saveFiles);
		file.Close();
	}

	public static void SaveEvent(EventSaveFile ev){
		if (!Directory.Exists (Application.persistentDataPath + "/events")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/events");
		}
		savedEvents.Add (ev);
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/events/events.ev");
		bf.Serialize (file, SaveLoad.savedEvents);
		file.Close();
		Debug.Log ("Event saved");

	}

	public static void SaveWisdom(string aforism){
		if (!Directory.Exists (Application.persistentDataPath + "/wisdoms")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/wisdoms");
		}
		aforismerSave.Add (aforism);
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/wisdoms/wisdoms.ev");
		bf.Serialize (file, SaveLoad.aforismerSave);
		file.Close();
		Debug.Log ("Wisdom saved");
		
	}

	private static void internalSave(){
		if (!Directory.Exists (Application.persistentDataPath + "/events")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/events");
		}
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/events/events.ev");
		bf.Serialize (file, SaveLoad.savedEvents);
		file.Close();
		Debug.Log ("internal save");
	}

	private static void internalSaveAfo(){
		if (!Directory.Exists (Application.persistentDataPath + "/wisdoms")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/wisdoms");
		}
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/wisdoms/wisdoms.ev");
		bf.Serialize (file, SaveLoad.aforismerSave);
		file.Close();
		Debug.Log ("internal save af");
	}

	public static string getEventNames(){
		string allNames = "";
		foreach (EventSaveFile esf in savedEvents) {
			allNames = allNames+"\r\n"+esf.eventName;
		}
		return allNames;
	}

	public static string getAforismerText(){
		string allAforismer = "";
		foreach (string af in aforismerSave) {
			allAforismer = allAforismer+"\r\n"+af;
		}
		return allAforismer;
	}
	
	public static void removeEvent(string name){
		EventSaveFile removeMe = null;
		foreach (EventSaveFile esf in savedEvents) {

			if((esf.eventName.ToLower()).Equals(name.ToLower())){
				removeMe = esf;
			}
		}
		if (removeMe != null) {
			savedEvents.Remove (removeMe);
			Debug.Log ("Event removed");
			internalSave();
		} else {
			Debug.Log("Didn't find event to remove");
		}
	}

	public static void removeWisdom(string wisdom){
		string removeMe = null;
		foreach (string esf in aforismerSave) {
			if((esf.ToLower().Equals(wisdom.ToLower()))){
				removeMe = esf;
			}
		}
		if (removeMe != null) {
			aforismerSave.Remove (removeMe);
			Debug.Log ("Wisdom removed");
			internalSaveAfo();
		} else {
			Debug.Log("Didn't find wisdom to remove");
		}
	}

	public static List<EventSaveFile> getSavedFiles(){
		return savedEvents;
	}

	public static void Load(string name){
		if (File.Exists (Application.persistentDataPath + "/save_files/"+name+".sg")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/save_files/"+name+".sg",FileMode.Open);
			SaveLoad.saveFiles = (List<SaveFile>)bf.Deserialize(file);
			file.Close();
		}

	}

	public static void LoadEvent(){
		if (File.Exists (Application.persistentDataPath + "/events/events.ev")) {
						BinaryFormatter bf = new BinaryFormatter ();
						FileStream file = File.Open (Application.persistentDataPath + "/events/events.ev", FileMode.Open);
						SaveLoad.savedEvents = (List<EventSaveFile>)bf.Deserialize (file);
						file.Close ();
		} else {
			Debug.Log("Couldn't find events");
		}
	}

	public static void LoadAforismer(){
		if (File.Exists (Application.persistentDataPath + "/wisdoms/wisdoms.ev")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/wisdoms/wisdoms.ev", FileMode.Open);
			SaveLoad.aforismerSave = (List<string>)bf.Deserialize (file);
			file.Close ();
		} else {
			Debug.Log("Couldn't find aforismer");
		}
	}
}
