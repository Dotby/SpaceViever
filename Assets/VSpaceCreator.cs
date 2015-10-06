using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InControl;
using UnityEngine.UI;


public class VSpaceCreator : MonoBehaviour {

	public List<Box> _boxes = new List<Box>();
	public Material _boxMat;
	public Material _boxSelected;

	public GameObject _boxPref;

	public Box _activeBox = null;
	public bool _waitFree = false;

	public Text _modeText;

	int _selectedBox = 0;

	string[] _helps = new string[]{
		"Mode 0: выбор куба",
		"Mode 1: движение выбранного куба по осям X/Y",
		"Mode 2: масштаб выбранного куба",
		"Mode 3: движение выбранного куба по оси Z",
		"Mode 4: вверх - создать куб, вниз - удалить выбранный"
	};

	//0 - noon, 1 - scale, 2 - move XY, 3 - move Z
	public int _mode = 0;

	void OnEnable(){
		//InputManager.Devices[0].AnyButton.WasRepeated += inputDevice => _waitFree = false;
		InputManager.OnDeviceAttached += inputDevice => Debug.Log( "Attached: " + inputDevice.Name );
	}

	void Start () {
		var unityDeviceManager = InputManager.GetDeviceManager<UnityInputDeviceManager>();
		unityDeviceManager.ReloadDevices();
	}

	void Update () {
		#if UNITY_ANDROID && INCONTROL_OUYA && !UNITY_EDITOR
		var inputDevice = InputManager.ActiveDevice;
		Debug.Log( "[InControl] " + inputDevice.LeftStick.Vector );
		var anyButton = inputDevice.AnyButton;
		if (anyButton)
		{
			Debug.Log( "[InControl] AnyButton = " + anyButton.Handle );
		}
		#endif
		
		foreach (var inputDevice in InputManager.Devices)
		{
			bool active = InputManager.ActiveDevice == inputDevice;
			
			var anyButton = inputDevice.AnyButton;

			if (anyButton && _waitFree == false)
			{
				_waitFree = true;
				Debug.Log("AnyButton = " + anyButton.Handle);

				//left but
				if (anyButton.Handle == "Button 0"){
						if (_mode == 4){
						Debug.Log("Four try///   0");
							//FourModeBut(0);
							_mode = 0;
						}else if(_mode == 0){
							_mode = 4;
						}else{
						_mode = 0;
					}
				}

				if (anyButton.Handle == "Button 1"){
						if (_mode == 4){
						Debug.Log("Four try///    1");
							FourModeBut(1);
						}else{
							_mode = 1;
						}
				}

				if (anyButton.Handle == "Button 2"){
						if (_mode == 4){
							FourModeBut(2);
						}else{
							_mode = 2;
						}
				}

				if (anyButton.Handle == "Button 3"){
					if (_mode == 4){
						FourModeBut(3);
					}else{
						_mode = 3;
					}
				}

//				if (anyButton.Handle == "Button 8"){
//					if (_mode != 4){
//						_mode = 4;
//					}else{
//						_mode = 0;
//					}
//					//Выбор удаление создание
//				}

				_modeText.text = _helps[_mode] + "\nКубы: " + _boxes.Count;
			}

			if (anyButton.LastValue != anyButton.Value && _waitFree == true){
				_waitFree = false;
			}
			
			var anyOs = inputDevice.Direction;
			
			//Debug.Log(anyOs.Vector.normalized.ToString());

			switch(_mode){
				case 0: ZeroModeAction(anyOs.Vector.normalized); break;
				case 1: if(_activeBox) Move(anyOs.Vector.normalized); break;
				case 2: if(_activeBox) Scale(anyOs.Vector.normalized); break;
				case 3: if(_activeBox) MoveZ(anyOs.Vector.normalized); break;
				default: break;
			}
		}
	}

