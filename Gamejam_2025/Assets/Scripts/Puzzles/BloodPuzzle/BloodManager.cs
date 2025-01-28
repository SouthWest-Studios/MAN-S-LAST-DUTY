using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodManager : MonoBehaviour
{

    private PuzzleManager puzzleManager;
    public List<BloodPuzzle> blood;
    private FetusScript fetusScript;
    public GameObject bloodTube;
    void Start()
    {
        
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
            // Lógica de CheckWin después de la espera
            PuzzleManager puzzleManager = FindAnyObjectByType<PuzzleManager>();
            if (puzzleManager != null)
            {
                puzzleManager.CompletePuzzle("BloodPuzzle");
            }

            
        }
        
    }

    

    public void ShowTube()
    {
        StartCoroutine(DelayedShowTube());
    }

    private IEnumerator DelayedShowTube()
    {
        // Espera 30 segundos
        //GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(30f);
        FetusScript fetusScript = FindAnyObjectByType<FetusScript>();
        if (fetusScript != null)
        {
            fetusScript.currentHint = "BloodPuzzle";
        }


        bloodTube.SetActive(true);
    }


}
