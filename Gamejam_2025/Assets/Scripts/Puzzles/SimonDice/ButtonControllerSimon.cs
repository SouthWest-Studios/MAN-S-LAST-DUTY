using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public int buttonIndex;
    private Light pointLight;
    private SimonGameManager gameManager;
    

    void Start()
    {
        pointLight = GetComponentInChildren<Light>();
        pointLight.intensity = 0f;
        gameManager = FindObjectOfType<SimonGameManager>();
    }

    public void ActivateLight()
    {
        pointLight.intensity = 1f; // Ajusta la intensidad según prefieras
    }

    public void DeactivateLight()
    {
        pointLight.intensity = 0f;
    }

    public void OnButtonClick()
    {
        ActivateLight();
        Invoke("DeactivateLight", 0.2f); // Feedback visual breve
        gameManager.ButtonPressed(buttonIndex);
    }
}
