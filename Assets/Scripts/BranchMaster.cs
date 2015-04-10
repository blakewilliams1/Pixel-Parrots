using UnityEngine;
using System.Collections;

public class BranchMaster : MonoBehaviour {
	// Use this for initialization
	void Start () {
		//prevOrient=Input.deviceOrientation.ToString();
		//generate branches onto the  field in hex pattern
		float rows = 3f;
		float cols = 3f;
		for(float x=0;x<rows;x++){
			for(float y=0.5f;y<cols;y+=0.75f){
				float inc = Mathf.RoundToInt((y-0.5f)%1.5f);
				inc/=2;
				float initX = (x+inc)*(Screen.width*4/3)/rows-Screen.width/6;
				float initY = y*(Screen.height*4/3)/cols-Screen.height/6;
				Vector3 vec = new Vector3(initX,initY,0.01f);
				vec = Camera.main.ScreenToWorldPoint(vec);
				createBranch(vec);
			}
		}
		Bird player=(Bird)Object.FindObjectOfType(typeof(Bird));
		player.findClosestBranch();
	}
	
	// Update is called once per frame
	void Update () {

	}

	/*void orientBranches(){
		Branch[] branches = (Branch[])Object.FindObjectsOfType(typeof(Branch));
		foreach(Branch b in branches){
			Vector3 newPos = Camera.main.WorldToScreenPoint(b.transform.position);
			newPos.x=newPos.y;
			newPos.y=Screen.height-newPos.x;
			newPos=Camera.main.ScreenToWorldPoint(newPos);
			newPos.z=0.01f;
			b.transform.position=newPos;
		}
	}*/


	void createBranch(Vector2 pos){
		GameObject newBranch = new GameObject("branch");
		newBranch.transform.parent=transform;
		newBranch.transform.position=pos;
		newBranch.AddComponent<Branch>();
		Texture2D tex = (Texture2D)Resources.Load("branch");
		Sprite s = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f),100);
		newBranch.AddComponent<SpriteRenderer>();
		newBranch.GetComponent<SpriteRenderer>().sprite=s;
	}
}
