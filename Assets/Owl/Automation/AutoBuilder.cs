#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;


public static class AutoBuilder
{
	public static void BuildAssetBundlesOSX()
	{
		BuildPipeline.BuildAssetBundles("AssetBundles/OSX", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);
		//BuildPipeline.BuildAssetBundles("AssetBundles");
	}

	public static void BuildAssetBundlesWin64()
	{
		//BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);	
		BuildPipeline.BuildAssetBundles("AssetBundles/Windows", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
	}
}

#endif
