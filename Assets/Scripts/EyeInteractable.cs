using UnityEngine;

public class ObjectGazeHandler : MonoBehaviour
{
    public float maxGazeDistance = 10f; // Distancia máxima de detección de la mirada
    public LayerMask gazeLayerMask;

    private bool isBeingGazed = false;
    private float gazeTimer = 0f;

    private void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, maxGazeDistance, gazeLayerMask))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isBeingGazed = true;
                gazeTimer += Time.deltaTime;
            }
            else
            {
                isBeingGazed = false;
            }
        }
        else
        {
            isBeingGazed = false;
        }

        if (isBeingGazed)
        {
            Debug.Log("El objeto está siendo mirado durante " + gazeTimer.ToString("F2") + " segundos.");
        }
        else
        {
            gazeTimer = 0f;
        }
    }
}
