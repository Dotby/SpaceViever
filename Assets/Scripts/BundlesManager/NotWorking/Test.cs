//using UnityEditor;
using UnityEngine;
using System.Collections;

class Test : MonoBehaviour {
	public string url;
	public int version;
	AssetBundle bundle = null;

	void OnGUI (){
		if (GUILayout.Button ("Download bundle")){
			bundle = AssetBundleManager.getAssetBundle (url, version);
			if(!bundle)
				StartCoroutine (DownloadAB());
		}
	}

	IEnumerator DownloadAB (){
		Debug.Log("downloading...");
			yield return StartCoroutine(AssetBundleManager.downloadAssetBundle (url, version));
			bundle = AssetBundleManager.getAssetBundle (url, version);
			Debug.Log("complete");
			Debug.Log(bundle);
	}

	void Update(){
		if (bundle != null){
			Debug.Log(bundle.name);
		}
	}

	void OnDisable (){
		AssetBundleManager.Unload(url, version, true);
	}
}