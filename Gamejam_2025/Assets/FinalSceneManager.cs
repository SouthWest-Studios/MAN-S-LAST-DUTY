using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalSceneManager : MonoBehaviour
{

    [Header("Turn off monitor")]
    public Animator turnOffMonitorAnimator;


    [Header("Sounds")]
    public AudioSource generalAudioSource;
    public AudioClip turnOffMonitor_AC;
    public AudioClip mouseClick_AC;

    [Header("Puntuacion")]
    public TextMeshProUGUI numeroTexto;



    public void Start()
    {
        numeroTexto.text = PuzzleManager.numeroDeBucles.ToString();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            generalAudioSource.PlayOneShot(mouseClick_AC);
        }
    }


    public void ReturnMainMenu()
    {
        
        turnOffMonitorAnimator.Play("turnOffMonitor");
        generalAudioSource.PlayOneShot(turnOffMonitor_AC);
    }

    public void OnTurnOffMonitorEnd()
    {
        
        
        SceneManager.LoadScene(0);
        
    }


    public void ApplyTint(DoubleClickUI script)
    {
        script.ApplyTint();
    }

    public void UnapplyTint(DoubleClickUI script)
    {
        script.UnapplyTint();
    }




}
