using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasStarter : MonoBehaviour
{
    bool timeToActive = false;
    // Start is called before the first frame update
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!timeToActive)
        {
            this.gameObject.SetActive(false);
            timeToActive = true;
        }
        return;
    }


}
