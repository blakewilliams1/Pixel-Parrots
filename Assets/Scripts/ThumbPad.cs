using UnityEngine;
using System.Collections;

public class ThumbPad : MonoBehaviour {
	Vector2 restingPos;
	public bool held=false;
	public Vector2 direction;
	private float pullDist = 80;
	// Use this for initialization
	void Start () {
		restingPos = new Vector2(Screen.width/9,Screen.width/9);
		Vector3 pos=Camera.main.ScreenToWorldPoint(restingPos);
		pos.z=-0.02f;
		transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
		//resizePad();
		dragPad();
	}

	void resizePad (){
		float scale = Camera.main.orthographicSize/4;
		transform.localScale=new Vector3(scale,scale,0);
	}

	void dragPad (){
		if(Input.touchCount==1){
			float maxDist = Screen.width/5;
			if(Input.touches[0].phase==TouchPhase.Began&&
			   	Vector2.Distance(restingPos,Input.touches[0].position)<maxDist){
				held=true;
			}
			if(Input.touches[0].phase==TouchPhase.Ended){
				held=false;
			}
			if(held){
				float dist = Vector2.Distance(restingPos,Input.touches[0].position);
				dist = Mathf.Clamp(dist,-pullDist,pullDist);
				Vector2 drag = Input.touches[0].position-restingPos;
				drag = drag.normalized*dist;
				direction=drag.normalized*100;
				transform.position=restingPos+drag;
			}else{
				direction=new Vector2(0,0);
				transform.position=restingPos;
			}
		}else{
			direction=new Vector2(0,0);
			transform.position=restingPos;
		}
	}
}
