using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
	[MenuItem ("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles ()
	{

//		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
//		AssetBundleBuild[] bd = new AssetBundleBuild[1];
//
//		bd[0].assetBundleName = "rooms";
//		string[] aNames = new string[selection.Length];
//
//		for (int i = 0; i < selection.Length; i++){
//			aNames[i] = selection[i].name;
//			Debug.Log(selection[i].name);
//		}
//
//		bd[0].assetNames = aNames;

//		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles", bd, BuildAssetBundleOptions.None, BuildTarget.Android);

		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles");
	}

	[MenuItem ("Assets/Bundle to Android")]
	static void GenerateAssetBundleAndroid ()
	{
		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
		string [ ] assetNames = new string [ selection. Length ] ;
		int i = 0 ;
		foreach ( UnityEngine. Object o in selection )
		{
			assetNames [ i ++ ] = AssetDatabase . GetAssetPath ( o ) ;
		}
		AssetBundleBuild [ ] buildMap = new AssetBundleBuild [ 1 ] ;
		buildMap [ 0 ] = new AssetBundleBuild ( ) ;
		buildMap [ 0 ] . assetBundleName = "AndroidBundle"; //mBundleId. ToString ( ) ;
		buildMap [ 0 ] . assetNames = assetNames;
		BuildPipeline . BuildAssetBundles ( "Assets/AssetsAndroid" , buildMap, BuildAssetBundleOptions. CollectDependencies | BuildAssetBundleOptions. CompleteAssets , BuildTarget.Android) ;
	}

	[MenuItem ("Assets/Bundle to IOS")]
	static void GenerateAssetBundleIOS ()
	{
		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
		string [ ] assetNames = new string [ selection. Length ] ;
		int i = 0 ;
		foreach ( UnityEngine. Object o in selection )
		{
			assetNames [ i ++ ] = AssetDatabase . GetAssetPath ( o ) ;
		}
		AssetBundleBuild [ ] buildMap = new AssetBundleBuild [ 1 ] ;
		buildMap [ 0 ] = new AssetBundleBuild ( ) ;
		buildMap [ 0 ] . assetBundleName = "IOSBundle"; //mBundleId. ToString ( ) ;
		buildMap [ 0 ] . assetNames = assetNames;
		BuildPipeline . BuildAssetBundles ( "Assets/AssetsIOS" , buildMap, BuildAssetBundleOptions. CollectDependencies | BuildAssetBundleOptions. CompleteAssets , BuildTarget.iOS) ;
	}
	
	[MenuItem ("Assets/Get AssetBundle names")]
	static void GetNames ()
	{
		var names = AssetDatabase.GetAllAssetBundleNames();
		foreach (var name in names)
			Debug.Log ("AssetBundle: " + name);
	}

//	[MenuItem("Assets/Build AssetBundle From Selection (for Android)")]
//	static void ExportAssetBundle() {
//		// Bring up save panel
//		string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
//		if (path.Length != 0) {
//			// Build the resource file from the active selection.
//			//
//			Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
//			//
//			BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);
//			Selection.objects = selection;
//			//
//		}
//	}
}