using UnityEngine;
using System.Collections;

public class ArboretumBird : Bird {
	Branch currPerch;
	float lastAction;
	float randomTime;
	bool resting = false;
	public bool alive = true;
	// Use this for initialization
	void Start () {
		findNewSpot();
		transform.position=currPerch.transform.position;
		Color color = new Color(stats.baseColor[0],stats.baseColor[1],stats.baseColor[2]);
		GetComponent<SpriteRenderer>().material.color=color;
		lastAction=Time.realtimeSinceStartup;
		randomTime=2+Random.value*4;
	}
	
	void Update () {
		if(!alive)return;
		flyToPerch();
		checkRandomEvent();
		if(touched()){
			DontDestroyOnLoad(transform.gameObject);
			Application.LoadLevel("BirdInfoScene");
		}
	}

	void checkRandomEvent (){
		if(Time.realtimeSinceStartup-lastAction>randomTime){
			lastAction = Time.realtimeSinceStartup;
			randomTime=2+Random.value*4;
			//decide which actions to take
			float choice = Random.value;
			if(choice<0.25f){
				//fly to new spot
				findNewSpot();
			}else if(choice<0.80f){
				//peck or turn
				if(!resting)return;
				Vector2 turn = transform.localScale;
				turn.x*=-1;
				transform.localScale=turn;
			}else{
				//chirp
				print ("chirp!");
			}
		}
	}

	public void findNewSpot (){
		Branch[] branches = (Branch[])Object.FindObjectsOfType(typeof(Branch));
		currPerch=branches[Mathf.FloorToInt(Random.value*branches.Length)];
		GetComponent<Animator>().SetTrigger("startFlying");
		GetComponent<Animator>().ResetTrigger("rest");
		resting=false;
		Vector2 flip=transform.localScale;
		if(currPerch.transform.position.x<transform.position.x){
			flip.x=Mathf.Abs(flip.x);
		}else {
			flip.x=-Mathf.Abs(flip.x);
		}
		transform.localScale=flip;
	}

	void flyToPerch(){
		if (currPerch == null) {
			return;
		}
		Vector3 branchPos = currPerch.transform.position;
		transform.position = Vector2.MoveTowards(transform.position, branchPos, 13f*Time.deltaTime);
		if(Vector2.Distance(branchPos,transform.position)<0.01&&!resting){
			GetComponent<Animator>().SetTrigger("rest");
			GetComponent<Animator>().ResetTrigger("startFlying");
			transform.position=branchPos;
			resting=true;
		}
		Vector3 pos = transform.position;
		pos.z=-0.1f;
		transform.position=pos;
    }
    
    void setPerch(Branch perch){
		currPerch=perch;

	}
	bool touched(){
		if (Input.touchCount==1&&Input.touches[0].phase==TouchPhase.Ended){
			Vector2 touchPos = Input.touches[0].position;
			touchPos=Camera.main.ScreenToWorldPoint(touchPos);
			return Vector2.Distance(touchPos,transform.position)<1.5f;
		}
		return false;
	}
	
}
