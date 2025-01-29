using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Linq;
using TMPro;

public class correctWaveScript : MonoBehaviour
{
    
    private int correctWaveNum;

    public bool primerCuadranteCheck = false;
    public bool segundoCuadranteCheck = false;
    public bool tercerCuadranteCheck = false;
    public bool cuartoCuadranteCheck = false;

    public ObjectInteraction trigger;

    public PuzzleManager puzzleManager;

    private bool canShowWave = true;

    public Button button;

    int randomIndex;

    public List<GameObject> correctWaves;

    public TextMeshPro finalText;

    public static int[] finalRandNum = new int[4];

    public NavigationManager navigationManager;


    void Start()
    {
        puzzleManager = FindAnyObjectByType<PuzzleManager>();

        int childCount = transform.childCount;
        Random.InitState((int)System.DateTime.Now.Ticks);
        randomIndex = Random.Range(1, 5);

        correctWaveNum = randomIndex;
        finalRandNum = new int[4];

        for (int i = 0; i < finalRandNum.Length; i++)
        {
            finalRandNum[i] = Random.Range(0, 10);
        }

        finalText.text = finalRandNum[0].ToString() + finalRandNum[1].ToString() + finalRandNum[2].ToString() + finalRandNum[3].ToString();

        Debug.Log("wave number = " +  finalText.text);

        navigationManager.InitializeNavigationWaves();

        this.gameObject.SetActive(false);

        canShowWave = true;
    }

    private void Update()
    {
        if (!canShowWave)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }

        
    }
    public void ShowCorrectWave()
    {
        if (!canShowWave)
        {
            
            Debug.Log("ShowCorrectWave bloqueado por cooldown.");
            return;
        }
        
        Debug.Log("Ejecutando ShowCorrectWave.");
        canShowWave = false;
        int childCount = transform.childCount;

        correctWaves[randomIndex - 1].SetActive(true);


        StartCoroutine(DeactivateAfterDelay(0.2f));
        StartCoroutine(ResetWaveCooldown(4f));
    }


    private System.Collections.IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        int childCount = transform.childCount;

        correctWaves[randomIndex - 1].SetActive(false);
    }

    private IEnumerator ResetWaveCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canShowWave = true; // Permitimos que la función pueda ejecutarse nuevamente
    }

    public void CheckWin(int numero, int cuadrante)
    {
        if (cuadrante == 1)
        {
            if (numero == correctWaveNum)
            {
                primerCuadranteCheck = true;
            }
            else
            {
                primerCuadranteCheck = false;
            }
        }
        if (cuadrante == 2)
        {
            if (numero == correctWaveNum)
            {
                segundoCuadranteCheck = true;
            }
            else
            {
                segundoCuadranteCheck = false;
            }
        }
        if (cuadrante == 3)
        {
            if (numero == correctWaveNum)
            {
                tercerCuadranteCheck = true;
            }
            else
            {
                tercerCuadranteCheck = false;
            }
        }
        if (cuadrante == 4)
        {
            if (numero == correctWaveNum)
            {
                cuartoCuadranteCheck = true;
            }
            else
            {
                cuartoCuadranteCheck = false;
            }
        }

        
    }

    public void CheckWin2()
    {
        if (primerCuadranteCheck && segundoCuadranteCheck && tercerCuadranteCheck && cuartoCuadranteCheck)
        {
            puzzleManager.CompletePuzzle("WavePuzzle");
            finalText.gameObject.SetActive(true);

            



            trigger.EndFocusTransition();
            trigger.enabled = false;
        }
    }
}
