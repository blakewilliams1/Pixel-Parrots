using UnityEngine;
using System.Collections;

public class WildBird : MonoBehaviour {
	int escapesLeft=3;
	bool runningAway=false;
	public Branch currPerch;
	public float health = 100;
	public float stamina = 100;
	public float maxAltitude = 50;
	public float maxSpeed = 10;
	//don't change this color here!! Change it in specific bird
	public Color baseColor = Color.magenta;

	public void Start(){
		findClosestPerch();
		baseColor=Color.cyan;
		GetComponent<SpriteRenderer>().material.color=baseColor;
	}
	
	void Update(){
		if(!atCurrPerch())flyToPerch();
		stayAlert();
	}

	float distFromPlayer(){
		Bird player = (Bird)Object.FindObjectOfType(typeof(Bird));
		if(player==null)return -1;
		return Vector2.Distance(transform.position,player.transform.position);
	}

	void stayAlert(){
		if(distFromPlayer()<1){
			if(!runningAway&&escapesLeft>0){

				escapesLeft--;
				runningAway=true;
				//TODO: pick random direction for a bit, then find closest instead
				Branch[] branches = (Branch[])Object.FindObjectsOfType(typeof(Branch));
				currPerch=branches[Mathf.FloorToInt(Random.value*branches.Length)];

			}else if(runningAway&&escapesLeft==0){
				//caught

			}
			runningAway=true;
		}
		if(runningAway&&distFromPlayer()>1.2){
			runningAway=false;
		}
	}

	bool atCurrPerch(){
		if (currPerch == null)return false;
		return Vector2.Distance (currPerch.transform.position, transform.position)<0.1;
	}
	
	void flyToPerch(){
		if (currPerch == null) {
			return;
		}
		Vector2 branchPos = currPerch.transform.position;
		transform.position = Vector2.Lerp(transform.position, branchPos, 10f*Time.deltaTime);
		if(Vector2.Distance(branchPos,transform.position)<0.1){
			transform.position=branchPos;
		}
	}

	void findClosestPerch (){
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
}
