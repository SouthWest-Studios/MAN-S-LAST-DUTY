using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonGameManager : MonoBehaviour
{
    public List<ButtonController> buttons;
    public float lightOnTime = 0.5f;
    public float lightOffTime = 0.3f;

    private List<int> pattern = new List<int>();
    private int currentStep = 0;
    private bool playerTurn = false;
    private bool gameStarted = false;
    private bool gameFinished = false;
    public TraumaInducer shakeEffect;
    public float errorShakeIntensity = 0.3f;

    void Start()
    {
        
    }

    private IEnumerator StartGame()
    {
        yield return StartCoroutine(FlashLightsAtStart());
        GenerateNextStep();
        yield return PlayPattern();
        playerTurn = true;
    }

    private IEnumerator FlashLightsAtStart()
    {
        for (int i = 0; i < 3; i++) // Tres parpadeos
        {
            // Activa todas las luces
            foreach (var button in buttons)
            {
                button.ActivateLight();
            }
            yield return new WaitForSeconds(0.3f); // Tiempo de encendido

            // Apaga todas las luces
            foreach (var button in buttons)
            {
                button.DeactivateLight();
            }
            yield return new WaitForSeconds(0.3f); // Tiempo de apagado
        }
    }

    private void GenerateNextStep()
    {
        int randomIndex = Random.Range(0, buttons.Count);
        pattern.Add(randomIndex);
    }

    private IEnumerator PlayPattern()
    {
        playerTurn = false;

        foreach (int index in pattern)
        {
            buttons[index].ActivateLight();
            yield return new WaitForSeconds(lightOnTime);
            buttons[index].DeactivateLight();
            yield return new WaitForSeconds(lightOffTime);
        }

        playerTurn = true;
        currentStep = 0; // Resetea el progreso del jugador para la nueva ronda
    }

    public void ButtonPressed(int buttonIndex)
    {
        if (!gameStarted)
        {
            StartCoroutine(StartGame());
            gameStarted = true;
        }
        else
        {
            if(gameFinished)
            {
                
                
                return;
                
            }
            if (!playerTurn) return;

            if (pattern[currentStep] == buttonIndex)
            {
                currentStep++;
                if (currentStep >= pattern.Count)
                {
                    if (pattern.Count < 6)
                    {
                        // El jugador completó el patrón correctamente
                        StartCoroutine(NextRound());
                    }
                    else
                    {
                        Debug.Log("juegoComletado");
                        gameFinished = true;
                    }

                    
                }
            }
            else
            {
                if(shakeEffect != null)
                {
                    shakeEffect.MaximumStress = errorShakeIntensity;
                    shakeEffect.Delay = 0f;
                    shakeEffect.InduceTrauma();
                    Debug.Log("¡Game Over! Reiniciando...");
                    StartCoroutine(RestartGame());
                }
                
            }
        }
    }

    private IEnumerator NextRound()
    {
        yield return new WaitForSeconds(1f);
        GenerateNextStep();
        yield return PlayPattern();
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1f);
        pattern.Clear();
        currentStep = 0;
        StartCoroutine(StartGame());
    }
}
