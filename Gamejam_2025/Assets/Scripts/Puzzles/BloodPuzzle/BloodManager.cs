using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodManager : MonoBehaviour
{

    private PuzzleManager puzzleManager;
    public static List<BloodPuzzle> blood;
    private FetusScript fetusScript;
    public GameObject bloodTube;

    public AudioSource rotateSound;
    public AudioSource startSound;
    public AudioSource finishSound;

    public GameObject[] rotatingTubes; // Arrastra aquí los 4 tubos
    private bool isRotating = false;
    public float rotationSpeed = 50f; // Velocidad de rotación ajustable
    private float currentRotationSpeed = 50f;
    private float maxRotationSpeed = 3000f;
    private float elapsedTime = 0f;
    private bool accelerating = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (isRotating)
        {
            if (elapsedTime <= 20f && accelerating)
            {
                // Acelerar hasta 3000 en 20 segundos
                currentRotationSpeed = Mathf.Lerp(50f, maxRotationSpeed, elapsedTime / 20f);
            }
            else if (elapsedTime > 20f && elapsedTime <= 30f)
            {
                accelerating = false;
                // Desacelerar de 3000 a 0 en los últimos 10 segundos
                currentRotationSpeed = Mathf.Lerp(maxRotationSpeed, 0f, (elapsedTime - 20f) / 10f);
            }

            foreach (GameObject tube in rotatingTubes)
            {
                tube.transform.Rotate(Vector3.up * currentRotationSpeed * Time.deltaTime);
            }

            elapsedTime += Time.deltaTime;
        }
    }

    public void CheckWin()
    {
        int winCounter = 0;

        for (int i = 0; i < blood.Count; i++)
        {
            if (blood[i].correctNumber == blood[i].currentNumber)
            {
                winCounter++;
            }
        }
        if (winCounter == 3)
        {
            // L�gica de CheckWin despu�s de la espera
            PuzzleManager puzzleManager = FindAnyObjectByType<PuzzleManager>();
            if (puzzleManager != null)
            {
                puzzleManager.CompletePuzzle("BloodPuzzle");
            }

            
        }
        
    }

    

    public void ShowTube()
    {
        if (startSound != null)
        {
            startSound.Play();
        }
        isRotating = true; // Comenzar rotación
        elapsedTime = 0f;
        accelerating = true;
        currentRotationSpeed = 50f;
        StartCoroutine(DelayedShowTube());
    }

    private IEnumerator DelayedShowTube()
    {
        // Espera 30 segundos
        //GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(30f);
        isRotating = false; // Detener rotación
        
        // Resetear la rotación de los tubos
        foreach (GameObject tube in rotatingTubes)
        {
            tube.transform.rotation = Quaternion.identity;
        }

        FetusScript fetusScript = FindAnyObjectByType<FetusScript>();
        if (fetusScript != null)
        {
            
        }

        if (finishSound != null)
        {
            finishSound.Play();
        }

        bloodTube.SetActive(true);
    }

    public void PlayRotateSound()
    {
        if (rotateSound != null)
        {
            rotateSound.Play();
        }
    }

}
