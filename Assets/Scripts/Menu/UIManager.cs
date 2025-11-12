using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject panelMenu;
    public GameObject panelModo;

    // Llamar al iniciar para mostrar solo el menú principal
    void Start() {

        MostrarMenuPrincipal();
    }
    public void MostrarMenuPrincipal() {

        panelMenu.SetActive(true);
        panelModo.SetActive(false);
    }
    public void MostrarSeleccionModo() {

        panelMenu.SetActive(false);
        panelModo.SetActive(true);
    }
    public void ModoSimple() {

        SceneManager.LoadScene("JuegoSimple");
    }
    public void ModoParejas() {

        SceneManager.LoadScene("JuegoParejas");
    }

    public void Salir() {

        Application.Quit();
    }
}