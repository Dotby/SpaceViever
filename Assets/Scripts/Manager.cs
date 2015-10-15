using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	public SpaceObj[] _spaces;
	public SpaceObj _startSpace = null;
	public SpaceObj _activeSpace = null;
	AudioSource _snd;
	public MouseCameraControl _mouseControl;

	public Text _spaceInfo;

	void Start () {

		Debug.Log("StartManager");

#if !UNITY_EDITOR
		if (_mouseControl != null){
			_mouseControl.enabled = false;
		}
#endif

		_snd = gameObject.AddComponent<AudioSource>();
		_snd.playOnAwake = false;
		_snd.loop = true;

		foreach(SpaceObj _space in _spaces){
			_space.gameObject.SetActive(false);
		}

		if (_startSpace == null){
			if (_spaces.Length != 0){
				OpenSpace(_spaces[0]);
			}
			else {
				Debug.Log("No spaces in array!");
			}
		}
		else {
			OpenSpace(_startSpace);
		}
		
		Debug.Log("Loaded");
	}

//	void OnLevelWasLoaded(int level) {
//		if (Application.loadedLevelName == "MainRoom"){
//
//
//	}

	public void OpenSpace(SpaceObj _space){

		if (_space == null) return;

		CloseSpace();

		_snd.clip = _space._sound;
		_snd.Play();
		_spaceInfo.text = _space._name + "\n" + _space._info;

		_activeSpace = _space;
		_activeSpace.gameObject.SetActive(true);

		Debug.Log("OpenSpace");
	}

	void CloseSpace(){

		Debug.Log("CloseOldSpace");

		if (_activeSpace != null){
			_activeSpace.gameObject.SetActive(false);
			_activeSpace = null;
		}

		_snd.Stop();
	}

	void Update () {
	
	}

	void OnGUI(){

	}
}
