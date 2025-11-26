using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadModelFromAssetsBundle : MonoBehaviour {

    private string prefabBundleUrl;
    private string prefabsBundleName;
    private uint a;
    private Hash128 b;

    void Start() {

        StartCoroutine(FetchGameObjectFromServer(prefabBundleUrl, prefabsBundleName, a, b));
    }
    IEnumerator FetchGameObjectFromServer(string url, string manifestFileName, uint crcR, Hash128 hashR) {

        //Get from generated manifest file of assetbundle.
        uint crcNumber = crcR;
        //Get from generated manifest file of assetbundle.
        Hash128 hashCode = hashR;
        UnityWebRequest webrequest = UnityWebRequestAssetBundle.GetAssetBundle(url, new CachedAssetBundle(manifestFileName, hashCode), crcNumber);

        webrequest.SendWebRequest();

        while (!webrequest.isDone) {
            Debug.Log(webrequest.downloadProgress);
        }

        AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(webrequest);
        yield return assetBundle;
        if (assetBundle == null)
            yield break;

        //Gets name of all the assets in that assetBundle.
        string[] allAssetNames = assetBundle.GetAllAssetNames();
        Debug.Log(allAssetNames.Length + "objects inside prefab bundle");
        foreach (string gameObjectsName in allAssetNames) {

            string gameObjectName = Path.GetFileNameWithoutExtension(gameObjectsName).ToString();
            GameObject objectFound = assetBundle.LoadAsset(gameObjectName) as GameObject;
            Instantiate(objectFound);
        }
        assetBundle.Unload(false);
        yield return null;
    }
}