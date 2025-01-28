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
        turnOffMonitorAnimator.Play("turnOffMonitor");
    }

    public void CerrarJuego()
    {
        cerrarJuego = true;
        turnOffMonitorAnimator.Play("turnOffMonitor");
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



}
