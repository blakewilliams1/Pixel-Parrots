using UnityEngine;
using System.Collections;

public class WildBird : Bird {
	int escapesLeft=3;
	bool runningAway=false;
	Vector2 escapeDir;
	Time escapeTime;
	public Branch currPerch;

	public void Start(){
		stats.birdName=names[Mathf.FloorToInt(Random.value*names.Length)];
		findClosestPerch();
		Color newColor;
		if(Random.value<0.6f){
			newColor=colors[Mathf.FloorToInt(Random.value*colors.Length)];
		}else newColor = Color.white;
		stats.baseColor[0]=newColor.r;
		stats.baseColor[1]=newColor.g;
		stats.baseColor[2]=newColor.b;
		Color color = new Color(stats.baseColor[0],stats.baseColor[1],stats.baseColor[2]);
		GetComponent<SpriteRenderer>().material.color=color;
		OpenWorldBird player = (OpenWorldBird)Object.FindObjectOfType(typeof(OpenWorldBird));
		stats.stamina=Mathf.Floor(Random.value*100+50f);
		stats.maxSpeed=Mathf.Floor(player.stats.maxSpeed+Random.value*8-3.5f);
		stats.maxAltitude=Mathf.Floor(Random.value*50+25f);
	}
	
	void Update(){
		if(runningAway){
			runWay();
		}else if(!atCurrPerch()){
			flyToPerch();
		}
		stayAlert();
	}

	void runWay (){
		transform.position = Vector2.MoveTowards(transform.position, escapeDir*100, stats.maxSpeed*Time.deltaTime);
	}

	float distFromPlayer(){
		OpenWorldBird player = (OpenWorldBird)Object.FindObjectOfType(typeof(OpenWorldBird));
		if(player==null)return -1;
		return Vector2.Distance(transform.position,player.transform.position);
	}

	void stayAlert(){
		if(distFromPlayer()<1){
			if(!runningAway&&escapesLeft>0){
				escapesLeft--;
				runningAway=true;
				GetComponent<Animator>().SetTrigger("startFlying");
				GetComponent<Animator>().ResetTrigger("rest");
				//TODO: pick random direction for a bit, then find closest instead
				escapeDir.x=Random.value;
				escapeDir.y=Random.value;
				escapeDir=escapeDir.normalized;
				//Branch[] branches = (Branch[])Object.FindObjectsOfType(typeof(Branch));
				//currPerch=branches[Mathf.FloorToInt(Random.value*branches.Length)];
				Vector3 flip = transform.localScale;
				if(escapeDir.x>0){
					flip.x =-Mathf.Abs(flip.x);
				}else flip.x =Mathf.Abs(flip.x);
				transform.localScale = flip;
			}else if(runningAway&&escapesLeft==0){
				addToArboretum();
				Destroy(gameObject);
			}
			runningAway=true;
		}
		if(runningAway&&distFromPlayer()>7f){
			runningAway=false;
			findClosestPerch();
		}
	}

	bool atCurrPerch(){
		if (currPerch == null)return false;
		bool result =Vector2.Distance (currPerch.transform.position, transform.position)<0.1;
		if(result){
			GetComponent<Animator>().SetTrigger("rest");
			GetComponent<Animator>().ResetTrigger("startFlying");
		}
		return result;
	}
	
	void flyToPerch(){
		if (currPerch == null) {
			return;
		}
		Vector2 branchPos = currPerch.transform.position;
		transform.position = Vector2.MoveTowards(transform.position, branchPos, stats.maxSpeed*Time.deltaTime);
		if(Vector2.Distance(branchPos,transform.position)<0.1){
			transform.position=branchPos;
		}
		Vector3 pos = transform.position;
		pos.z=-0.1f;
		transform.position=pos;
		//flip image

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

	void addToArboretum (){
		GameController control = (GameController)Object.FindObjectOfType(typeof(GameController));
		control.addNewBirdRef(gameObject.GetComponent<WildBird>());
		print ("captured!");
	}
}
