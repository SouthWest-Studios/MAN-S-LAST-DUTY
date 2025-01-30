using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodPuzzle : MonoBehaviour
{
    public int BloodRuedaID;
    public int correctNumber;

    public int currentNumber;

    public List<StickHelpers> sticks;

    private Quaternion inicialRotation;

    private List<Color> originalColor;

    private FetusScript fetusScript;

    public BloodManager manager;


    void Start()
    {
        
        correctNumber = Random.Range(1, 10); // 1 incluido, 10 excluido
        currentNumber = 0;
        inicialRotation = Quaternion.identity;


        originalColor = new List<Color>(sticks.Count);

        

        for (int i = 0; i < sticks.Count; i++)
        {
            // Accede al objeto correcto usando el �ndice i
            MeshRenderer renderer = sticks[i].gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                
                sticks[i].gameObject.GetComponent<Renderer>().material.color = Color.black;
                sticks[i].gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.blue;
                //sticks[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);

                originalColor.Add(renderer.material.color); // Agrega el color a la lista

            }
            else
            {
                Debug.LogWarning($"El objeto en sticks[{i}] no tiene un MeshRenderer.");
            }
        }
        sticks[0].gameObject.transform.GetChild(0).gameObject.SetActive(true);

       
    }

    void Update()
    {
        
    }

    public void RotateButton()
    {
        if (manager != null)
        {
            manager.PlayRotateSound();
        }
        
        if (currentNumber > 7)
        {
            currentNumber = 0;
            this.transform.rotation = Quaternion.identity;
            this.transform.localRotation = Quaternion.identity;

        }
        else
        {
            this.transform.Rotate(Vector3.forward, 23f);
            currentNumber++;
        }
        
        sticks[currentNumber].gameObject.transform.GetChild(0).gameObject.SetActive(true);

        for (int i = 0; i < sticks.Count; i++)
        {
            if (i != currentNumber)
            {
                sticks[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        
        
        
    }

    
    
    public void saveTry()
    {
        
        for (int i = 0; i < 9; i++)
        {
            
            sticks[i].stickColoring(currentNumber, correctNumber);

        }
        for (int i = 0; i < sticks.Count; i++)
        {
            
            MeshRenderer renderer = sticks[i].gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {

                originalColor[i] = renderer.material.color;

            }
            
            
        }


        
    }
}
