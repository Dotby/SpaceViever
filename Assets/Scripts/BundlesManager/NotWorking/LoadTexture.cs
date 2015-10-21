using UnityEngine;
using System.Collections;

public class LoadTexture : MonoBehaviour {
	public GameObject go;
	public string web;
	
	private IEnumerator loadImage( GameObject page, string url ) {
		WWW www = new WWW( url );
		yield return www;
		page.GetComponent<Renderer>().material.mainTexture = www.texture;
	}
	
	void OnGUI() {
		if ( GUILayout.Button( "Go!" ) ) {
			StartCoroutine( loadImage( go, web ) );
		}
	}
}