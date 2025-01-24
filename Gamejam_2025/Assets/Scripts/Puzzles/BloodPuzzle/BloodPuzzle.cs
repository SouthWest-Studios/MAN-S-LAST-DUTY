using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodPuzzle : MonoBehaviour
{
    public int correctNumber;

    public int currentNumber;

    public List<StickHelpers> sticks;

    void Start()
    {
        correctNumber = Random.Range(1, 10); // 1 incluido, 10 excluido
        currentNumber = 1;
    }

    void Update()
    {
        
    }

    public void RotateButton()
    {
        if (currentNumber > 8)
        {
            currentNumber = 1;
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
