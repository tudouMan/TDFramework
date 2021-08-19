using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AssetEditor :Editor
{
    [MenuItem("Tools/BuildAsset")]
    public static void Build()
    {
      
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/Bundle/", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
