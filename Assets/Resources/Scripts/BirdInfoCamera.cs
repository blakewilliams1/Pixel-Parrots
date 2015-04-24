using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BirdInfoCamera : MonoBehaviour {
	float startTime;
	public ArboretumBird currBird;
	GameObject bird;


	void Start () {
		startTime=Time.time;
		currBird = (ArboretumBird)Object.FindObjectOfType(typeof(ArboretumBird));
		showStats();
	}
	
	void Update () {
		if (Input.GetKey(KeyCode.Escape)&&Time.time-startTime>0.5f){
			Application.LoadLevel("ArboretumScene");
		}
	}
	
	 void showStats(){
		GameObject nameText = GameObject.Find ("Name");
		nameText.GetComponent<Text> ().text = currBird.stats.birdName;

		GameObject typeText = GameObject.Find ("Type");
		typeText.GetComponent<Text> ().text = "Type  = "+currBird.stats.type;

		GameObject speedText = GameObject.Find ("Speed");
		speedText.GetComponent<Text> ().text = "Speed = "+currBird.stats.maxSpeed;

		GameObject staminaText = GameObject.Find ("Stamina");
		staminaText.GetComponent<Text> ().text = "Stamina = "+currBird.stats.stamina;

		GameObject heightText = GameObject.Find ("Max Height");
		heightText.GetComponent<Text> ().text = "Max Height = "+currBird.stats.maxAltitude;

		GameObject flyText = GameObject.Find ("Can Fly");
		flyText.GetComponent<Text> ().text = "Can Free Fly = "+currBird.stats.canFreeFly.ToString();

		GameObject icon = GameObject.Find ("Icon");
		icon.GetComponent<Image>().sprite = currBird.GetComponent<SpriteRenderer>().sprite;
		Color color = new Color(currBird.stats.baseColor[0],currBird.stats.baseColor[1],currBird.stats.baseColor[2]);
		icon.GetComponent<Image>().color = color;

		ArboretumBird[] birds = (ArboretumBird[])Object.FindObjectsOfType(typeof(ArboretumBird));
		foreach(ArboretumBird b in birds){
			if(b!=currBird)Destroy(b.gameObject);
			else {
				bird=b.gameObject;
				b.alive=false;
				Destroy(bird.GetComponent<SpriteRenderer>());
			}
		}
	}

	public void goToOpenWorld(){
		if (Time.time-startTime>0.5f){
			GameController control=(GameController)Object.FindObjectOfType(typeof(GameController));
			DontDestroyOnLoad(bird);
			DontDestroyOnLoad(control.gameObject);
			Application.LoadLevel("OpenWorldScene"); 
		}
	}

	public void goToArboretum(){
		if (Time.time-startTime>0.5f){
			//GameController control = (GameController)Object.FindObjectOfType(typeof(GameController));
			//control.loadArboretumBirds();
			DestroyImmediate(bird);
			Application.LoadLevel("ArboretumScene");
		}
	}

	public void sellBird(){
		GameObject birds = GameObject.Find("Birds");
		if(birds.transform.childCount>1){
			GameController control = (GameController)Object.FindObjectOfType(typeof(GameController));
			control.credits++;
			ArboretumBird oldBird = (ArboretumBird)Object.FindObjectOfType(typeof(ArboretumBird));
			control.removeBirdRef(oldBird);
			goToArboretum();
		}else{
			print ("Cannot sell your only bird!");
		}

	}
}
