using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetusScript : MonoBehaviour
{
    public string currentObject;
    public string currentHint;
    private PuzzleManager manager;
    private WordleController wordleController;
    private CordonUmbilical cordonUmbilical;
   public void FetusInteract()
    {
        Debug.Log("fetusInteract");
        manager = FindAnyObjectByType<PuzzleManager>();
        wordleController = FindAnyObjectByType<WordleController>();
        cordonUmbilical = FindAnyObjectByType<CordonUmbilical>();

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
            if(currentHint == "UmbilicalCord")
            {
                cordonUmbilical.CheckPuzzle();
            }
            
        }
    }
}
