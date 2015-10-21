using UnityEngine;
using System.Collections;

public class UnityEx : MonoBehaviour
{
	IEnumerator Start ()
	{
		var www = WWW.LoadFromCacheOrDownload("http://mixarine.com/ftp/spheres_assets/spheres.unity3d", 2);
		yield return www;
		if(!string.IsNullOrEmpty(www.error))
		{
			Debug.Log(www.error);
			//return;
		}

		AssetBundle myLoadedAssetBundle = www.assetBundle;
		Texture masset = myLoadedAssetBundle.mainAsset as Texture;
		Debug.Log(masset);
	}
}