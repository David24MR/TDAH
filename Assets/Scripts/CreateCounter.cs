using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ClickCounter : MonoBehaviour
{
    public Button countButton; // Arrastra aqu� el bot�n desde el inspector
    public VideoPlayer videoPlayer; // Arrastra aqu� el componente VideoPlayer desde el inspector

    private int clickCount = 0;

    void Start()
    {
        // A�adir un listener al bot�n para ejecutar la funci�n CountClick cuando se haga clic
        if (countButton != null)
        {
            countButton.onClick.AddListener(CountClick);
        }

        // A�adir un listener al VideoPlayer para ejecutar la funci�n OnVideoEnd cuando el video termine
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    void CountClick()
    {
        clickCount++;
        Debug.Log("El bot�n ha sido clickeado " + clickCount + " veces.");
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("El video ha terminado.");
        Debug.Log("El bot�n fue clickeado " + clickCount + " veces durante la reproducci�n del video.");
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
