using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerP : MonoBehaviour {

    public int vidasIniciales = 3;
    public List<string> todasLasSeries = new List<string> {
        "princesa de hyrule",
        "nintendo wii",
        "windwaker",
        "princesa de reino champi",
        "nintendo 64",
        "hermanos fontaneros",
        "link guerrero"
    };

    [Header("Referencias UI")]
    public GameObject panelVictoria;
    public GameObject panelError;

    public TMP_Text vidasText;
    public TMP_Text puntosText;
    public TMP_Text targetText;

    int vidas;
    int puntos;
    List<string> colaSeries;
    int indiceActual = 0;
    bool juegoTerminado = false;

    HashSet<string> nombresEncontrados = new HashSet<string>();

    void Start() {

        vidas = vidasIniciales;
        puntos = 0;
        colaSeries = new List<string>(todasLasSeries);

        BarajarLista(colaSeries);
        ActualizarUI();
    }
    string SerieObjetivoActual() {
        if (indiceActual >= colaSeries.Count) return null;
        return colaSeries[indiceActual];
    }
    public void OnCartaDetectada(MetaDatos carta) {

        Debug.Log($"[GameManagerS] Detectada carta {carta.nombre} / serie {carta.serie}");

        if (juegoTerminado) return;

        string objetivo = SerieObjetivoActual();
        if (objetivo == null) return;

        if (carta.serie != objetivo) {
            vidas--;
            if (vidas <= 0) {
                vidas = 0;
                juegoTerminado = true;
                panelError.SetActive(true);
            }
            ActualizarUI();
            return;
        }
        if (nombresEncontrados.Contains(carta.nombre)) {
            // misma carta repetida, ni sumas ni restas
            return;
        }
        nombresEncontrados.Add(carta.nombre);

        if (nombresEncontrados.Count >= 2) {

            puntos++;
            indiceActual++;
            nombresEncontrados.Clear();

            if (indiceActual >= colaSeries.Count) {
                juegoTerminado = true; // ha completado todas
                panelVictoria.SetActive(true);
            }
        }
        ActualizarUI();
    }
    void ActualizarUI() {

        if (vidasText != null) vidasText.text = "vidas: "+vidas.ToString();
        if (puntosText != null) puntosText.text = "puntos: "+puntos.ToString();
        if (targetText != null) {
            string objetivo = SerieObjetivoActual() ?? "-";
            targetText.text = "busca alguien: "+objetivo;
        }
    }
    void BarajarLista<T>(List<T> lista) {
        for (int i = 0; i < lista.Count; i++) {
            int r = Random.Range(i, lista.Count);
            (lista[i], lista[r]) = (lista[r], lista[i]);
        }
    }
}