using UnityEngine;
using System.Collections;

public class Parrot: Bird {
	public virtual new void Start(){
		base.Start();
		baseColor=Color.red;
		GetComponent<SpriteRenderer>().material.color=baseColor;
	}

	public virtual new void Update(){
		base.Update();
	}
}