using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickHelpers : MonoBehaviour
{
    public int position;

    
    // Start is called before the first frame update
    public void stickColoring(int num, int correctNum)
    {
        if(Mathf.Abs(num - position) <= 1)
        {
            if (GetComponent<Renderer>() != null)
            {
                if (correctNum == position)
                {
                    GetComponent<Renderer>().material.color = Color.green;
                }
                else if (Mathf.Abs(correctNum - position) == 1)
                {
                    GetComponent<Renderer>().material.color = new Color(0.7f, 0.9f, 0f); // Color lime
                }
                else if (Mathf.Abs(correctNum - position) == 2)
                {
                    GetComponent<Renderer>().material.color = Color.yellow;
                }
                else if (Mathf.Abs(correctNum - position) == 3)
                {
                    GetComponent<Renderer>().material.color = new Color(1f, 0.647f, 0f); // Color orange
                }
                else if (Mathf.Abs(correctNum - position) == 4)
                {
                    GetComponent<Renderer>().material.color = Color.red;
                }
                else if (Mathf.Abs(correctNum - position) >= 5) 
                {
                    GetComponent<Renderer>().material.color = new Color(0, 0, 0); // Color violeta
                }
            }
            else
            {
                Debug.LogWarning("El objeto no tiene un componente Renderer.");
            }
        }
        

        
    }

}
