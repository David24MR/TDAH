using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;

public class EyeInteractable : MonoBehaviour
{
    public float maxGazeDistance = 10f; // Distancia m�xima de detecci�n de la mirada
    public LayerMask gazeLayerMask;
    public VideoPlayer videoPlayer; // Arrastra aqu� el componente VideoPlayer desde el inspector
    public Button countButton; // Arrastra aqu� el bot�n desde el inspector
    public Text gazeInfoText; // Objeto de texto para mostrar la informaci�n de mirada
    public List<GameObject> allFigures; // Lista de todas las figuras que se deben ocultar al finalizar el video
    public GameObject resultsCanvas; // Canvas de los resultados de los intervalos de mirada

    private bool isBeingGazed = false;
    private float gazeStartTime = 0f;
    private List<(float, float)> gazeIntervals = new List<(float, float)>(); // Lista de intervalos de mirada
    private bool isVideoPlaying = false;
    private int clickCount = 0;

    private void Start()
    {
        // A�adir un listener al bot�n para ejecutar la funci�n CountClick cuando se haga clic
        if (countButton != null)
        {
            countButton.onClick.AddListener(CountClick);
        }

        // A�adir listeners al VideoPlayer para detectar cuando el video comienza y termina
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.started += OnVideoStart;
        }
    }

    private void Update()
    {
        if (isVideoPlaying)
        {
            // Emitir el rayo desde la posici�n de la c�mara hacia adelante
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, maxGazeDistance, gazeLayerMask))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (!isBeingGazed)
                    {
                        isBeingGazed = true;
                        gazeStartTime = Time.time; // Registrar el inicio del intervalo de mirada
                    }
                }
                else
                {
                    if (isBeingGazed)
                    {
                        float gazeEndTime = Time.time; // Registrar el final del intervalo de mirada
                        gazeIntervals.Add((gazeStartTime, gazeEndTime)); // Agregar el intervalo a la lista
                    }
                    isBeingGazed = false;
                }
            }
            else
            {
                if (isBeingGazed)
                {
                    float gazeEndTime = Time.time; // Registrar el final del intervalo de mirada
                    gazeIntervals.Add((gazeStartTime, gazeEndTime)); // Agregar el intervalo a la lista
                }
                isBeingGazed = false;
            }
        }
    }

    void CountClick()
    {
        if (isVideoPlaying)
        {
            clickCount++;
            Debug.Log("El bot�n ha sido clickeado " + clickCount + " veces.");
        }
    }

    void OnVideoStart(VideoPlayer vp)
    {
        isVideoPlaying = true;
        Debug.Log("El video ha comenzado.");
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        isVideoPlaying = false;
        Debug.Log("El video ha terminado.");

        // Ocultar todas las figuras excepto el canvas de los resultados de los intervalos
        foreach (var figure in allFigures)
        {
            figure.SetActive(false);
        }
        resultsCanvas.SetActive(true);

        // Registrar el final del �ltimo intervalo de mirada si el usuario todav�a estaba mirando
        if (isBeingGazed)
        {
            float gazeEndTime = Time.time; // Registrar el final del intervalo de mirada
            gazeIntervals.Add((gazeStartTime, gazeEndTime)); // Agregar el intervalo a la lista
        }

        // Mostrar los intervalos de mirada en el objeto de texto
        string gazeInfo = "Intervalos de mirada:\n";
        foreach (var interval in gazeIntervals)
        {
            float intervalDuration = interval.Item2 - interval.Item1;
            gazeInfo += intervalDuration.ToString("F2") + " segundos\n";
        }
        gazeInfoText.text = gazeInfo;
    }

    private void OnDestroy()
    {
        // Limpiar los listeners al destruir el objeto para evitar posibles errores
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
            videoPlayer.started -= OnVideoStart;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * maxGazeDistance);
    }
}
