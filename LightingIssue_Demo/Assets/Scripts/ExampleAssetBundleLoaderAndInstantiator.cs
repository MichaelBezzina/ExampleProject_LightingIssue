using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ExampleAssetBundleLoaderAndInstantiator : MonoBehaviour
{
    [HideInInspector] public List<AssetBundle> downloadedAssetBundleList = new List<AssetBundle>();

    private string pathToAssetBundle;

    private void OnEnable()
    {
        SetupAssetBundlesFromServer();
    }

    private void SetupAssetBundlesFromServer()
    {
        pathToAssetBundle = Application.streamingAssetsPath + "/AssetBundles/Editor/examplebundle";

        StartCoroutine(GetAssetBundle(pathToAssetBundle));
    }

    IEnumerator GetAssetBundle(string path)
    {
        using (UnityWebRequest bundle = UnityWebRequestAssetBundle.GetAssetBundle(path))
        {
            var request = bundle.SendWebRequest();
            while (!request.isDone)
                yield return null;

            AssetBundle loadedAssetBundle = DownloadHandlerAssetBundle.GetContent(bundle);

            if (!downloadedAssetBundleList.Contains(loadedAssetBundle)) // If Asset Bundle has not already been added to our Asset Bundle list... add it
            {
                Debug.Log("Asset Bundle Added");
                downloadedAssetBundleList.Add(loadedAssetBundle);

                GetPrefabFromAssetBundle("apartment");
            }
        }
    }

    public GameObject GetPrefabFromAssetBundle(string prefabName) // Called from SceneLoader & SpawnSystem_Items
    {
        GameObject loadedPrefab = null;
        prefabName = prefabName.ToLower();
        bool foundPrefab = false;
        string itemName;

        for (int i = 0; i < downloadedAssetBundleList.Count; i++)
        {
            foreach (string itemPath in downloadedAssetBundleList[i].GetAllAssetNames())
            {
                itemName = Path.GetFileNameWithoutExtension(itemPath).ToLower();

                if (prefabName.Equals(itemName))
                {
                    loadedPrefab = downloadedAssetBundleList[i].LoadAsset(prefabName) as GameObject;
                    foundPrefab = true;

                    InstantiateGameObject(loadedPrefab);
                    break;
                }
            }

            if (foundPrefab)
                break;
        }

        return loadedPrefab;
    }

    private void InstantiateGameObject(GameObject objectToInstantiate)
    {
        Instantiate(objectToInstantiate, this.transform);
    }
}
