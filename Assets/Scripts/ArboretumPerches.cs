using UnityEngine;
using System.Collections;

public class ArboretumPerches : MonoBehaviour {

	void Start () {
		//generate perches on the background
		for(int i=0;i<10;i++){
			Vector3 pos = new Vector3((Random.value*80f)-40f,(Random.value*40f)-20f,-0.01f);
			createPerch(pos);
		}
		ArboretumBird[] birds = (ArboretumBird[])Object.FindObjectsOfType(typeof(ArboretumBird));
		foreach(ArboretumBird b in birds)b.findNewSpot();
	}
	
	void Update () {
	
	}
	void createPerch(Vector3 pos){
		GameObject newBranch = new GameObject("branch");
		newBranch.AddComponent<Branch>();
		newBranch.transform.parent=transform;
		newBranch.transform.position=pos;
		//Texture2D tex = (Texture2D)Resources.Load("branch");
		//Sprite s = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f),100);
		//newBranch.AddComponent<SpriteRenderer>();
		//newBranch.GetComponent<SpriteRenderer>().sprite=s;
	}
}
