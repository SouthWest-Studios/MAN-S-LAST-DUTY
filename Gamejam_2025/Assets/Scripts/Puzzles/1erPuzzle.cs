using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    private int correctNum;
    
    public TextMeshPro hintNum;


    private void Start()
    {
        correctNum = Random.Range(1, 119);
    }

    public void GuessNum(int num)
    {
        if(num < correctNum)
        {
            hintNum.text = "> " + num;
        }
        if (num > correctNum)
        {
            hintNum.text = "< " + num;
        }
        if(num == correctNum)
        {
            hintNum.text = "correcto";
        }

    }
}
