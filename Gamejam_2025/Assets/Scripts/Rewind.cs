using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind : MonoBehaviour
{
    public bool isRewinding = false;


    private Stack<Vector3> positionLists = new Stack<Vector3>();
    private Stack<Quaternion> rotationLists = new Stack<Quaternion>();

    public float timeBetweenSaves = 0.2f;
    private float timeContador = 0;

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
                rotationLists.Push(transform.rotation);
            }
            else
            {
                timeContador += Time.deltaTime;
            }
        }
        else
        {
            if(positionLists.Count > 0) transform.position = positionLists.Pop();
            if (rotationLists.Count > 0) transform.rotation = rotationLists.Pop();
        }
    }
}
