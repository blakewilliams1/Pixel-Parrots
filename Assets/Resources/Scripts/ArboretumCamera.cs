using UnityEngine;
using System.Collections;

public class ArboretumCamera : MonoBehaviour {
	float startTime;
	float zoomStart;
	float oldZoom;
	Vector2 prevTouchZero;
	Vector2 prevMiddle;
	int fingerCount=0;

	void Start () {
		startTime=Time.time;
		if(GameObject.Find("GameController")==null){
			GameObject control = new GameObject("GameController");
			control.AddComponent<GameController>();
			GameObject birds = new GameObject("Birds");
			birds.transform.parent=control.transform;
			birds.AddComponent<Bird>();
			//TODO this is just a temp solution to the ref
			//that should be passed in
			Bird b = birds.GetComponent<Bird>();
			control.GetComponent<GameController>().addNewBirdRef(b);
			control.GetComponent<GameController>().loadArboretumBirds();
			Destroy(birds.GetComponent<Bird>());
		}
		DontDestroyOnLoad(GameObject.Find("GameController"));
	}

	void Update() {
		dragCamera();
		zoomCamera();
		restrictCamera();
		if (Input.GetKey(KeyCode.Escape)&&Time.time-startTime>0.5f){
			//pressed back button
		}
	}
	void dragCamera(){
		if(Input.touchCount > 0){
			if(Mathf.Abs(fingerCount-Input.touchCount)==1){
				prevTouchZero=Input.GetTouch(0).position;
			}
			if(Input.touchCount==1){
				if(Input.GetTouch(0).phase == TouchPhase.Began){
					prevTouchZero=Input.GetTouch(0).position;
				}
				if(Input.GetTouch(0).phase == TouchPhase.Moved) {
					Vector2 touchVec = Input.GetTouch(0).position;
					Vector2 start = Camera.main.ScreenToWorldPoint(prevTouchZero);
					Vector2 end = Camera.main.ScreenToWorldPoint(touchVec);
					Vector2 deltaVec = end - start;
					transform.Translate(-deltaVec.x, -deltaVec.y, 0);
					prevTouchZero=touchVec;
				}
			}else if(Input.touchCount>=2){
				//TODO: allow finger ownership to transfer when original fingers are lifted
				Vector2 touchZero = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
				Vector2 touchOne = Camera.main.ScreenToWorldPoint(Input.touches[1].position);
				if(fingerCount<=1){
					prevMiddle=Vector2.Lerp(Input.touches[0].position,Input.touches[1].position,0.5f);
					prevMiddle=Camera.main.ScreenToWorldPoint(prevMiddle);
				}
				Vector2 currMiddle = Vector2.Lerp(touchOne,touchZero,0.5f);
				Vector2 deltaMiddle = prevMiddle-currMiddle;
				Camera.main.transform.Translate(deltaMiddle);
				touchZero = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
				touchOne = Camera.main.ScreenToWorldPoint(Input.touches[1].position);
				prevMiddle=Vector2.Lerp(touchOne,touchZero,0.5f);
			}
		}

	}

	void zoomCamera(){
		if(Input.touchCount>=2){
			if(fingerCount<=1){
				oldZoom=Camera.main.orthographicSize;
				zoomStart=(Input.touches[0].position-Input.touches[1].position).magnitude;
			}
			float currPinchDist = (Input.touches[0].position-Input.touches[1].position).magnitude;
			Camera.main.orthographicSize=oldZoom*zoomStart/currPinchDist;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize,2.5f,10f);
		}else if(Input.touchCount-fingerCount==1){

		}
		fingerCount=Input.touchCount;
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
