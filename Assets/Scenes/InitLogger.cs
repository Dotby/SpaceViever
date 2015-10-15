using UnityEngine;
using System.Collections;

public class InitLogger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("Go", 1.0f);
	}

	void Go(){
		Application.LoadLevel(1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
