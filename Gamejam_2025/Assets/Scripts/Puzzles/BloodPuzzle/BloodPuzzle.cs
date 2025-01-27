using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodPuzzle : MonoBehaviour
{
    public int correctNumber;

    public int currentNumber;

    public List<StickHelpers> sticks;

    private Quaternion inicialRotation;


    void Start()
    {
        correctNumber = Random.Range(1, 10); // 1 incluido, 10 excluido
        currentNumber = 1;
        inicialRotation = Quaternion.identity;
        sticks[0].GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    void Update()
    {
        
    }

    public void RotateButton()
    {
        
    
        for (int i = 1; i < sticks.Count + 1; i++)
        {

        }
        if (currentNumber > 8)
        {
            currentNumber = 1;
            this.transform.rotation = inicialRotation;

        }
        else
        {
            this.transform.Rotate(Vector3.forward, 23f);
            currentNumber++;
        }
        sticks[currentNumber].GetComponent<MeshRenderer>().material.color = Color.blue;


    }
    
    public void saveTry()
    {
        for(int i = 0; i < 9; i++)
        {
            sticks[i].stickColoring(currentNumber, correctNumber);
        }
    }
}
