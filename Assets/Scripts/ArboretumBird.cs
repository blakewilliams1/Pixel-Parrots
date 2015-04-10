using UnityEngine;
using System.Collections;

public class ArboretumBird : MonoBehaviour {
	Branch currPerch;
	float lastAction;
	float randomTime;
	public string birdName = "Slinky";
	public string type = "Parrot";
	public float speed =10;
	public float maxAtlitude=50;
	public int stamina = 100;
	bool active=true;
	// Use this for initialization
	void Start () {
		lastAction=Time.realtimeSinceStartup;
		randomTime=2+Random.value*4;
	}
	
	void Update () {
		if(!active)return;
		flyToPerch();
		checkRandomEvent();
		if(touched()){
			DontDestroyOnLoad(transform.gameObject);
			GameObject.FindObjectOfType<GameController>().selectBird(gameObject);
			Application.LoadLevel("BirdInfo");
		}

	}

	void checkRandomEvent (){
		if(Time.realtimeSinceStartup-lastAction>randomTime){
			lastAction = Time.realtimeSinceStartup;
			randomTime=2+Random.value*4;
			//decide which actions to take
			float choice = Random.value;
			if(choice<0.2f){
				//fly to new spot
				findNewSpot();
			}else if(choice<0.7f){
				//peck or turn
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
	}

	void flyToPerch(){
		if (currPerch == null) {
			return;
		}
		Vector3 branchPos = currPerch.transform.position;
		transform.position = Vector2.MoveTowards(transform.position, branchPos, 13f*Time.deltaTime);
		branchPos.z=-0.01f;
		if(Vector2.Distance(branchPos,transform.position)<0.1){
			transform.position=branchPos;
		}
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

	void transferData (){
		GameObject.DestroyImmediate(gameObject);
	}
}
