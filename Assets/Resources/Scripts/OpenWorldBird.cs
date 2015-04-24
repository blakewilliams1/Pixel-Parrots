using UnityEngine;
using System.Collections;

public class OpenWorldBird : Bird {
	Branch currPerch;
	GameObject thumbpadRef;
	GameObject altiRef;
	public bool flying = false;
	Vector2 flyingDir = new Vector2();
	bool resting = false;
	public void Start(){
		GetComponent<Animator>().SetTrigger("rest");
		Color color = new Color(stats.baseColor[0],stats.baseColor[1],stats.baseColor[2]);
		GetComponent<SpriteRenderer>().material.color=color;
		thumbpadRef=GameObject.Find("ThumbPad");
		thumbpadRef.SetActive(flying);
		altiRef=GameObject.Find("Altimeter");
		altiRef.SetActive(flying);
        findClosestBranch();
	}
	
	public void Update(){
		if(tappedSelf()&&stats.canFreeFly){
			flying=!flying;
			if(flying){
				GetComponent<Animator>().SetTrigger("startFlying");
				GetComponent<Animator>().ResetTrigger("rest");
			}else{
				GetComponent<Animator>().SetTrigger("rest");
				findClosestBranch();
			}
			thumbpadRef.SetActive(flying);
			altiRef.SetActive(flying);
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
		if(currPerch==perch)return;
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
		if(result){
			GetComponent<Animator>().SetTrigger("rest");
		}
		return result;
	}
	
	void flyToPerch(){
		if (currPerch == null) {
			return;
		}
		GetComponent<Animator>().SetTrigger("startFlying");
		Vector3 branchPos = currPerch.transform.position;
		transform.position = Vector2.MoveTowards(transform.position, branchPos, stats.maxSpeed*Time.deltaTime);
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
