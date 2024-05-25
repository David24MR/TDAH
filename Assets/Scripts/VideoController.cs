using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Arrastra aqu� el componente VideoPlayer desde el inspector
    public Button playButton; // Arrastra aqu� el bot�n desde el inspector

    void Start()
    {
        // Aseg�rate de que el video no se reproduzca autom�ticamente al iniciar la escena
        videoPlayer.playOnAwake = false;

        // A�adir un listener al bot�n para ejecutar la funci�n PlayVideo cuando se haga clic
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
