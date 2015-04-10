using UnityEngine;
using System.Collections;

public class Duck : Bird {

	public virtual new void Start(){
		base.Start();
		GetComponent<Animator>().Play("flyingDuck");
		baseColor=Color.white;
		GetComponent<SpriteRenderer>().material.color=baseColor;
	}
	
	public virtual new void Update(){
		base.Update();
	}
}
