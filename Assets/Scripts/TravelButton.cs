using UnityEngine;
using System.Collections;

public class TravelButton : MonoBehaviour {
	
	public SpaceObj _nextRoom;
	int git;

	public void Go() {
		GameObject.Find("Manager").GetComponent<Manager>().OpenSpace(_nextRoom);
	}
}
