using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteracctionCast : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject crosshair;
    public Color defaultColor = Color.white;
    public Color interactColor = Color.green;
    public KeyCode interactionKey = KeyCode.E;
    private UnityEngine.UI.Image crosshairImage;
    public float maxInteractionDistance = 5f;

    void Start()
    {
        if (crosshair != null)
        {
            crosshairImage = crosshair.GetComponent<UnityEngine.UI.Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
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

                if (Input.GetKeyDown(interactionKey) && targetButton.interactable == true)
                {
                    targetButton.onClick.Invoke();
                    Debug.Log("button pressed");
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
