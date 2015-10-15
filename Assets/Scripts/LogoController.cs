using UnityEngine;
using System.Collections;

public class LogoController : MonoBehaviour {

	public LensFlare _light;
	public AudioClip _launchSound = null;
	public GameObject _loderLabel;
	public GameObject[] _components;

	public string levelName;
	AsyncOperation async;
	
	void Start () {
		Cardboard.SDK.OnTrigger += Launch;
		//StartLoading();
	}
	
	void Update () {
	
	}

	public void StartLoading() {
		StartCoroutine("load");
	}
	
	IEnumerator load() {
		Debug.LogWarning("ASYNC LOAD STARTED - " +
		                 "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
		async = Application.LoadLevelAsync(levelName);
		async.allowSceneActivation = false;
		yield return async;
		Debug.Log("Loaded space");
		ActivateScene();
	}
	
	public void ActivateScene() {
		async.allowSceneActivation = true;
	}

//	IEnumerator Start() {
//		AsyncOperation async = Application.LoadLevelAsync("MainScene");
//		yield return async;
//
//		Debug.Log("Loading complete");
//	}

	void Launch(){
		StartFade();
		AudioSource.PlayClipAtPoint(_launchSound, Vector3.zero);
	}

	public void StartFade(){
		Cardboard.SDK.OnTrigger -= Launch;

		iTween.ValueTo(gameObject, iTween.Hash(
			"time", 1.0f,
			"from", 0.0f,
			"to", 10.8f,
			"onupdate", "FadeUpd",
			"oncompletetarget", gameObject,
			"oncomplete", "FadeEnd"
			));
	}

	void FadeUpd(float _v){
		_light.brightness = _v;
	}

	void FadeEnd(){
		foreach(GameObject _obj in _components){
			_obj.SetActive(false);
		}

		_loderLabel.SetActive(true);
		Application.LoadLevel("MainRoom");
	}
}
