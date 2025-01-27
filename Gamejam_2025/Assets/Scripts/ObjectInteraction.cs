using System.Collections;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject canvasToOpen;
    private bool playerIsNear = false;
    public FirstPersonLook cameraFirstPerson;
    public Camera focusCamera;
    public float transitionSpeed = 2.0f; // Velocidad de transición
    private bool isTransitioning = false;

    public bool isCanvasToOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            Debug.Log("El jugador está cerca del cubo.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            Debug.Log("El jugador se alejó del cubo.");
        }
    }

    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            if (focusCamera != null && !isTransitioning)
            {
                StartCoroutine(StartFocusTransition(focusCamera));
            }
        }

        if (playerIsNear && Input.GetKeyDown(KeyCode.Escape))
        {
            EndFocusTransition();
        }
    }

    IEnumerator StartFocusTransition(Camera targetCamera)
    {
        isTransitioning = true;

        // Desactivar el control de la cámara del jugador
        cameraFirstPerson.enabled = false;

        // Guardar la posición y rotación iniciales de la cámara del jugador
        Transform playerCameraTransform = cameraFirstPerson.transform;
        Vector3 startPosition = playerCameraTransform.position;
        Quaternion startRotation = playerCameraTransform.rotation;

        // Posición y rotación de la cámara objetivo
        Vector3 targetPosition = targetCamera.transform.position;
        Quaternion targetRotation = targetCamera.transform.rotation;

        float elapsedTime = 0f;

        // Transición suave entre las cámaras
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed;

            // Interpolación de posición y rotación
            playerCameraTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            playerCameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);

            yield return null;
        }

        // Al finalizar la transición, activa la cámara de enfoque y desactiva la cámara del jugador
        cameraFirstPerson.gameObject.SetActive(false);
        targetCamera.gameObject.SetActive(true);

        // Activa el canvas si es necesario
        if (isCanvasToOpen)
        {
            canvasToOpen.SetActive(true);
            cameraFirstPerson.isPanelOpen = true;
        }

        isTransitioning = false;
    }

    void EndFocusTransition()
    {
        if (focusCamera == null || isTransitioning) return;

        // Desactiva el canvas si está activo
        if (isCanvasToOpen)
        {
            canvasToOpen.SetActive(false);
            cameraFirstPerson.isPanelOpen = false;
        }

        // Reactiva la cámara del jugador
        focusCamera.gameObject.SetActive(false);
        cameraFirstPerson.gameObject.SetActive(true);

        // Reactiva el control de la cámara del jugador
        cameraFirstPerson.enabled = true;

        focusCamera = null;
        isTransitioning = false;
    }
}
