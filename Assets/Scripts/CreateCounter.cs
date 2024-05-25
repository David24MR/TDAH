using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ClickCounter : MonoBehaviour
{
    public Button countButton; // Arrastra aquí el botón desde el inspector
    public VideoPlayer videoPlayer; // Arrastra aquí el componente VideoPlayer desde el inspector

    private int clickCount = 0;

    void Start()
    {
        // Añadir un listener al botón para ejecutar la función CountClick cuando se haga clic
        if (countButton != null)
        {
            countButton.onClick.AddListener(CountClick);
        }

        // Añadir un listener al VideoPlayer para ejecutar la función OnVideoEnd cuando el video termine
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    void CountClick()
    {
        clickCount++;
        Debug.Log("El botón ha sido clickeado " + clickCount + " veces.");
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("El video ha terminado.");
        Debug.Log("El botón fue clickeado " + clickCount + " veces durante la reproducción del video.");
    }

    void OnDestroy()
    {
        // Limpiar los listeners al destruir el objeto para evitar posibles errores
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}
