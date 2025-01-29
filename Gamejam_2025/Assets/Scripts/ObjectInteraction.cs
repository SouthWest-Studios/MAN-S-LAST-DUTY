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
    private bool isFocused = false; // Estado de la cámara

    public bool isCanvasToOpen = false;

    private Vector3 initialPlayerCameraPosition;
    private Quaternion initialPlayerCameraRotation;
    private bool hasSavedInitialTransform = false;

    public FirstPersonMovement player;

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
            if (!isFocused && !isTransitioning)
            {
                
                StartCoroutine(StartFocusTransition(focusCamera));
            }
            else if (isFocused && !isTransitioning)
            {
                EndFocusTransition();
            }
        }
    }

    IEnumerator StartFocusTransition(Camera targetCamera)
    {
        isTransitioning = true;
        isFocused = true;

        // Bloquear el movimiento del jugador inmediatamente
        player.enabled = false;

        // Si el jugador usa Rigidbody, detenemos su velocidad para evitar deslizamientos
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Guardar la posición y rotación iniciales solo una vez
        if (!hasSavedInitialTransform)
        {
            initialPlayerCameraPosition = cameraFirstPerson.transform.position;
            initialPlayerCameraRotation = cameraFirstPerson.transform.rotation;
            hasSavedInitialTransform = true;
        }

        // Desactivar el control de la cámara del jugador
        cameraFirstPerson.enabled = false;

        Transform playerCameraTransform = cameraFirstPerson.transform;
        Vector3 startPosition = playerCameraTransform.position;
        Quaternion startRotation = playerCameraTransform.rotation;

        Vector3 targetPosition = targetCamera.transform.position;
        Quaternion targetRotation = targetCamera.transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed;
            float t = Mathf.Clamp01(elapsedTime);

            playerCameraTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            playerCameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        // Asegurar que la cámara llega a su destino
        playerCameraTransform.position = targetPosition;
        playerCameraTransform.rotation = targetRotation;

        cameraFirstPerson.gameObject.SetActive(false);
        targetCamera.gameObject.SetActive(true);

        if (isCanvasToOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            canvasToOpen.SetActive(true);
            cameraFirstPerson.isPanelOpen = true;
            cameraFirstPerson.crosshairController.gameObject.SetActive(false);
        }

        isTransitioning = false;
    }


    public void EndFocusTransition()
    {
        if (focusCamera == null || isTransitioning) return;

        isFocused = false; // Estado actualizado

        if (isCanvasToOpen)
        {
            canvasToOpen.SetActive(false);
            cameraFirstPerson.isPanelOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        focusCamera.gameObject.SetActive(false);
        cameraFirstPerson.gameObject.SetActive(true);

        // Restaurar la posición y rotación originales
        cameraFirstPerson.transform.position = initialPlayerCameraPosition;
        cameraFirstPerson.transform.rotation = initialPlayerCameraRotation;

        cameraFirstPerson.enabled = true;
        cameraFirstPerson.crosshairController.gameObject.SetActive(true);

        // Resetear variables
        isTransitioning = false;
        hasSavedInitialTransform = false;
        player.enabled = true;
    }
}
