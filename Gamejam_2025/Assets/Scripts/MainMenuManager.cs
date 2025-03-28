using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [Header("Loading Screen")]
    public Image loadingBar;
    public Transform loadingScreenPanel;
    public float loadSpeed = 4f;
    private bool isLoading = true;

    [Header("Windows Bar")]
    public TextMeshProUGUI timeText;
    public Transform windowsBarMenuPanel;
    private bool isWindowsBarShow = false;

   


    [Header("Turn off monitor")]
    public Animator turnOffMonitorAnimator;
    private bool iniciarPartida = false;
    private bool cerrarJuego = false;


    [Header("Sounds")]
    public AudioSource generalAudioSource;
    public AudioClip mouseClick_AC;


    public AudioClip turnOffMonitor_AC;
    public AudioClip popOpen_AC;
    public AudioClip popClose_AC;

  




    // Start is called before the first frame update
    void Start()
    {
        loadingBar.fillAmount = 0;
        loadingScreenPanel.gameObject.SetActive(true);

        windowsBarMenuPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading) { LoadingScreen(); return; }

        timeText.text = DateTime.Now.ToString("hh:mm tt");

        if (Input.GetMouseButtonDown(0))
        {
            generalAudioSource.PlayOneShot(mouseClick_AC);
        }


    }

    void LoadingScreen()
    {
        loadingBar.fillAmount += loadSpeed * Time.deltaTime;
        if(loadingBar.fillAmount >= 0.99f)
        {
            loadingScreenPanel.gameObject.SetActive(false);
            isLoading = false;
        }
    }

    public void ToggleWindowsBarMenu()
    {
        isWindowsBarShow = !isWindowsBarShow;
        windowsBarMenuPanel.gameObject.SetActive(isWindowsBarShow);
    }

    public void NuevaPartida()
    {
        iniciarPartida = true;
        PuzzleManager.instance.NewGame();
        turnOffMonitorAnimator.Play("turnOffMonitor");
        generalAudioSource.PlayOneShot(turnOffMonitor_AC);
    }

    public void CargarPartida()
    {
        if (!PlayerPrefs.HasKey("PuzzleCount"))
        {
            Debug.LogWarning("No hay datos de guardado.");
            return;
        }
        else
        {
            iniciarPartida = true;
            PuzzleManager.instance.LoadGame();
            turnOffMonitorAnimator.Play("turnOffMonitor");
            generalAudioSource.PlayOneShot(turnOffMonitor_AC);
        }
    }

    public void CerrarJuego()
    {
        cerrarJuego = true;
        turnOffMonitorAnimator.Play("turnOffMonitor");
        generalAudioSource.PlayOneShot(turnOffMonitor_AC);
    }


    // Esta función se ejecutará cuando termine la animación
    public void OnTurnOffMonitorEnd()
    {
        if (iniciarPartida)
        {
            SceneManager.LoadScene(1);
        }
        else if (cerrarJuego)
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.ExitPlaymode(); // Funciona en el editor
            #else
                        Application.Quit(); // Funciona en la build
            #endif
        }
    }


    public void IconMark(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }

    public void IconMarkOff(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    public void ApplyTint(DoubleClickUI script)
    {
        script.ApplyTint();
    }

    public void UnapplyTint(DoubleClickUI script)
    {
        script.UnapplyTint();
    }

    public void ShowGameObject(Transform transform)
    {
        transform.gameObject.SetActive(true);
        //Si tiene animacion, getAnimator play in
    }

    public void HideGameObject(Transform transform)
    {
        transform.gameObject.SetActive(false);
        //Si tiene animacion, getAnimator play out
    }

    public void PlayPopOpen()
    {
        generalAudioSource.PlayOneShot(popOpen_AC);
    }

    public void PlayPopClose()
    {
        generalAudioSource.PlayOneShot(popClose_AC);
    }



}
