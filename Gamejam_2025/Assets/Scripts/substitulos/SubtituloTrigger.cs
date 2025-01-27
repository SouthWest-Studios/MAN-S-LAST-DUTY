using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtituloTrigger : MonoBehaviour
{
    public string[] lines;
    public AudioClip[] clips;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        SubtitulosManager.instance.PlayDialogue(lines, clips);
    }
}
