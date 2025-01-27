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

    private List<Color> originalColor; 


    void Start()
    {
        correctNumber = Random.Range(1, 10); // 1 incluido, 10 excluido
        currentNumber = 0;
        inicialRotation = Quaternion.identity;
        sticks[0].GetComponent<MeshRenderer>().material.color = Color.blue;
        for (int i = 0; i < sticks.Count; i++)
        {
            originalColor[i] = sticks[0].gameObject.GetComponent<MeshRenderer>().material.color;
        }
    }

    void Update()
    {
        
    }

    public void RotateButton()
    {
        
    
        
        if (currentNumber > 7)
        {
            currentNumber = 0;
            this.transform.rotation = inicialRotation;

        }
        else
        {
            this.transform.Rotate(Vector3.forward, 23f);
            currentNumber++;
        }
        sticks[currentNumber].GetComponent<MeshRenderer>().material.color = Color.blue;
        for (int i = 0; i < sticks.Count + 0; i++)
        {
            if(i != currentNumber - 1)
            {
                sticks[currentNumber].GetComponent<MeshRenderer>().material.color = originalColor[i];
            }
        }


    }
    
    public void saveTry()
    {
        for(int i = 0; i < 8; i++)
        {
            sticks[i].stickColoring(currentNumber, correctNumber);
        }
    }
}
