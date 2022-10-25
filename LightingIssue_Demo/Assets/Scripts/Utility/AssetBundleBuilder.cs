using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleBuilder : Editor
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        /*BuildPipeline.BuildAssetBundles(Application.dataPath + "/AssetBundles", 
        BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);*/

        string folderName = "AssetBundles/WebGL";
        string filePath = Path.Combine(Application.streamingAssetsPath, folderName);

        //Build for WebGL platform
        BuildPipeline.BuildAssetBundles(filePath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);

        //Refresh the Project folder
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Build AssetBundles For Editor")]
    static void BuildAllAssetBundlesForEditor()
    {
        string folderName = "AssetBundles/Editor";
        string filePath = Path.Combine(Application.streamingAssetsPath, folderName);

        //Build for Windows platform - Compatible with in-editor testing, unlike WebGL
        BuildPipeline.BuildAssetBundles(filePath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);

        //Refresh the Project folder
        AssetDatabase.Refresh();
    }
}
