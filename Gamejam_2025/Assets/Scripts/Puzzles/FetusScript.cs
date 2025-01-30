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



    public AudioClip[] clipsDialogo1;

    public string[] linesDialogo1_ES;
    public string[] linesDialogo1_EN;
    public string[] linesDialogo1_CA;



    public AudioClip[] clipsDialogoFinal;

    public string[] linesDialogo1_ES_FINAL;
    public string[] linesDialogo1_EN_FINAL;
    public string[] linesDialogo1_CA_FINAL;


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


        SubtitulosManager.instance.PlayDialogue(linesDialogo1_ES, linesDialogo1_EN, linesDialogo1_CA, clipsDialogo1);

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
            manager.GiveHint(currentHint);
        }

        if (grabObjects != null && grabObjects.GetHeldObject() != null)
        {
            Destroy(grabObjects.GetHeldObject());
            grabObjects.ForceDropObject();
        }

        int counter = 0;

        for (int i = 0; i < manager.puzzles.Count; i++)
        {
            if (manager.puzzles[i].isCompleted == true && manager.puzzles[i].isPuzzleGiven)
            {
                counter++;
            }
        }

        if (counter >= 5)
        {

            SubtitulosManager.instance.PlayDialogue(linesDialogo1_ES_FINAL, linesDialogo1_EN_FINAL, linesDialogo1_CA_FINAL, clipsDialogoFinal);
            TransicionFinal.instance.PlayTransicionFinal();


            Debug.Log("VICTORIA");
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
            if (manager.puzzles[i].isHintGiven && manager.puzzles[i].name == "CordonUmbilicalPuzzle")
            {
                cordonUmbilical = FindAnyObjectByType<CordonUmbilical>();
                cordonUmbilical.CheckPuzzle();
                manager.puzzles[i].isHintGiven = false;
            }
            if (manager.puzzles[i].isHintGiven && manager.puzzles[i].name == "BloodPuzzle")
            {
                bloodManager = FindAnyObjectByType<BloodManager>();
                bloodManager.LoadBlood();
                manager.puzzles[i].isHintGiven = false;
            }
        }

        
    }
}
