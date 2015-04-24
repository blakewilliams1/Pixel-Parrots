using UnityEngine;
using System.Collections;

public class OpenWorldCamera : MonoBehaviour {
	Branch[] branches =new Branch[0];
	GameObject branchContainer;
	public OpenWorldBird player;
	Vector2 cameraPos2d;
	float startTime;
	public float width;
	public float height;


	// Use this for initialization
	void Start () {
		startTime = Time.time;
		branchContainer = new GameObject("Branches");
		generateBranches();

	}
	
	// Update is called once per frame
	void Update () {
		if(player==null){
			print ("player is null");
			return;
		}
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

	void generateBranches(){
		float rows = 3f;
		float cols = 3f;
		for (float x = 0; x < rows; x++) {
			for (float y = 0.5f; y < cols; y += 0.75f) {
				float inc = Mathf.RoundToInt ((y - 0.5f) % 1.5f);
				inc /= 2;
				float initX = (x + inc) * (Screen.width * 4 / 3) / rows - Screen.width / 6;
				float initY = y * (Screen.height * 4 / 3) / cols - Screen.height / 6;
				Vector3 vec = new Vector3 (initX, initY, 0.01f);
				vec = Camera.main.ScreenToWorldPoint (vec);
				createBranch (vec);
			}
		}
	}
	
	void followActiveParrot(){
		Vector2 parrotPos2d = player.transform.position;
		cameraPos2d = new Vector2 (transform.position.x,transform.position.y);
		cameraPos2d = Vector2.Lerp(cameraPos2d, parrotPos2d, 10*Time.deltaTime);
		if (Vector2.Distance (parrotPos2d, transform.position) < 0.1) {
			cameraPos2d = parrotPos2d;
		}
		transform.position = new Vector3 (cameraPos2d.x, cameraPos2d.y, -10f);
	}
	void createBranch(Vector2 pos){
		GameObject newBranch = new GameObject("branch");
		newBranch.transform.position=pos;
		newBranch.AddComponent<Branch>();
		Texture2D tex = (Texture2D)Resources.Load("Art/branch");
		Sprite s = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f),100);
		newBranch.AddComponent<SpriteRenderer>();
		newBranch.GetComponent<SpriteRenderer>().sprite=s;
		newBranch.transform.parent=branchContainer.transform;
	}
}
