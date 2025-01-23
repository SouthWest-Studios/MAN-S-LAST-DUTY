using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodPuzzle : MonoBehaviour
{
    int correctNumber;

    int currentNumber = 0;

    public List<StickHelpers> sticks;

    void Start()
    {
        correctNumber = Random.Range(0, 9); // 0 incluido, 9 excluido

    }

    void Update()
    {
        
    }

    public void RotateButton()
    {
        if (currentNumber > 7)
        {
            currentNumber = 0;
            this.transform.rotation = Quaternion.Euler(90.73801f, 0f, 0f);

        }
        else
        {
            this.transform.Rotate(Vector3.forward, -10f);
            currentNumber++;
        }
        

    }
    
    public void saveTry()
    {
        for(int i = 0; i < 9; i++)
        {
            sticks[i].stickColoring(currentNumber, correctNumber);
        }
    }
}
