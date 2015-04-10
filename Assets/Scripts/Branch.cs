using UnityEngine;
using System.Collections;

public class Branch: MonoBehaviour {
	Vector3 hexPos;
	// Use this for initialization
	void Start () {
		hexPos=transform.position;
		Vector3 screenPos = transform.position;
		float wiggle = 2.5f;
		screenPos.x+=Random.value*wiggle-wiggle/2;
		screenPos.y+=Random.value*wiggle-wiggle/2;
		transform.position=screenPos;
	}
	
	// Update is called once per frame
	void Update () {
		if(touched()){
			Bird firstParrot = (Bird)Object.FindObjectOfType(typeof(Bird));
			if(firstParrot!=null)firstParrot.setPerch(this);
		}
	}
	bool touched(){
		if (Input.touchCount==1&&Input.touches[0].phase==TouchPhase.Ended){
			Vector2 touchPos = Input.touches[0].position;
			touchPos=Camera.main.ScreenToWorldPoint(touchPos);
			return Vector2.Distance(touchPos,transform.position)<1;
		}
		return false;
	}
	public void moveBranch(){
		Vector3 oldVec = new Vector3();
		oldVec.x=transform.position.x;
		oldVec.y=transform.position.y;
		oldVec.z=transform.position.z;
		Vector3 newPos = Camera.main.WorldToScreenPoint(transform.position);
		float marg = 100;
		newPos.x=(newPos.x+Screen.width*4/3+marg)%(Screen.width*4/3)-marg;
		newPos.y=(newPos.y+Screen.height*4/3+marg)%(Screen.height*4/3)-marg;
		newPos=Camera.main.ScreenToWorldPoint(newPos);
		transform.position=newPos;
		if(Vector3.Distance(transform.position,oldVec)>1){
			//the branch moved. Delete npc if on branch, random spawn item
			WildBird[] npcs = (WildBird[])Object.FindObjectsOfType(typeof(WildBird));
			foreach(WildBird b in npcs)if(b.currPerch==this)Destroy(b.gameObject);
			if(Random.value<0.05){
				//spawn something
				spawnNPCBird();
			}
		}
	}
	void spawnNPCBird(){
		GameObject bird = new GameObject("NPC Bird");
		bird.transform.position=transform.position;
		bird.AddComponent<SpriteRenderer>();
		bird.AddComponent<WildBird>();
		Texture2D tex = (Texture2D)Resources.Load("parrot");
		Sprite s = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f),100);
		bird.GetComponent<SpriteRenderer>().sprite=s;
	}
}
