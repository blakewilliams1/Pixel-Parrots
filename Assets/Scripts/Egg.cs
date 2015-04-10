using UnityEngine;
using System.Collections;
using System;

public class Egg : MonoBehaviour{
	Color baseColor = Color.white;
	DateTime endTime;

	void Start(){
		endTime=System.DateTime.UtcNow;
		//somewhere we need to add the delay
	}

	void Update(){
		if(System.DateTime.UtcNow>endTime){
			//hatch
		}
	}
}
