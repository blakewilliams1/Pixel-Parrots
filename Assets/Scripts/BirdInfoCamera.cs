using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BirdInfoCamera : MonoBehaviour {
	float startTime;
	public ArboretumBird currBird;

	void Start () {
		startTime=Time.time;
		currBird = (ArboretumBird)Object.FindObjectOfType(typeof(ArboretumBird));
		showStats();
	}
	
	void Update () {
		if (Input.GetKey(KeyCode.Escape)&&Time.time-startTime>0.5f){
			Application.LoadLevel("ArboretumScene");
			//pressed the back button
		}
	}
	
	 void showStats(){
		GameObject nameText = GameObject.Find ("Name");
		nameText.GetComponent<Text> ().text = currBird.name;

		GameObject typeText = GameObject.Find ("Type");
		typeText.GetComponent<Text> ().text = "Type  = "+currBird.type;

		GameObject speedText = GameObject.Find ("Speed");
		speedText.GetComponent<Text> ().text = "Speed = "+currBird.speed;

		GameObject staminaText = GameObject.Find ("Stamina");
		staminaText.GetComponent<Text> ().text = "Stamina = "+currBird.stamina;

		GameObject heightText = GameObject.Find ("Max Height");
		heightText.GetComponent<Text> ().text = "Max Height = "+currBird.maxAtlitude;

		GameObject icon = GameObject.Find ("Icon");
		icon.GetComponent<Image>().sprite = currBird.GetComponent<SpriteRenderer>().sprite;
		currBird.gameObject.SetActive(false);
	}

	public void goToOpenWorld(){
		if (Time.time-startTime>0.5f){
			Application.LoadLevel("OpenWorldScene");
			//pressed the back button
		}
	}

	public void goToArboretum(){
		if (Time.time-startTime>0.5f){
			GameController control = (GameController)Object.FindObjectOfType(typeof(GameController));
			control.loadArboretumBirds();
			Application.LoadLevel("ArboretumScene");
		}
	}
}
