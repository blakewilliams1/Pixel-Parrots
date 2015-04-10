using UnityEngine;
using System.Collections;

public class OpenWorldCamera : MonoBehaviour {
	Branch[] branches =new Branch[0];
	Bird firstParrot;
	Vector2 cameraPos2d;
	float startTime;
	public float width;
	public float height;


	// Use this for initialization
	void Start () {
		startTime = Time.time;
		firstParrot = (Bird)Object.FindObjectOfType(typeof(Bird));
	}
	
	// Update is called once per frame
	void Update () {
		if(branches.Length==0){
			branches=(Branch[])Object.FindObjectsOfType(typeof(Branch));
		}
		foreach(Branch b in branches){
			b.moveBranch();
		}
		followActiveParrot();
		if (Input.GetKey(KeyCode.Escape)&&Time.time-startTime>0.5f){
			Application.LoadLevel("ArboretumScene");
			//pressed the back button
		}
	}
	
	void followActiveParrot(){
		Vector2 parrotPos2d = firstParrot.transform.position;
		cameraPos2d = new Vector2 (transform.position.x,transform.position.y);
		cameraPos2d = Vector2.Lerp(cameraPos2d, parrotPos2d, 10*Time.deltaTime);
		if (Vector2.Distance (parrotPos2d, transform.position) < 0.1) {
			cameraPos2d = parrotPos2d;
		}
		transform.position = new Vector3 (cameraPos2d.x, cameraPos2d.y, -10f);
	}
	
}
