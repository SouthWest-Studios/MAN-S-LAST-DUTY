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

        originalColor = new List<Color>(sticks.Count);

        

        for (int i = 0; i < sticks.Count; i++)
        {
            // Accede al objeto correcto usando el índice i
            MeshRenderer renderer = sticks[i].gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                
                    sticks[i].gameObject.GetComponent<Renderer>().material.color = Color.black;
                    
                
                originalColor.Add(renderer.material.color); // Agrega el color a la lista

            }
            else
            {
                Debug.LogWarning($"El objeto en sticks[{i}] no tiene un MeshRenderer.");
            }
        }
        sticks[0].gameObject.GetComponent<Renderer>().material.color = Color.blue;
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
        
        sticks[currentNumber].GetComponent<Renderer>().material.color = Color.blue;
        for (int i = 0; i < sticks.Count; i++)
        {
            if (i != currentNumber)
            {
                sticks[i].GetComponent<Renderer>().material.color = originalColor[i];
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
