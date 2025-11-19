using UnityEngine;
using UnityEngine.Networking;
using GLTFast;

public class LoadModelFromURL : MonoBehaviour
{

    public GameObject parentObject;

    public async void LoadModel(string url)
    {

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {

            var op = request.SendWebRequest();
            while (!op.isDone)
            {
                await System.Threading.Tasks.Task.Yield();
            }
            if (request.result == UnityWebRequest.Result.Success)
            {
                var gltf = new GltfImport();
                // Convertir MemoryStream a byte[]
                byte[] data = request.downloadHandler.data;
                bool success = await gltf.Load(data, new System.Uri(url));
                if (success)
                {
                    bool instantiated = await gltf.InstantiateMainSceneAsync(parentObject.transform);
                    if (instantiated)
                    {
                        foreach (Transform child in parentObject.transform)
                        {
                            if (child != null)
                                child.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                        }
                        AssignURPShader(parentObject.transform);
                    }
                    else
                    {
                        Debug.LogError("Error al instanciar el modelo GLTF");
                    }
                }
                else
                {
                    Debug.LogError("Error al cargar el modelo GLTF");
                }
            }
            else
            {
                Debug.LogError("Error al descargar el modelo: " + request.error);
            }
        }
    }
    private void AssignURPShader(Transform parent)
    {
        foreach (Transform child in parent)
        {
            var renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (Material mat in renderer.materials)
                {
                    if (mat.shader.name != "Universal Render Pipeline/Lit")
                    {
                        mat.shader = Shader.Find("Universal Render Pipeline/Lit");
                        if (mat.HasProperty("_BaseMap"))
                        {
                            // mat.SetTexture("_BaseMap", tuTexturaAlbedo);
                        }
                    }
                }
            }
        }
    }
}