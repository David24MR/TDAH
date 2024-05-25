using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Arrastra aquí el componente VideoPlayer desde el inspector
    public Button playButton; // Arrastra aquí el botón desde el inspector

    void Start()
    {
        // Asegúrate de que el video no se reproduzca automáticamente al iniciar la escena
        videoPlayer.playOnAwake = false;

        // Añadir un listener al botón para ejecutar la función PlayVideo cuando se haga clic
        playButton.onClick.AddListener(PlayVideo);
    }

    void PlayVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }
}
