using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour
{
    public GameObject crosshair;
    public Color defaultColor = Color.white;
    public Color interactColor = Color.green;
    public FirstPersonLook cameraFirstPerson;
    public float maxInteractionDistance = 5f;
    public KeyCode interactionKey = KeyCode.E;

    private Camera playerCamera;
    private UnityEngine.UI.Image crosshairImage;

    

    void Start()
    {
        playerCamera = Camera.main;

        if (crosshair != null)
        {
            crosshairImage = crosshair.GetComponent<UnityEngine.UI.Image>();
        }
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxInteractionDistance))
        {
            if (hit.collider.CompareTag("Button"))
            {
                
                if (crosshairImage != null)
                {
                    crosshairImage.color = interactColor;
                }

                Button targetButton = hit.collider.gameObject.GetComponent<Button>();

                if (targetButton != null && Input.GetKeyDown(interactionKey))
                {
                    targetButton.onClick.Invoke();
                }
            }
            else
            {
                if (crosshairImage != null)
                {
                    crosshairImage.color = defaultColor;
                }
            }
        }
        else
        {
            if (crosshairImage != null)
            {
                crosshairImage.color = defaultColor;
            }
        }
    }
}
