using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class GameController : MonoBehaviour {

	void Start () {

	}
	
	void Update () {

	}

	void OnLevelWasLoaded(int level){
		if(Application.loadedLevelName.CompareTo("ArboretumScene")==0){
			loadArboretumBirds();
		}
		if(Application.loadedLevelName.CompareTo("OpenWorldScene")==0){
			loadOpenWorld();
		}
	}

	public void loadArboretumBirds (){
		foreach(Transform child in transform.Find ("Birds").transform){
			child.gameObject.SetActive(true);
		}
	}

	void loadOpenWorld (){
		foreach(Transform child in transform.Find ("Birds").transform){
			child.gameObject.SetActive(false);
		}
		//TODO get info from the displayed bird and transfer to open world
	}

	public void selectBird(GameObject bird){
		foreach(Transform child in GameObject.Find("Birds").transform){
			if(bird.GetInstanceID()!=child.gameObject.GetInstanceID()){
				child.gameObject.SetActive(false);
			}else print ("found the selected one!");
		}
	}
	
	public void Save(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath+"/PixelParrotsSave.dat",FileMode.Open);
		GameObject data = GameObject.Find("GameController");
		if(data!=null){
			bf.Serialize(file, data);
			file.Close();
		}else{
			print("Error! Could not serialize save data!");
		}
	}

	public void Load(){
		if (File.Exists(Application.persistentDataPath + "/drawing.draw")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/PixelParrotsSave.dat", FileMode.Open);
			GameObject data = (GameObject)bf.Deserialize(file);
			file.Close();
		}
	}
}
