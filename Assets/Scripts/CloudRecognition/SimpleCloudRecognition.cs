using UnityEngine;
using Vuforia;
using JetBrains.Annotations;
using static UnityEngine.CullingGroup;
using static Vuforia.CloudRecoBehaviour;
using System;

public class MetaDatos {

    public string nombre;
    public string url;
    public string otro;

    public static MetaDatos CreateFromJSON(string jsonString) {

        return JsonUtility.FromJson<MetaDatos>(jsonString);
    }
}
public class SimpleCloudRecognition : MonoBehaviour {

    CloudRecoBehaviour mCloudRecoBehaviour;
    bool mIsScanning = false;
    string mTargetMetadata = "";

    public ImageTargetBehaviour ImageTargetTemplate;

    void Awake() {
        
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);

    }
    void OnDestroy() {

        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    public void OnInitialized(CloudRecoBehaviour cloudRecoBehaviour) {
        Debug.Log("Cloud Reco initialized");
    }
    public void OnInitError(CloudRecoBehaviour.InitError initError) {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }
    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError) {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }
    public void OnStateChanged(bool scanning) {

        mIsScanning = scanning;

        if (scanning) {
            // Clear all known targets
        }
    }
    void OnGUI() {
        // Display current 'scanning' status
        GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
        // Display metadata of latest detected cloud-target
        GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);
        // If not scanning, show button
        // so that user can restart cloud scanning
        if (!mIsScanning) {
            if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning")) {
                // Reset Behaviour
                mCloudRecoBehaviour.enabled = true;
                mTargetMetadata = "";
            }
        }
    }
    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult) {

        MetaDatos datos;
        datos = MetaDatos.CreateFromJSON(cloudRecoSearchResult.MetaData);

        mTargetMetadata = datos.nombre;

        mCloudRecoBehaviour.enabled = false;   

        if (ImageTargetTemplate){
            mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);
        }
    }
}