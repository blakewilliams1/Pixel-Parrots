using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class GameController : MonoBehaviour {
	public int credits = 0;
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
		int i=0;
		foreach(Transform child in transform.Find ("Birds").transform){
			Bird refBird = child.gameObject.GetComponent<Bird>();
			GameObject bird = new GameObject(refBird.stats.birdName);
			bird.transform.localScale=new Vector2(10,10);
			bird.AddComponent<SpriteRenderer>();
			bird.AddComponent<Animator>();
			bird.GetComponent<Animator>().runtimeAnimatorController=
				(RuntimeAnimatorController)Resources.Load("Animations/"+refBird.stats.type+"Anim");
            bird.AddComponent<ArboretumBird>();
			ArboretumBird b = bird.GetComponent<ArboretumBird>();
			b.stats.birdName=refBird.stats.birdName;
			b.stats.maxSpeed=refBird.stats.maxSpeed;
			b.stats.type=refBird.stats.type;
			b.stats.maxAltitude=refBird.stats.maxAltitude;
			b.stats.stamina=refBird.stats.stamina;
			b.name=refBird.stats.birdName;
			b.stats.baseColor=refBird.stats.baseColor;
			bird.name=refBird.stats.birdName;
			i++;
		}
	}

	public void loadOpenWorld (){
		//TODO get info from the displayed bird and transfer to open world bird
		GameObject bird = new GameObject("Player");
		bird.transform.localScale=new Vector2(10,10);
		bird.AddComponent<SpriteRenderer>();
		bird.AddComponent<Animator>();
		ArboretumBird refBird = (ArboretumBird)UnityEngine.Object.FindObjectOfType(typeof(ArboretumBird));
		bird.GetComponent<Animator>().runtimeAnimatorController=
			(RuntimeAnimatorController)Resources.Load("Animations/"+refBird.stats.type+"Anim");
		bird.AddComponent<OpenWorldBird>();
		//add attributes!
		OpenWorldBird b = bird.GetComponent<OpenWorldBird>();
		b.stats.baseColor=refBird.stats.baseColor;
		b.stats.canFreeFly=refBird.stats.canFreeFly;
		b.stats.stamina=refBird.stats.stamina;
		b.stats.maxAltitude=refBird.stats.maxAltitude;
		b.stats.maxSpeed=refBird.stats.maxSpeed;
		OpenWorldCamera camera = (OpenWorldCamera)UnityEngine.Object.FindObjectOfType(typeof(OpenWorldCamera));
		camera.player=bird.GetComponent<OpenWorldBird>();
		Destroy(refBird.gameObject);
    }
    
    
    public void addNewBirdRef(Bird refBird){
		GameObject newBird = new GameObject("Tamed");
		newBird.transform.parent=transform.Find("Birds");
		newBird.AddComponent<Bird>();
		Bird b = newBird.GetComponent<Bird>();
		b.stats.birdName=refBird.stats.birdName;
		b.stats.maxSpeed=refBird.stats.maxSpeed;
		b.stats.type=refBird.stats.type;
		b.stats.maxAltitude=refBird.stats.maxAltitude;
		b.stats.stamina=refBird.stats.stamina;
		b.stats.baseColor=refBird.stats.baseColor;
		Save();
	}

	public void removeBirdRef(Bird refBird){
		GameObject control = GameObject.Find("GameController");
		Bird[] birdRefs = control.GetComponentsInChildren<Bird>();
		foreach(Bird currBird in birdRefs){
			print(currBird.stats);
			//TODO: properly compare the stats
			if(currBird.stats==refBird.stats){
				print ("selling "+currBird.stats.birdName);
				Destroy(refBird.gameObject);
				Save();
				return;
			}
		}
	}

	public void Save(){
		GameObject birdHolder = transform.FindChild("Birds").gameObject;
		Bird.Stats[] birds = new Bird.Stats[birdHolder.transform.childCount];
		for(int i=0;i<birdHolder.transform.childCount;i++){
			birds[i]=birdHolder.transform.GetChild(i).GetComponent<Bird>().stats;
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath+"/PixelParrotsSave.dat",FileMode.Create);
		if(birds.Length>0){
			bf.Serialize(file, birds);
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
			DontDestroyOnLoad(data);
		}
	}
}
