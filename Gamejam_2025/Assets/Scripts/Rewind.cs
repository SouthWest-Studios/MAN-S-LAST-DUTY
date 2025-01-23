using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rewind : MonoBehaviour
{
    public bool isRewinding = false;

    public Transform cameraTransform;


    private Stack<Vector3> positionLists = new Stack<Vector3>();
    private Stack<Quaternion> rotationLists = new Stack<Quaternion>();


    public float timeBetweenSaves = 0.2f;
    public float smoothTime = 0.1f;
    private float timeContador = 0;

    private int minuteCounter = 60;
    private float secondCounter = 0f;
    public TextMeshProUGUI minuteText;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRewinding)
        {
            
            if(timeContador > timeBetweenSaves)
            {
                timeContador = 0;
                positionLists.Push(transform.position);
                rotationLists.Push(cameraTransform.rotation);
                
            }
            else
            {
                timeContador += Time.deltaTime;
            }

            secondCounter += Time.deltaTime;

            if (secondCounter >= 1f)
            {
                minuteCounter--;
                secondCounter = 0f;
            }

            

            if (minuteCounter <= 0)
            {
                isRewinding = true;
                minuteCounter = 0;
            }
        }
        else
        {
            if(timeContador > smoothTime)
            {
                timeContador = 0;
                smoothTime -= 0.01f;
                if (positionLists.Count > 0) transform.position = positionLists.Pop();
                if (rotationLists.Count > 0) cameraTransform.rotation = rotationLists.Pop();
            }
            else
            {
                timeContador += Time.deltaTime;
            }

            if (positionLists.Count <= 0 && rotationLists.Count <= 0)
            {
                isRewinding = false;
                minuteCounter = 60;
            }

        }
        
        minuteText.text = "" + minuteCounter;
    }


}
