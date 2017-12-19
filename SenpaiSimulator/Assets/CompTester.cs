using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompTester : MonoBehaviour {

	
	
	// Use this for initialization
	void Start () {
		Text text = gameObject.AddComponent(typeof(Text)) as Text;
		
		text.text = "Hey, good job, pal!";
	
	}



	
	
}
