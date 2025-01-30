using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerBlood : MonoBehaviour
{

    private bool seHaActivado = false;

    public AudioClip[] clipsFirstRun;
    public AudioClip[] clipsResumen;


    public string[] linesFirstRun_EN;
    public string[] linesFirstRun_ES;
    public string[] linesFirstRun_CA;

    public string[] linesResumen_EN;
    public string[] linesResumen_ES;
    public string[] linesResumen_CA;



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
        if (other.CompareTag("Player") && !seHaActivado)
        {
            seHaActivado = true;
            if (!PuzzleManager.firstBloodDialogueDoned)
            {
                PuzzleManager.firstBloodDialogueDoned = true;
                SubtitulosManager.instance.PlayDialogue(linesFirstRun_ES, linesFirstRun_EN, linesFirstRun_CA, clipsFirstRun);
            }
            else
            {
                SubtitulosManager.instance.PlayDialogue(linesResumen_ES, linesResumen_EN, linesResumen_CA, clipsResumen);
            }
        }
    }

}
