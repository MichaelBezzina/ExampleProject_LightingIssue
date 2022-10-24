using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;

public class InitializationManager : MonoBehaviour
{
    public static InitializationManager Instance { get; private set; }

    // VARIABLES
    [SerializeField] private bool loadCoreAssetBundleAsAScene;

    [HideInInspector] public AssetBundle coreAssetBundle;
    private GameObject coreAssetBundlePrefab;
    [HideInInspector] public GameObject coreAssetBundlePrefabInstance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SetupCoreAssetBundle();
    }

    private void SetupCoreAssetBundle()
    {
        string coreABPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles/Editor/core_example");

        StartCoroutine(GetCoreAssetBundle(coreABPath));
    }

    IEnumerator GetCoreAssetBundle(string path)
    {
        using (UnityWebRequest bundle = UnityWebRequestAssetBundle.GetAssetBundle(path))
        {
            yield return bundle.SendWebRequest();

            if (bundle.isNetworkError || bundle.isHttpError)
                Debug.Log("Error in GetBundle: " + bundle.error);
            else
            {
                coreAssetBundle = DownloadHandlerAssetBundle.GetContent(bundle);
                UnpackAssetBundle(coreAssetBundle);
            }
        }

        yield return null;
    }

    public void UnpackAssetBundle(AssetBundle ab)
    {
        if (loadCoreAssetBundleAsAScene)
        {
            string[] scenePath = ab.GetAllScenePaths();

            for (int i = 0; i < scenePath.Length; i++)
                Debug.Log("ScenePath = " + scenePath[i].ToString());

            SceneManager.LoadScene(scenePath[0]);
        }
        else if (coreAssetBundlePrefab == null)
        {
            foreach (string goName in ab.GetAllAssetNames())
            {
                //Debug.Log(goName);
                if (goName != null)
                {
                    /*string[] scenePath = ab.GetAllScenePaths();

                    for(int i = 0; i < scenePath.Length; i++)
                        Debug.Log(scenePath[i].ToString());

                    SceneManager.LoadScene(scenePath[0]);
                    */
                    coreAssetBundlePrefab = ab.LoadAsset(goName) as GameObject;
                    break;
                }
            }

            if (coreAssetBundlePrefab != null)
                coreAssetBundlePrefabInstance = Instantiate(coreAssetBundlePrefab, this.transform);
            else
                Debug.LogError("Failed to instantiate the core asset bundle.");
        }
    }
}
