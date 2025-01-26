using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetusScript : MonoBehaviour
{
    public string currentObject;
    public string currentHint;
    private PuzzleManager manager;
    private WordleController wordleController;
   public void FetusInteract()
    {
        manager = FindAnyObjectByType<PuzzleManager>();
        wordleController = FindAnyObjectByType<WordleController>();

        if (currentObject != null)
        {            
          manager.GivePuzzle(currentObject);
            
        }

        if(currentHint != null)
        {
            if(currentHint == "WordlePuzzle")
            {
                wordleController.CheckCombination();
            }
            
        }
    }
}
