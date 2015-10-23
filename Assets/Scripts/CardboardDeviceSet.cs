using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardboardDeviceSet : MonoBehaviour {

	int _keyvar = 0;
	public AnimationClip[] _anims;

	public GameObject _cardboardTriggerIcon;

	// Use this for initialization
	void Start () {

		//_anims = GetAnimationNames();

//		if (RuntimePlatform.Android){
//			Debug.Log("Run in android.");
//
//			//Cardboard.SDK.ScreenSize = CardboardProfile.ScreenSizes.iPhone5;
//		}
	}

	void Update () {
	
	}

//	AnimationClip[] GetAnimationNames(){
//
//		//AnimationClip[] _clips = ac.animationClips;
//
//		for (int i = 0; i < _clips.Length; i++) {
//			Debug.Log(string.Format("State: {0}", _clips[i].name));
//		}
//
//		return _clips;
//	}

	bool Int2Bool (int _num) {

		if (_num < 0 || _num > 1) {return false;}
		return _num == 0 ? false : true;
	}

	public void SetCardboardMode (bool _vrbox){
	
		_keyvar = _vrbox ? 1 : 0;
		PlayerPrefs.SetInt("cardboard_enabled", _keyvar);

		if (_vrbox == false){
			_cardboardTriggerIcon.SetActive(false);
			GetComponent<Animator>().Play("Mode360Select");
			Invoke("SetMode", _anims[1].length);
		}else{
			GetComponent<Animator>().Play("ModeVRSelect");
			Invoke("SetMode", _anims[2].length);
		}
	}

	void SetMode() {

		Cardboard.SDK.VRModeEnabled = Int2Bool(_keyvar);

		gameObject.SetActive(false);
	}
}
