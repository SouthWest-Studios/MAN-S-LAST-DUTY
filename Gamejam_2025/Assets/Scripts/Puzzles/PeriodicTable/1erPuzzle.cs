using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class PeriodicPuzzle : MonoBehaviour
{
    public static int correctNum;
    
    public TextMeshPro hintNumText;
    private int hintNum;

    public List<PrimerPuzzleButtons> buttons;

    private PuzzleManager puzzleManager;

    public GameObject finalObject;




    public AudioClip[] clip;


    public string[] lines_ES;
    public string[] lines_EN;
    public string[] lines_CA;

    private void Awake()
    {
        puzzleManager = FindObjectOfType<PuzzleManager>();
        {
            puzzleManager.LoadAllPuzzles();
        }
        
    }
    private void Start()
    {
        puzzleManager = FindAnyObjectByType<PuzzleManager>();
        correctNum = Random.Range(1, 56);
    }

    public void GuessNum(int num)
    {
        hintNum = num;
        hintNumText.text = "?";
    }
    public void ShowNum()
    {
        if (hintNum < correctNum)
        {
            hintNumText.text = "> " + hintNum;
        }
        if (hintNum > correctNum)
        {
            hintNumText.text = "< " + hintNum;
        }
        if (hintNum == correctNum)
        {


            SubtitulosManager.instance.PlayDialogue(lines_ES, lines_EN, lines_CA, clip);



            puzzleManager = FindAnyObjectByType<PuzzleManager>();
            hintNumText.text = "correct";
            puzzleManager.CompletePuzzle("PeriodicTablePuzzle");
            finalObject.SetActive(true);

        }
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].num == hintNum)
            {
                buttons[i].gameObject.GetComponent<Button>().interactable = false;
            }
        }

    }
}
