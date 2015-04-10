using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {
	GameObject thumbpadRef;
	GameObject altiRef;
	Branch currPerch;
	public int stamina = 100;
	public string type = "Parrot";
	public float health = 100;
	public float maxAltitude = 50;
	public float maxSpeed = 10;
	public bool canFreeFly = true;
	public bool flying = false;
	Vector2 flyingDir = new Vector2();
	public Color baseColor = Color.magenta;

	public virtual void Start () {
		findClosestBranch();
		thumbpadRef=GameObject.Find("ThumbPad");
		thumbpadRef.SetActive(flying);
		altiRef=GameObject.Find("Altimeter");
		altiRef.SetActive(flying);
	}
	
	public virtual void Update () {
		if(Application.loadedLevelName.CompareTo("OpenWorldScene")==0){
			updateOpenWorld();
		}else if(Application.loadedLevelName.CompareTo("ArboretumScene")==0){
			//TODO transfer arboretum functions to here
		}

	}

	void updateOpenWorld (){
		if(tappedSelf()){
			flying=!flying;
		GetComponent<Animator>().speed=flying?1:0;
			thumbpadRef.SetActive(flying);
			altiRef.SetActive(flying);
			if(!flying)findClosestBranch();
		}
		if(!flying&&!atCurrPerch())flyToPerch();
		if(flying)flyAround();
	}

	void flyAround(){
		//read input from the thumbpad
		Vector2 dir = thumbpadRef.GetComponent<ThumbPad>().direction;
		dir=dir/6;
		if(dir.x==0&&dir.y==0){
			transform.Translate(flyingDir*Time.deltaTime);
		}else{
			flyingDir = dir;
			transform.Translate(dir.x*Time.deltaTime,dir.y/2*Time.deltaTime,0);
		}
		Vector3 flip = transform.localScale;
		if(flyingDir.x>0)flip.x =-Mathf.Abs(flip.x);
		else flip.x =Mathf.Abs(flip.x);
		transform.localScale = flip;
	}

	public void setPerch(Branch perch){
		if(currPerch!=perch){
			GetComponent<Animator>().speed=1;
		}else return;
		Vector3 flip = transform.localScale;
		if(perch.transform.position.x>transform.position.x){
			flip.x =-Mathf.Abs(flip.x);
		}else flip.x =Mathf.Abs(flip.x);
        transform.localScale = flip;
		currPerch=perch;
	}
	bool atCurrPerch(){
		if (currPerch == null)return false;
		bool result = Vector2.Distance(currPerch.transform.position, transform.position)<0.1;
		if(result)GetComponent<Animator>().speed=0;
		return result;
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

	public void findClosestBranch(){
		float closest = 1000;
		Branch[] branches = (Branch[])Object.FindObjectsOfType(typeof(Branch));
		for (int i=0; i<branches.Length; i++) {
			float curr=Vector2.Distance(transform.position,branches[i].transform.position);
			if(curr<closest){
				closest=curr;
				currPerch=branches[i];
			}
		}
	}
	bool tappedSelf(){
		if (Input.touchCount==1&&Input.touches[0].phase==TouchPhase.Ended){
			Vector2 touchPos = Input.touches[0].position;
			touchPos=Camera.main.ScreenToWorldPoint(touchPos);
			return Vector2.Distance(touchPos,transform.position)<1;
		}
		return false;
	}
}
