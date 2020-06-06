using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildBundle : MonoBehaviour
{
    [MenuItem("Tools/BuildBundleWithTypeTree")]
    public static void BuildBundleWithTypeTree()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression,
            BuildTarget.StandaloneWindows64);
    }
    
    [MenuItem("Tools/BuildBundleWithOutTypeTree")]
    public static void BuildBundleWithOutTypeTree()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath,
            BuildAssetBundleOptions.DisableWriteTypeTree | BuildAssetBundleOptions.ChunkBasedCompression,
            BuildTarget.StandaloneWindows64);
    }
    
    [MenuItem("Tools/BuildBundleWithOutExtraNames")]
    public static void BuildBundleWithOutExtraNames()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath,
            BuildAssetBundleOptions.DisableWriteTypeTree | BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DisableLoadAssetByFileName,
            BuildTarget.StandaloneWindows64);
    }
}
