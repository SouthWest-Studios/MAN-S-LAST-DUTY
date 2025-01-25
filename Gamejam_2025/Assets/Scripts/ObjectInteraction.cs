using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject canvasToOpen; // Arrastra el Canvas aquí en el Inspector.
    private bool playerIsNear = false; // Indica si el jugador está cerca.
    public FirstPersonLook cameraFirstPerson;

    public bool isCanvasToOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger es el jugador.
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            Debug.Log("El jugador está cerca del cubo.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando el jugador sale del trigger, actualiza el estado.
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            Debug.Log("El jugador se alejó del cubo.");
        }
    }

    private void Update()
    {
        // Si el jugador está cerca y presiona la tecla E, activa el Canvas.
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            if (isCanvasToOpen)
            {
                cameraFirstPerson.isPanelOpen = true;
                canvasToOpen.SetActive(true); // Activa el Canvas y el cursor.
            }
            else
            {
                GetComponent<Button>().onClick.Invoke();
            }
            
        }
        if (playerIsNear && Input.GetKeyDown(KeyCode.Escape))
        {
            cameraFirstPerson.isPanelOpen = false;
            canvasToOpen.SetActive(false); // Desactiva el Canvas y el cursor.
            
        }
    }
}
