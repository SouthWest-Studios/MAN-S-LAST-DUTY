using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SettingsManager : MonoBehaviour
{
    public GameObject configPanel;
    public TextMeshProUGUI[] buttonTexts;  // Botones de la izquierda referenciados
    private Animator[] buttonTextAnimators;
    private bool isConfigOpen = false;
    private TextMeshProUGUI currentSelectedButtonText = null;
    private bool[] buttonSelectedStates;

    public Image backgroundImage;
    public PanelController panelController;
    public FirstPersonLook isPanelOpen;

    private void Start()
    {
        buttonTextAnimators = new Animator[buttonTexts.Length];
        buttonSelectedStates = new bool[buttonTexts.Length];

        for (int i = 0; i < buttonTexts.Length; i++)
        {
            buttonTextAnimators[i] = buttonTexts[i].GetComponent<Animator>();
            buttonSelectedStates[i] = false;
        }

        configPanel.SetActive(false);
        SetTextTriggers("Out");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isConfigOpen)
                CloseConfig();
            else
                OpenConfig();
        }

        // Obtener el objeto seleccionado actualmente
        GameObject selectedObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (selectedObject != null)
        {
            TextMeshProUGUI selectedText = selectedObject.GetComponentInChildren<TextMeshProUGUI>();

            // Verificar si el texto seleccionado está dentro de los botones de la izquierda
            int selectedIndex = System.Array.IndexOf(buttonTexts, selectedText);
            if (selectedIndex != -1)
            {
                HandleButtonSelection(selectedText, selectedIndex);
            }
        }
        else
        {
            SetTextTriggers("In");
        }
    }

    private void HandleButtonSelection(TextMeshProUGUI selectedText, int selectedIndex)
    {
        if (selectedText != currentSelectedButtonText)
        {
            if (currentSelectedButtonText != null)
            {
                SetTextTrigger(currentSelectedButtonText, "Unselect");
                int currentIndex = System.Array.IndexOf(buttonTexts, currentSelectedButtonText);
                buttonSelectedStates[currentIndex] = false;
            }

            currentSelectedButtonText = selectedText;
            buttonSelectedStates[selectedIndex] = true;
            SetTextTrigger(currentSelectedButtonText, "Select");
        }
    }

    private void OpenConfig()
    {
        isConfigOpen = true;
        configPanel.SetActive(true);
        SetTextTriggers("In");

        if (backgroundImage != null)
            backgroundImage.GetComponent<Animator>()?.SetTrigger("Start");

        isPanelOpen.isPanelOpen = true;
    }

    public void CloseConfig()
    {
        isConfigOpen = false;
        SetTextTriggers("Out");

        if (backgroundImage != null)
            backgroundImage.GetComponent<Animator>()?.SetTrigger("Stop");

        panelController.CloseAllPanels();
        isPanelOpen.isPanelOpen = false;

        StartCoroutine(CloseConfigWithDelay());
    }

    private IEnumerator CloseConfigWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        configPanel.SetActive(false);
    }

    private void SetTextTriggers(string trigger)
    {
        foreach (Animator textAnimator in buttonTextAnimators)
        {
            if (textAnimator != null)
            {
                textAnimator.ResetTrigger("In");
                textAnimator.ResetTrigger("Out");
                textAnimator.SetTrigger(trigger);
            }
        }
    }

    private void SetTextTrigger(TextMeshProUGUI text, string trigger)
    {
        Animator textAnimator = text.GetComponent<Animator>();
        if (textAnimator != null)
        {
            textAnimator.ResetTrigger("In");
            textAnimator.ResetTrigger("Out");
            textAnimator.SetTrigger(trigger);
        }
    }

    public void OnButtonClick(int index)
    {
        panelController.ActivatePanel(index);
    }
}