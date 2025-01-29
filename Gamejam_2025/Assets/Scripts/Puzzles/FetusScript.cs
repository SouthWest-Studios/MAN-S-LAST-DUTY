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
    private GrabObjects grabObjects; 

    private bool showHints = true;


    void Start()
    {
        grabObjects = FindObjectOfType<GrabObjects>();
        
    }

    private void Update()
    {
        if (showHints)
        {
            ShowHints();
            showHints = false;
        }
    }

    public void FetusInteract()
    {
        Debug.Log("fetusInteract");
        manager = FindAnyObjectByType<PuzzleManager>();
        wordleController = FindAnyObjectByType<WordleController>();
        cordonUmbilical = FindAnyObjectByType<CordonUmbilical>();
        bloodManager = FindAnyObjectByType<BloodManager>();

        // Si hay un objeto siendo sostenido, lo destruimos
        
        if (currentObject != "")
        {
          manager.GivePuzzle(currentObject);           
        }

        if(currentHint != null)
        {

            if(currentHint == "WordlePuzzle")
            {
                
            }
            if(currentHint == "UmbilicalCord")
            {
                cordonUmbilical.CheckPuzzle();
            }
            if (currentHint == "BloodPuzzle")
            {
                for(int i = 0; i <= 2; i++)
                {
                    BloodManager.blood[i].saveTry();
                    bloodManager.CheckWin();
                }
                
            }
            manager.GiveHint(currentHint);
        }

        if (grabObjects != null && grabObjects.GetHeldObject() != null)
        {
            Destroy(grabObjects.GetHeldObject());
            grabObjects.ForceDropObject();
        }

    }

    public void ShowHints()
    {
        manager = FindAnyObjectByType<PuzzleManager>();

        for (int i = 0; i < manager.puzzles.Count; i++)
        {
            if (manager.puzzles[i].isHintGiven && manager.puzzles[i].name == "WordlePuzzle")
            {
                wordleController = FindAnyObjectByType<WordleController>();
                wordleController.LoadMolecules();
                manager.puzzles[i].isHintGiven = false;
            }
        }


    }
}
