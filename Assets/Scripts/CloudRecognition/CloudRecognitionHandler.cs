using UnityEngine;
using Vuforia;

public class CloudRecognitionHandler : MonoBehaviour {

    private CloudRecoBehaviour cloudRecoBehaviour;

    void Start(){

        cloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

        if (cloudRecoBehaviour){

            cloudRecoBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }
    private void OnInitialized() {

        Debug.Log("Cloud Reco inicializado correctamente.");
    }
    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status){

        if (status.Status == Status.TRACKED) {

            Debug.Log("Target detectado: " + behaviour.TargetName);

            // Puedes instanciar contenido aquí
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(behaviour.transform);
            cube.transform.localPosition = Vector3.zero;
        }
    }
}