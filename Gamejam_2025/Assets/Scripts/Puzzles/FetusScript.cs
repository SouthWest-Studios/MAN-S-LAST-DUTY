using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetusScript : MonoBehaviour
{
    public string currentObject = null;
    public string currentHint;
    private PuzzleManager manager;
    private WordleController wordleController;
    private CordonUmbilical cordonUmbilical;
    private BloodManager bloodManager;
   public void FetusInteract()
    {
        Debug.Log("fetusInteract");
        manager = FindAnyObjectByType<PuzzleManager>();
        wordleController = FindAnyObjectByType<WordleController>();
        cordonUmbilical = FindAnyObjectByType<CordonUmbilical>();
        bloodManager = FindAnyObjectByType<BloodManager>();

        if (currentObject != "")
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
            if (currentHint == "BloodPuzzle")
            {
                for(int i = 0; i <= 2; i++)
                {
                    bloodManager.blood[i].saveTry();
                    bloodManager.CheckWin();
                }
                
            }

        }
    }
}
