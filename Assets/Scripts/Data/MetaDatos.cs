using UnityEngine;

public class MetaDatos : MonoBehaviour {

        public string nombre;
        public string serie;
        public string URL;
        public static MetaDatos CreateFromJSON(string jsonString) {

            return JsonUtility.FromJson<MetaDatos>(jsonString);
        }
}