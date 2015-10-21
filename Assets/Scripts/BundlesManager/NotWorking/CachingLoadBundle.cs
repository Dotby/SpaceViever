using System;
using UnityEngine;
using System.Collections;

public class CachingLoadBundle : MonoBehaviour {
	public string BundleURL;
	public string AssetName;
	public int version;
	public string hashstring = "52493182f0ca7528471e1f270211472b";
	
	void Start() {
		StartCoroutine (DownloadAndCache());
	}
	
	IEnumerator DownloadAndCache (){
		// Wait for the Caching system to be ready
		while (!Caching.ready)
			yield return null;
		
		// Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
		using(WWW www = WWW.LoadFromCacheOrDownload (BundleURL, Hash128.Parse(hashstring))){
			yield return www;
			if (www.error != null)
				throw new Exception("WWW download had an error:" + www.error);
			AssetBundle bundle = www.assetBundle;

			Debug.Log(bundle.name);
			if (AssetName == "")
				Debug.Log(bundle.mainAsset);
			else
				Debug.Log(bundle.mainAsset);
				//Instantiate(bundle.LoadAsset(AssetName));
			// Unload the AssetBundles compressed contents to conserve memory
			bundle.Unload(false);
			
		} // memory is freed from the web stream (www.Dispose() gets called implicitly)
	}
}
