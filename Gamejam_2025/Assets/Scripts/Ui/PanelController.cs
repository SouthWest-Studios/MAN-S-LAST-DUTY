using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PanelController : MonoBehaviour
{
    public SettingsManager settingsManager;

    public GameObject[] panels;

    // Referencias a los controladores de paneles internos
    public Panel0Handler panel0Handler;
    public Panel1Handler panel1Handler;
    public Panel2Handler panel2Handler;
    public Panel3Handler panel3Handler;
    public Panel4Handler panel4Handler;
    public Panel5Handler panel5Handler;

    private void Start()
    {
        panel0Handler.Initialize(this);  // Pasamos la referencia a PanelController
        panel1Handler.Initialize(this);  // Pasamos la referencia a PanelController
        panel2Handler.Initialize();
        panel3Handler.Initialize();
        panel4Handler.Initialize(this);  // Pasamos la referencia a PanelController
        panel5Handler.Initialize(this);  // Pasamos la referencia a PanelController
    }

    public void ActivatePanel(int index)
    {
        StartCoroutine(SwitchPanelWithAnimation(index));
    }


    private IEnumerator SwitchPanelWithAnimation(int newIndex)
    {
        panels[newIndex].SetActive(true);

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

    public void CloseAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                Animator panelAnimator = panel.GetComponent<Animator>();
                if (panelAnimator != null)
                {
                    panelAnimator.SetTrigger("Out");
                }
                panel.SetActive(false);
            }
        }
    }

    // Función reutilizable para cerrar la configuración
    public void CloseConfig()
    {
        if (settingsManager != null)
        {
            settingsManager.CloseConfig();
        }
    }

    // ------------------------------
    // Clases internas para cada panel
    // ------------------------------

    [System.Serializable]
    public class Panel0Handler
    {
        public Button button1;
        public Button closeConfigButton;  // Renombrado a closeConfigButton

        private PanelController panelController;  // Referencia a la instancia de PanelController

        public void Initialize(PanelController controller)
        {
            panelController = controller;  // Guardamos la referencia de PanelController

            if (button1 != null)
                button1.onClick.AddListener(Button1Action);

            if (closeConfigButton != null)
                closeConfigButton.onClick.AddListener(CloseConfigAction);  // Asignamos el mismo método para cerrar la configuración
        }

        private void Button1Action()
        {
            Debug.Log("Panel 0 - Botón 1 presionado");
        }

        private void CloseConfigAction()
        {
            if (panelController != null)
            {
                panelController.CloseConfig();  // Llamamos a la función de cerrar configuración
            }
        }
    }

    [System.Serializable]
    public class Panel1Handler
    {
        public Button button1;
        public Button closeConfigButton;  // Renombrado a closeConfigButton

        private PanelController panelController;  // Referencia a la instancia de PanelController

        public void Initialize(PanelController controller)
        {
            panelController = controller;  // Guardamos la referencia de PanelController

            if (button1 != null)
                button1.onClick.AddListener(Button1Action);

            if (closeConfigButton != null)
                closeConfigButton.onClick.AddListener(CloseConfigAction);  // Asignamos el mismo método para cerrar la configuración
        }

        private void Button1Action()
        {
            Debug.Log("Panel 1 - Botón 1 presionado");
        }

        private void CloseConfigAction()
        {
            if (panelController != null)
            {
                panelController.CloseConfig();  // Llamamos a la función de cerrar configuración
            }
        }
    }

    [System.Serializable]
    public class Panel2Handler
    {
        [System.Serializable]
        public class PanelElements
        {
            public GameObject content;
            public Button button;
        }

        // Elementos del top banner (siempre visibles)
        public GameObject topBanner;
        public PanelElements gamePanel;
        public PanelElements graphicsPanel;
        public PanelElements audioPanel;
        public PanelElements accessibilityPanel;

        [System.Serializable]
        public class GameSettings
        {
            public Text languageText;
            public Button leftLanguageButton;
            public Button rightLanguageButton;
            public Button applyButton;
        }

        public GameSettings gameSettings;

        [System.Serializable]
        public class AudioSettings
        {
            public Slider globalVolume;
            public Slider musicVolume;
            public Slider voiceVolume;
            public Slider sfxVolume;
            public Button applyButton;
        }

        public AudioSettings audioSettings;

        [System.Serializable]
        public class AccessibilitySettings
        {
            public Toggle subtitlesToggle;
            public Toggle speechToggle;
            public Toggle highContrastToggle;
            public Button applyButton;
        }

        public AccessibilitySettings accessibilitySettings;

        private string[] languages = { "Ingles", "Castellano", "Catalan" };
        private int currentLanguageIndex = 0;

        public void Initialize()
        {
            // Asegurarnos de que el top banner esté activo y visible
            if (topBanner != null)
                topBanner.SetActive(true);

            // Asegurar que todos los botones están siempre visibles
            gamePanel.button?.gameObject.SetActive(true);
            graphicsPanel.button?.gameObject.SetActive(true);
            audioPanel.button?.gameObject.SetActive(true);
            accessibilityPanel.button?.gameObject.SetActive(true);

            // Asignar eventos de clic a los botones
            gamePanel.button?.onClick.AddListener(() => ShowContent(gamePanel));
            graphicsPanel.button?.onClick.AddListener(() => ShowContent(graphicsPanel));
            audioPanel.button?.onClick.AddListener(() => ShowContent(audioPanel));
            accessibilityPanel.button?.onClick.AddListener(() => ShowContent(accessibilityPanel));

            // Configurar sistema de lenguaje
            gameSettings.leftLanguageButton?.onClick.AddListener(() => ChangeLanguage(-1));
            gameSettings.rightLanguageButton?.onClick.AddListener(() => ChangeLanguage(1));
            gameSettings.applyButton?.onClick.AddListener(ApplySettings);

            audioSettings.applyButton?.onClick.AddListener(ApplyAudioSettings);
            accessibilitySettings.applyButton?.onClick.AddListener(ApplyAccessibilitySettings);

            UpdateLanguageDisplay();

            // Mostrar por defecto el contenido de Game
            ShowContent(gamePanel);
        }

        public void ShowContent(PanelElements selectedPanel)
        {
            gamePanel.content?.SetActive(selectedPanel == gamePanel);
            graphicsPanel.content?.SetActive(selectedPanel == graphicsPanel);
            audioPanel.content?.SetActive(selectedPanel == audioPanel);
            accessibilityPanel.content?.SetActive(selectedPanel == accessibilityPanel);

            gamePanel.button.interactable = selectedPanel != gamePanel;
            graphicsPanel.button.interactable = selectedPanel != graphicsPanel;
            audioPanel.button.interactable = selectedPanel != audioPanel;
            accessibilityPanel.button.interactable = selectedPanel != accessibilityPanel;
        }

        private void ChangeLanguage(int direction)
        {
            currentLanguageIndex = (currentLanguageIndex + direction + languages.Length) % languages.Length;
            UpdateLanguageDisplay();
        }

        private void UpdateLanguageDisplay()
        {
            if (gameSettings.languageText != null)
                gameSettings.languageText.text = languages[currentLanguageIndex];
        }

        private void ApplySettings()
        {
            Debug.Log("Settings applied: Language - " + languages[currentLanguageIndex]);
        }

        private void ApplyAudioSettings()
        {
            Debug.Log("Audio settings applied: Global - " + audioSettings.globalVolume.value +
                      ", Music - " + audioSettings.musicVolume.value +
                      ", Voice - " + audioSettings.voiceVolume.value +
                      ", SFX - " + audioSettings.sfxVolume.value);
        }

        private void ApplyAccessibilitySettings()
        {
            Debug.Log("Accessibility settings applied: Subtitles - " + accessibilitySettings.subtitlesToggle.isOn +
                      ", Speech - " + accessibilitySettings.speechToggle.isOn +
                      ", High Contrast - " + accessibilitySettings.highContrastToggle.isOn);
        }
    }

    [System.Serializable]
    public class Panel3Handler
    {
        // Elementos del top banner (siempre visibles)
        public GameObject topBanner;
        public Button creatorsButton;
        public Button thirdLicenseButton;

        // Contenido correspondiente
        public GameObject creatorsContent;
        public GameObject thirdLicenseContent;

        public void Initialize()
        {
            // Asegurarnos de que el top banner esté activo y visible
            if (topBanner != null)
                topBanner.SetActive(true);

            // Asegurar que ambos botones están siempre visibles
            creatorsButton?.gameObject.SetActive(true);
            thirdLicenseButton?.gameObject.SetActive(true);

            // Asignar eventos de clic a los botones
            creatorsButton?.onClick.AddListener(() => ShowContent("creators"));
            thirdLicenseButton?.onClick.AddListener(() => ShowContent("thirdLicense"));

            // Mostrar por defecto el contenido de Creators
            ShowContent("creators");
        }

        public void ShowContent(string selected)
        {
            // Determinar qué contenido mostrar
            bool showCreators = selected == "creators";

            // Mostrar/ocultar el contenido correspondiente
            creatorsContent?.SetActive(showCreators);  // Mostrar el contenido de "Creators" si "creators" está seleccionado
            thirdLicenseContent?.SetActive(!showCreators);  // Mostrar el contenido de "3rd License" si "creators" no está seleccionado

            // Cambiar la interactividad de los botones
            creatorsButton.interactable = !showCreators;  // El botón de "Creators" no debe ser interactivo cuando su contenido está activo
            thirdLicenseButton.interactable = showCreators;  // El botón de "3rd License" no debe ser interactivo cuando su contenido está activo
        }
    }





    [System.Serializable]
    public class Panel4Handler
    {
        public Button button1;
        public Button closeConfigButton;  // Renombrado a closeConfigButton

        private PanelController panelController;  // Referencia a la instancia de PanelController

        public void Initialize(PanelController controller)
        {
            panelController = controller;  // Guardamos la referencia de PanelController

            if (button1 != null)
                button1.onClick.AddListener(Button1Action);

            if (closeConfigButton != null)
                closeConfigButton.onClick.AddListener(CloseConfigAction);  // Asignamos el mismo método para cerrar la configuración
        }

        private void Button1Action()
        {
            Debug.Log("Panel 4 - Botón 1 presionado");
        }

        private void CloseConfigAction()
        {
            if (panelController != null)
            {
                panelController.CloseConfig();  // Llamamos a la función de cerrar configuración
            }
        }
    }

    [System.Serializable]
    public class Panel5Handler
    {
        public Button closeGameButton;
        public Button closeConfigButton;

        private PanelController panelController;  // Referencia a la instancia de PanelController

        public void Initialize(PanelController controller)
        {
            panelController = controller;  // Guardamos la referencia de PanelController

            if (closeConfigButton != null)
                closeConfigButton.onClick.AddListener(CloseConfigAction);  // Asignamos el método de cerrar configuración

            if (closeGameButton != null)
                closeGameButton.onClick.AddListener(CloseGame);
        }


        private void CloseGame()
        {
            Debug.Log("Closing game...");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // Cierra el editor de Unity
#else
                Application.Quit();  // Cierra el juego en compilación
#endif
        }
        private void CloseConfigAction()
        {
            if (panelController != null)
            {
                panelController.CloseConfig();  // Llamamos a la función de cerrar configuración
            }
        }
    }
}
