using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LoadBundle : MonoBehaviour
{
	WWW www;
	AssetBundleRequest request;

	public Text _loadText;
	public string url = "http://mixarine.com/ftp/spheres_assets/newbundle";

	int _loadStatus = 0;

	public Texture _loadedTex = new Texture();
	public Renderer _currentSpace;
	public Texture _defTex;

	public Manager _manager;

	public LogoController _logo = null;

	public Image _camFader;
	float _blackFade = 1.0f;
	float _alphaFade = 0.0f;


//	void Awake(){
//	
////		#if   UNITY_ANDROID && !UNITY_EDITOR
////		url += ".android.unity3d";
////		#elif UNITY_IPHONE  && !UNITY_EDITOR
////		url += ".iphone.unity3d";
////		#else
////		url += ".unity3d";
////		#endif
////
////		Debug.Log("Url set to:" + url);
//	}

	void Start(){
		if (_camFader != null)
		_camFader.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}

	public void StartPreloading(LogoController _lo){
		_logo = _lo;
		_loadStatus = 3;
		StartCoroutine(PreloadBundle());
	}

	IEnumerator PreloadBundle(){

		#if   UNITY_ANDROID && !UNITY_EDITOR
		url += ".android.unity3d";
		#elif UNITY_IPHONE  && !UNITY_EDITOR
		url += ".ios.unity3d";
		#else
		url += ".ios.unity3d";
		#endif
		
		Debug.Log("Url set to:" + url);

		www = WWW.LoadFromCacheOrDownload(url, 2);
		//www = WWW.LoadFromCacheOrDownload(url, Hash128.Parse("52493182f0ca7528471e1f270211472b"));

		yield return www;
		
		if (www.isDone == true){
			_loadText.text = "";
		}
		
		if (www.error != null){
			_loadText.text = "WWW download had an error: " + www.error;
			throw new Exception("WWW download had an error: " + www.error);
		}

		// Frees the memory from the web stream
		www.Dispose();

		_loadStatus = 0;
		_logo.PreloadEnd();
	}

	void StartFade(string _name){
		iTween.ValueTo(gameObject, iTween.Hash("from", _alphaFade, 
		                                       "to", _blackFade,
		                                       "delay", 0.0f,
		                                       "time", 0.5f,
		                                       "onComplete","StartAfterFade",
		                                       "onCompleteTarget", gameObject,
		                                       "oncompleteparams", _name,
		                                       "onupdate", "SetFaderColor"
		                                      )
		               );
	}

	void StartFadeOut(){
		iTween.ValueTo(gameObject, iTween.Hash("from", _blackFade, 
		                                       "to", _alphaFade,
		                                       "delay", 0.0f,
		                                       "time", 0.5f,
		                                       "onupdate", "SetFaderColor"
		                                       )
		               );
	}

	void SetFaderColor(float _alp){
		if (_camFader != null)
		_camFader.color = new Color(0.0f, 0.0f, 0.0f, _alp);
	}

	public void StartLoading(string _name, Renderer _space){

		if (_space.material.mainTexture)
		{
			if (_space.material.mainTexture.name != _defTex.name) 
			{
				Debug.Log(_space.material.mainTexture.name + " was finded in app memory.");
				_currentSpace = _space;

				StartFade (_name);
				//iTween.CameraFadeAdd();
//				iTween.CameraFadeTo(iTween.Hash("amount", 1.0f, 
//				                                "delay", 0.0f,
//				                                "onComplete","StartAfterFade",
//				                                "onCompleteTarget", gameObject,
//				                                "oncompleteparams", _name)
//				                    );

				return;
			}

		}
	

		if (_name == "") {return;}

		_currentSpace = _space;

		StartFade (_name);

		//iTween.CameraFadeAdd();
//		iTween.CameraFadeTo(iTween.Hash("amount", 1.0f, 
//		                                "delay", 0.0f,
//		                                "onComplete","StartAfterFade",
//		                                "onCompleteTarget", gameObject,
//		                                "oncompleteparams", _name)
//		                    );


	}

	void StartAfterFade(string _name){
		StartCoroutine(LoadTexture(_name));
	}

	IEnumerator LoadTexture (string _name) {

		_loadText.text = "Загрузка пространства...";

		www = WWW.LoadFromCacheOrDownload(url, 2);
		//www = WWW.LoadFromCacheOrDownload(url, Hash128.Parse("52493182f0ca7528471e1f270211472b"));
		_loadStatus = 1;
		yield return www;

		if (www.isDone == true){
			_loadText.text = "";
		}

		if (www.error != null){
			_loadText.text = "WWW download had an error: " + www.error;
			throw new Exception("WWW download had an error: " + www.error);
		}

		_loadStatus = 0;

		AssetBundle bundle = www.assetBundle;

		string[] names = bundle.GetAllAssetNames();
		foreach(string nm in names){
			Debug.Log(nm);
		}

		request = bundle.LoadAssetAsync (_name + ".jpg", typeof(Texture));
		if (request.isDone == true)
		{
			Debug.Log("Found in memory.");
			_loadText.text = "";
		}
		else{
			_loadStatus = 2;
		}

		//AssetBundleRequest request = bundle.LoadAssetAsync (names[0], typeof(Texture));


		// Wait for completion
		yield return request;

		if (www.error != null){
			//_loadText.text = "WWW download had an error: " + www.error;
			throw new Exception("WWW download had an error: " + www.error);
		}

		_loadStatus = 0;
		_loadText.text = "";

		Texture tex = request.asset as Texture;
		//_loadText.text = "Loaded texture name: " + tex.name;
		//_texTest.GetComponent<Renderer>().material.mainTexture = tex;

		_currentSpace.material.mainTexture = tex;
		_manager.RoomLoaded(_currentSpace.GetComponent<SpaceObj>());
		//iTween.CameraFadeTo(0.0f, 1.0f);

		StartFadeOut();

		// Unload the AssetBundles compressed contents to conserve memory
		bundle.Unload(false);
		
		// Frees the memory from the web stream
		www.Dispose();
	}

	void Update () {
		if (_loadStatus == 1){ 
			int percent = (int)(www.progress * 100);
			_loadText.text = "Загрузка пространства: " + percent.ToString() + "%";
		}

		if (_loadStatus == 2){ 
			int percent = (int)(request.progress * 100);
			_loadText.text = "Загрузка пространства: " + percent.ToString() + "%";
		}

		if (_loadStatus == 3){ 
			int percent = (int)(www.progress * 100);
			_logo._preloadText.text = "Загрузка пространства\n" + percent.ToString() + "%";
		}
	}
}