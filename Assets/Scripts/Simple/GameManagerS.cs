using System.Collections.Generic;
using UnityEngine;
using TMPro; // si usas TextMeshPro

public class GameManagerS : MonoBehaviour {

    [Header("Config juego")]
    public int vidasIniciales = 3;
    public int repeticionesPorSerie = 2; // cada serie aparece 2 veces
    public List<string> todasLasSeries = new List<string> {
        "princesa de hyrule",
        "nintendo wii",
        "windwaker",
        "princesa de reino champi",
        "nintendo 64",
        "hermanos fontaneros",
        "link guerrero"
    };

    [Header("UI")]
    public TMP_Text vidasText;
    public TMP_Text puntosText;
    public TMP_Text targetText;

    [Header("Referencias UI")]
    public GameObject panelVictoria;
    public GameObject panelError;

    int vidas;
    int puntos;
    List<string> colaSeries; // lista “barajada” con las series que deben ir saliendo

    int indiceActual = -1;
    bool juegoTerminado = false;

    void Start() {

        vidas = vidasIniciales;
        puntos = 0;

        // Construir la cola de series: cada serie repetida X veces y luego barajar
        colaSeries = new List<string>();
        foreach (var s in todasLasSeries) {
            for (int i = 0; i < repeticionesPorSerie; i++) {
                colaSeries.Add(s);
            }
        }
        // Barajar la lista para que el orden sea aleatorio
        BarajarLista(colaSeries); // ver método más abajo
        SiguienteObjetivo();
        ActualizarUI();
    }
    void ActualizarUI() {

        if (vidasText != null) vidasText.text = "vidas: "+vidas.ToString();
        if (puntosText != null) puntosText.text = "puntos: "+puntos.ToString();
        if (targetText != null) {
            string objetivo = indiceActual < colaSeries.Count ? colaSeries[indiceActual] : "-";
            targetText.text = "busca a: "+objetivo;
        }
    }
    void SiguienteObjetivo() {

        indiceActual++;
        if (indiceActual >= colaSeries.Count) {
            // No quedan más series: juego completo
            juegoTerminado = true;
            // aquí puedes mostrar pantalla de victoria final
            panelVictoria.SetActive(true);
        }
        ActualizarUI();
    }
    string SerieObjetivoActual() {

        if (indiceActual >= colaSeries.Count) return null;
        return colaSeries[indiceActual];
    }
    // llamado desde SimpleCloudRecognition
    public void OnCartaDetectada(MetaDatos carta) {

        Debug.Log($"[GameManagerS] Detectada carta {carta.nombre} / serie {carta.serie}");

        if (juegoTerminado) return; 

        string objetivo = SerieObjetivoActual();
        if (objetivo == null) return;

        if (carta.serie == objetivo) {
            // Acierto
            puntos += 1;
            SiguienteObjetivo();
        }
        else {
            // Error
            vidas -= 1;
            if (vidas <= 0) {
                vidas = 0;
                juegoTerminado = true;
                // pantalla de game over
                panelError.SetActive(true);
            }
        }
        ActualizarUI();
    }
    void BarajarLista<T>(List<T> lista) {

        for (int i = 0; i < lista.Count; i++) {
            int r = Random.Range(i, lista.Count);
            (lista[i], lista[r]) = (lista[r], lista[i]);
        }
    }
}