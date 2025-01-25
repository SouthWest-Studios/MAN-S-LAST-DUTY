using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    public GameObject configPanel;
    public TextMeshProUGUI[] buttonTexts;
    private Animator[] buttonTextAnimators;
    private bool isConfigOpen = false;
    private TextMeshProUGUI currentSelectedButtonText = null;

    private bool[] buttonSelectedStates;
    public Image backgroundImage;
    public GameObject[] panels;

    public FirstPersonLook isPanelOpen;

    [System.Serializable]
    public class PanelButtons
    {
        public Button[] buttons;
    }
    public List<PanelButtons> panelButtons;

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

        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isConfigOpen)
            {
                CloseConfig();
            }
            else
            {
                OpenConfig();
            }
        }

        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
        {
            TextMeshProUGUI selectedText = GetSelectedText();
            int selectedIndex = System.Array.IndexOf(buttonTexts, selectedText);

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
        else
        {
            SetTextTriggers("In");
        }
    }

    private void OpenConfig()
    {
        isConfigOpen = true;
        configPanel.SetActive(true);
        SetTextTriggers("In");

        if (backgroundImage != null)
        {
            Animator backgroundAnimator = backgroundImage.GetComponent<Animator>();
            if (backgroundAnimator != null)
            {
                backgroundAnimator.SetTrigger("Start");
            }
        }

        isPanelOpen.isPanelOpen = true;
    }

    private void CloseConfig()
    {
        isConfigOpen = false;
        SetTextTriggers("Out");

        if (backgroundImage != null)
        {
            Animator backgroundAnimator = backgroundImage.GetComponent<Animator>();
            if (backgroundAnimator != null)
            {
                backgroundAnimator.SetTrigger("Stop");
            }
        }

        // Cerrar paneles activos
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                Animator panelAnimator = panel.GetComponent<Animator>();
                if (panelAnimator != null)
                {
                    panelAnimator.SetTrigger("Out");
                }
            }
        }

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

    private TextMeshProUGUI GetSelectedText()
    {
        return UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject?.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnButton1Click() { ActivatePanel0(); }
    public void OnButton2Click() { ActivatePanel1(); }
    public void OnButton3Click() { ActivatePanel2(); }
    public void OnButton4Click() { ActivatePanel3(); }
    public void OnButton5Click() { ActivatePanel4(); }
    public void OnButton6Click() { ActivatePanel5(); }

    private void ActivatePanel0() { StartCoroutine(SwitchPanelWithAnimation(0)); ConfigurePanel0(); }
    private void ActivatePanel1() { StartCoroutine(SwitchPanelWithAnimation(1)); ConfigurePanel1(); }
    private void ActivatePanel2() { StartCoroutine(SwitchPanelWithAnimation(2)); ConfigurePanel2(); }
    private void ActivatePanel3() { StartCoroutine(SwitchPanelWithAnimation(3)); ConfigurePanel3(); }
    private void ActivatePanel4() { StartCoroutine(SwitchPanelWithAnimation(4)); ConfigurePanel4(); }
    private void ActivatePanel5() { StartCoroutine(SwitchPanelWithAnimation(5)); ConfigurePanel5(); }

    private void ConfigurePanel0() { Debug.Log("Configuring panel 0 with two buttons"); }
    private void ConfigurePanel1() { Debug.Log("Configuring panel 1 with two buttons"); }
    private void ConfigurePanel2() { Debug.Log("Configuring panel 2 with future functionality"); }
    private void ConfigurePanel3() { Debug.Log("Configuring panel 3 with future functionality"); }
    private void ConfigurePanel4() { Debug.Log("Configuring panel 4 with two buttons"); }
    private void ConfigurePanel5()
    {
        Debug.Log("Configuring panel 5 with two buttons");
        Button[] buttons = panels[5].GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(CloseGame);
        buttons[1].onClick.AddListener(CloseConfig);
    }

    private void CloseGame()
    {
        Debug.Log("Closing game...");
        Application.Quit();
    }

    private void ActivatePanel(int index)
    {
        panels[index].SetActive(true);
        Animator newPanelAnimator = panels[index].GetComponent<Animator>();
        if (newPanelAnimator != null)
        {
            newPanelAnimator.SetTrigger("In");
        }
        StartCoroutine(SwitchPanelWithAnimation(index));
    }

    private IEnumerator SwitchPanelWithAnimation(int newIndex)
    {   panels[newIndex].SetActive(true);
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf && panels[newIndex] != panel)
            {
                Animator panelAnimator = panel.GetComponent<Animator>();
                if (panelAnimator != null)
                {
                    panelAnimator.SetTrigger("Out");
                    yield return new WaitForSeconds(0.5f);
                }
                panel.SetActive(false);
            }
        }  
    }
}