using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class PeriodicPuzzle : MonoBehaviour
{
    private int correctNum;
    
    public TextMeshPro hintNumText;
    private int hintNum;

    public List<PrimerPuzzleButtons> buttons;


    private void Start()
    {
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
            hintNumText.text = "correcto";
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
