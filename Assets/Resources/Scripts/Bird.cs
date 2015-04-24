using UnityEngine;
using System.Collections;
using System;


public class Bird : MonoBehaviour {
	[System.Serializable]
	public class Stats{
		public string birdName = "Slinky";
		public float stamina = 100;
		public string type = "Parrot";
		public float health = 100;
		public float maxAltitude = 50;
		public float maxSpeed = 10;
		public bool canFreeFly = true;
		public float[] baseColor = {1,1,1};
	}
	public Stats stats= new Stats();
	public string[] names = {
		"Slinky","Alex","Saff",
		"Slade","Phil","Doodles",
		"William","Tiger","Scruff",
		"Bleep Bloop","Clam","720p",
		"Trinket","Chicken Tikka",
		"Aerosol","Pastrami","Gey",
		"Bumpkin","Urfcake","Cornhole",
		"Fajita","Walter","Jesus",
		"Envelope","Sashimi","Goggles",
		"Einstein","Suzie"
	};
	public Color[] colors = {
		Color.blue,Color.cyan,
		Color.gray,Color.green,Color.magenta,
		Color.red,Color.yellow,Color.white
	};
}