	void FourModeBut(int _but){
		//if (_waitFree == true) {_waitFree = false; return;}
		Debug.Log("Four action " + _but);
		switch(_but){
			case 1: 
					if (_activeBox){
						_activeBox.GetComponent<Renderer>().sharedMaterial = _boxMat;
					}

					_activeBox = null;
					Debug.Log("New");

				//down but
					CreateNew();
					_waitFree = true;
			break;

			
			case 0: 
//				if (_activeBox != null){
//					Debug.Log("Created");
//					_activeBox = null;
//					_waitFree = true;
//				}
			break;


			//up but
			case 2:
				if (_activeBox != null){
					Debug.Log("Deleted");
					Cancel();
					_waitFree = true;
				}
			break;


			case 3:
//				if (_activeBox != null){
//					Debug.Log("Created");
//					//_activeBox = null;
//					_waitFree = true;
//				}
			break;

			default: break;
		}
	}

	public void	ZeroModeAction(Vector2 _vec){

			//if (_waitFree == true) {_waitFree = false; return;}

			if (_vec == Vector2.zero) {return;}
			//Debug.Log("Zero" + _vec);

			if (_vec.x < 0 && _boxes.Count > 0){
				Debug.Log("delect next");
				SelectNext();
			}
	}

	public void SelectNext(){

		if (_boxes.Count == 0) {return;}
			
		if (_activeBox){
			_activeBox.GetComponent<Renderer>().sharedMaterial = _boxMat;
			_activeBox = null;
		}

			if (_selectedBox == (Math.Max(0, _boxes.Count - 1))){
				_selectedBox = 0;
			}else{
				_selectedBox ++;
			}

			_activeBox = _boxes[_selectedBox];
			_activeBox.GetComponent<Renderer>().sharedMaterial = _boxSelected;
	}

	public void Move(Vector2 _vec){
		_activeBox.transform.Translate(new Vector3(_vec.y * 0.01f, -_vec.x * 0.01f, 0f));
	}

	public void MoveZ(Vector2 _vec){
		_activeBox.transform.Translate(new Vector3(0.0f, 0.0f, -_vec.x * 0.01f));
	}

	public void Scale(Vector2 _vec){
		//_activeBox.transform.localRotation = Quaternion.Euler();

		if (_vec.x == 0) {return;}

		Debug.Log(_vec);

		float _scale = 1.0f;
		Vector3 _scaleOld = _activeBox.transform.localScale;

		if (_vec.x < 0f){
			_scale = 1.01f;
		}

		if (_vec.x > 0f){
			_scale = 0.99f;
		}

		_scaleOld = _scaleOld * _scale;

			if (_scaleOld.x < 0.1f) {_scaleOld = new Vector3(0.1f, 0.1f, 0.1f);}
		
		_activeBox.transform.localScale = _scaleOld;
	}
	
	public void Cancel(){
		_boxes.Remove(_activeBox);
		DestroyImmediate(_activeBox.gameObject);
		_activeBox = null;
		_selectedBox --;
		SelectNext();
	}

	void FlipNormals(GameObject _box){

		Mesh mesh = _box.GetComponent<MeshFilter>().mesh;

		int[] triangles = mesh.triangles;
		int numpolies = triangles.Length / 3;

		Debug.Log(numpolies);
		
		for(int t = 0; t < numpolies; t++)
		{
			int tribuffer = triangles[t * 3];
			triangles[t * 3] = triangles[(t * 3) + 2];
			triangles[(t * 3) + 2 ] = tribuffer;
		}
		
		mesh.triangles = triangles;
	}
	
	public void CreateNew(){
		GameObject _tmp = (GameObject)Instantiate(_boxPref, Vector3.zero, Quaternion.Euler(Vector3.zero));
		_tmp.transform.SetParent(gameObject.transform);
		Box _box = _tmp.AddComponent<Box>();
		_tmp.transform.localPosition = Vector3.zero;
		_tmp.GetComponent<Renderer>().sharedMaterial = _boxSelected;
		FlipNormals(_tmp);
		_boxes.Add(_box);
		_selectedBox = _boxes.Count - 1;
		_activeBox = _box;
	}
}
