using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;

public class EyeInteractable : MonoBehaviour
{
    public float maxGazeDistance = 10f; // Distancia máxima de detección de la mirada
    public LayerMask gazeLayerMask;
    public VideoPlayer videoPlayer; // Arrastra aquí el componente VideoPlayer desde el inspector
    public PressableButton pressableButton; // Arrastra aquí el PressableButton desde el inspector
    public Text gazeInfoText; // Objeto de texto para mostrar la información de mirada
    public List<GameObject> allFigures; // Lista de todas las figuras que se deben ocultar al finalizar el video
    public GameObject resultsCanvas; // Canvas de los resultados de los intervalos de mirada

    private bool isBeingGazed = false;
    private float gazeStartTime = 0f;
    private List<(float, float)> gazeIntervals = new List<(float, float)>(); // Lista de intervalos de mirada
    private bool isVideoPlaying = false;
    private int clickCount = 0;
    private float totalGazeTime = 0f; // Tiempo total de mirada al video

    private void Start()
    {
        // Añadir un listener al PressableButton para ejecutar la función CountButtonPresses cuando se haga clic
        if (pressableButton != null)
        {
            pressableButton.OnClicked.AddListener(CountButtonPresses);
        }
        else
        {
            Debug.LogError("No se encontró un componente PressableButton en los hijos de este objeto.");
        }

        // Añadir listeners al VideoPlayer para detectar cuando el video comienza y termina
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.started += OnVideoStart;
            videoPlayer.playOnAwake = false; // Asegúrate de que el video no se reproduzca automáticamente al iniciar la escena
        }
    }

    private void Update()
    {
        if (isVideoPlaying)
        {
            // Emitir el rayo desde la posición de la cámara hacia adelante
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
                        totalGazeTime += gazeEndTime - gazeStartTime; // Sumar al tiempo total de mirada
                        gazeIntervals.Add((gazeStartTime, gazeEndTime)); // Agregar el intervalo a la lista
                        isBeingGazed = false;
                    }
                }
            }
            else
            {
                if (isBeingGazed)
                {
                    float gazeEndTime = Time.time; // Registrar el final del intervalo de mirada
                    totalGazeTime += gazeEndTime - gazeStartTime; // Sumar al tiempo total de mirada
                    gazeIntervals.Add((gazeStartTime, gazeEndTime)); // Agregar el intervalo a la lista
                    isBeingGazed = false;
                }
            }
        }
    }

    public void CountButtonPresses()
    {
        if (isVideoPlaying) // Solo contar los clics si el video se está reproduciendo
        {
            clickCount++;
            Debug.Log("El botón ha sido clickeado " + clickCount + " veces.");
        }
    }

    private void OnVideoStart(VideoPlayer vp)
    {
        isVideoPlaying = true;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        isVideoPlaying = false;

        if (isBeingGazed)
        {
            float gazeEndTime = Time.time; // Registrar el final del intervalo de mirada
            totalGazeTime += gazeEndTime - gazeStartTime; // Sumar al tiempo total de mirada
            gazeIntervals.Add((gazeStartTime, gazeEndTime)); // Agregar el intervalo a la lista
            isBeingGazed = false;
        }

        // Mostrar solo los intervalos de mirada
        string intervalsInfo = "Intervalos que se vio al video:\n";
        foreach (var interval in gazeIntervals)
        {
            float intervalDuration = interval.Item2 - interval.Item1;
            intervalsInfo += intervalDuration.ToString("F2") + " segundos\n";
        }

        // Calcular el tiempo total de mirada en el texto
        string totalTimeInfo = "Tiempo total de mirada al video: " + totalGazeTime.ToString("F2") + " segundos";

        // Mostrar el tiempo total de mirada y los intervalos en el texto
        gazeInfoText.text = intervalsInfo + "\n" + totalTimeInfo;

        // Mostrar el número de veces que se presionó el botón durante la reproducción del video
        string clickInfo = "El botón ha sido clickeado " + clickCount + " veces durante la reproducción del video.";
        Debug.Log(clickInfo);

        // Ocultar todas las figuras
        foreach (GameObject figure in allFigures)
        {
            figure.SetActive(false);
        }

        // Mostrar el canvas de resultados
        resultsCanvas.SetActive(true);
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