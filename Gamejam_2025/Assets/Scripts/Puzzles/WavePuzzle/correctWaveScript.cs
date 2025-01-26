using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class correctWaveScript : MonoBehaviour
{
    
    private int correctWaveNum;

    public bool primerCuadranteCheck = false;
    public bool segundoCuadranteCheck = false;
    public bool tercerCuadranteCheck = false;
    public bool cuartoCuadranteCheck = false;

    public ObjectInteraction trigger;

    public PuzzleManager puzzleManager;
    void Start()
    {
        puzzleManager = FindAnyObjectByType<PuzzleManager>();

        int childCount = transform.childCount;

        int randomIndex = Random.Range(0, childCount);

        correctWaveNum = randomIndex;

        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == randomIndex);
        }
        gameObject.SetActive(false);
    }

    public void ShowCorrectWave()
    {
        gameObject.SetActive(true);
        StartCoroutine(DeactivateAfterDelay(0.2f));
    }

    private System.Collections.IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    public void CheckWin(int numero, int cuadrante)
    {
        if (cuadrante == 1)
        {
            if (numero == correctWaveNum + 1)
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
            if (numero == correctWaveNum + 1)
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
            if (numero == correctWaveNum + 1)
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
            if (numero == correctWaveNum + 1)
            {
                cuartoCuadranteCheck = true;
            }
            else
            {
                cuartoCuadranteCheck = false;
            }
        }

        if(primerCuadranteCheck && segundoCuadranteCheck && tercerCuadranteCheck && cuartoCuadranteCheck)
        {
            puzzleManager.CompletePuzzle("WavePuzzle");
        }
    }
}
