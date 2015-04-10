using UnityEngine;
using System.Collections;

public class ArboretumCamera : MonoBehaviour {
	float startTime;
	Vector2 prevVec;
	int fingerCount=0;
	void Start () {
		startTime=Time.time;
		if(GameObject.Find("GameController")==null){
			GameObject control = new GameObject("GameController");
			control.AddComponent<GameController>();
			GameObject birds = new GameObject("Birds");
			birds.transform.parent=control.transform;
			for(int i=0;i<3;i++){
			GameObject firstBird = new GameObject("Tamed");
			firstBird.AddComponent<SpriteRenderer>();
			Texture2D tex = (Texture2D)Resources.Load("pixel parrot");
			Sprite s = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f),100);
			firstBird.GetComponent<SpriteRenderer>().sprite=s;
			firstBird.AddComponent<ArboretumBird>();
			firstBird.transform.parent=birds.transform;
			}
		}
		DontDestroyOnLoad(GameObject.Find("GameController"));
	}

	void Update() {
		dragCamera();
		zoomCamera();
		restrictCamera();
		if (Input.GetKey(KeyCode.Escape)&&Time.time-startTime>0.5f){
			Application.LoadLevel("OpenWorldScene");
		}
	}
	void dragCamera(){
		if(Input.touchCount > 0){
			if(fingerCount-Input.touchCount==1){
				prevVec=Input.GetTouch(0).position;
			}
			fingerCount=Input.touchCount;
			if(Input.GetTouch(0).phase == TouchPhase.Began){
				prevVec=Input.GetTouch(0).position;
			}
			if(Input.GetTouch(0).phase == TouchPhase.Moved) {
				Vector2 touchVec = Input.GetTouch(0).position;
				Vector2 start = Camera.main.ScreenToWorldPoint(prevVec);
				Vector2 end = Camera.main.ScreenToWorldPoint(touchVec);
				Vector2 deltaVec = end - start;
				transform.Translate(-deltaVec.x, -deltaVec.y, 0);
				prevVec=touchVec;
			}
		}
	}

	void zoomCamera(){
		if(Input.touchCount!=2)return;
		float orthoZoomSpeed = 0.0125f;
		Touch touchZero = Input.GetTouch(0);
		Touch touchOne = Input.GetTouch(1);
		Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
		Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
		float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
		float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
		float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
		Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize,2.5f,10f);
	}

	void restrictCamera (){
		float vertExtent = Camera.main.orthographicSize;  
		float horzExtent = vertExtent * Screen.width / Screen.height;
		//next 2 floats are hardcoded to background
		float mapX = 84f;
		float mapY = 40.6f;
		float minX = horzExtent - mapX / 2.0f;
		float maxX = mapX / 2.0f - horzExtent ;
		float minY = vertExtent - mapY / 2.0f;
		float maxY = mapY / 2.0f - vertExtent;
		Vector3 v3 = transform.position;
		v3.x = Mathf.Clamp(v3.x, minX, maxX);
		v3.y = Mathf.Clamp(v3.y, minY, maxY);
		transform.position = v3;
	}
}
