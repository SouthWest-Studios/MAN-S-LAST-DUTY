using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class PeriodicPuzzle : MonoBehaviour
{
    public int correctNum;
    
    public TextMeshPro hintNumText;
    private int hintNum;

    public List<PrimerPuzzleButtons> buttons;

    public PuzzleManager puzzleManager;

    private void Awake()
    {
        
        {
            puzzleManager.LoadAllPuzzles();
        }
        
    }
    private void Start()
    {
        puzzleManager = FindAnyObjectByType<PuzzleManager>();
        correctNum = Random.Range(1, 119);
    }

    public void GuessNum(int num)
    {
        hintNum = num;
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
            puzzleManager = FindAnyObjectByType<PuzzleManager>();
            hintNumText.text = "correcto";
            puzzleManager.CompletePuzzle("PeriodicTablePuzzle");

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
