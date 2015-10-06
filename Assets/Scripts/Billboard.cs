using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
	public Transform _goal;
	public Transform _head = null;
	public float addRot = 0f;
	public Canvas _canv;

	public float _minDistToGoal;
	public float _maxDistToGoal;
	public float _alpha = 0f;
	public float _onePerc = 0f;
	public float _koef = 0f;

	void Awake(){
		_canv = transform.parent.GetComponentInChildren<Canvas>();

		_minDistToGoal = Vector3.Distance(transform.parent.transform.position, _goal.transform.position);
		_maxDistToGoal = _minDistToGoal;
	}

	void HideCanvas(){
		Image[] _imgs = _canv.GetComponentsInChildren<Image>();
		Text[] _txts = _canv.GetComponentsInChildren<Text>();
		Renderer[] _rend = GetComponentsInChildren<Renderer>();

		foreach(Renderer _rn in _rend){
			_rn.material.color = new Color(0f, 0f, 0f, 0f);
		}

		foreach(Image _img in _imgs){
			_img.CrossFadeAlpha(0f, 1f, true);
		}

		foreach(Text _txt in _txts){
			_txt.CrossFadeAlpha(0f, 1f, true);
		}
	}

	void ShowCanvas(){
		Image[] _imgs = _canv.GetComponentsInChildren<Image>();
		Text[] _txts = _canv.GetComponentsInChildren<Text>();
		
		foreach(Image _img in _imgs){
			_img.CrossFadeAlpha(1f, 1f, true);
		}

		foreach(Text _txt in _txts){
			_txt.CrossFadeAlpha(1f, 1f, true);
		}

		Renderer[] _rend = GetComponentsInChildren<Renderer>();
		
		foreach(Renderer _rn in _rend){
			_rn.material.color = new Color(0f, 0f, 0f, 1f);
		}
	}
	
	void Update()
	{
		float _d = Vector3.Distance(transform.parent.transform.position, _goal.transform.position);
		if (_d > _maxDistToGoal){
			_maxDistToGoal = _d;
			_onePerc = _maxDistToGoal / 100.0f;
			//_koef = 1.0f / _maxDistToGoal;
		}

		_alpha = (_d / _onePerc);
		Debug.Log(_alpha);
		
		Vector3 v = _goal.transform.position - transform.position;
		v.x = v.z = 0.0f;
		v.y += addRot;
		transform.LookAt(_goal.transform.position - v);

		if (_head != null){
			if (Mathf.Abs(_head.localRotation.eulerAngles.y) > 330.0f || Mathf.Abs(_head.localRotation.eulerAngles.y) < 30.0f){
				HideCanvas();
			}else{
				ShowCanvas();
			}

		}
	}
}