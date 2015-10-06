using UnityEngine;
using System.Collections;

public class LogoController : MonoBehaviour {

	public LensFlare _light;
	public PlayMakerFSM _player;

	// Use this for initialization
	void Start () {
		Cardboard.SDK.OnTrigger += Launch;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Launch(){
		_player.SendEvent("Start");
	}

	public void StartFade(){
		iTween.ValueTo(gameObject, iTween.Hash(
			"time", 5.0f,
			"from", 0.0f,
			"to", 2.8f,
			"onupdate", "FadeUpd",
			"oncompletetarget", gameObject,
			"oncomplete", "FadeEnd"
			));
	}

	void FadeUpd(float _v){
		_light.brightness = _v;
	}

	void FadeEnd(){
		_player.SendEvent("FINISHED");
	}
}
