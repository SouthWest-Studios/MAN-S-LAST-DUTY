using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogosIniciales : MonoBehaviour
{


    public AudioClip[] clipsFirstRun;
    public AudioClip[] clipsVolverPrueba;
    public AudioClip[] clipsResumen;
    public AudioClip[] clipsPuzleCompletado;

    
    public string[] linesFirstRun_EN;
    public string[] linesFirstRun_ES;
    public string[] linesFirstRun_CA;

    public string[] linesVolverPruebas_EN;
    public string[] linesVolverPruebas_ES;
    public string[] linesVolverPruebas_CA;

    public string[] linesResumen_EN;
    public string[] linesResumen_ES;
    public string[] linesResumen_CA;

    public string[] linesCompletarPuzzle_EN;
    public string[] linesCompletarPuzzle_ES;
    public string[] linesCompletarPuzzle_CA;


    // Start is called before the first frame update
    void Start()
    {
        if(PuzzleManager.instance.idiomaIndex == 0)
        {
            if (!PuzzleManager.firstRunDoned)
            {
                SubtitulosManager.instance.PlayDialogue(linesFirstRun_ES, clipsFirstRun);
                PuzzleManager.firstRunDoned = true;
            }
            else
            {
                SubtitulosManager.instance.PlayDialogue(linesResumen_ES, clipsResumen);
            }
        }else if (PuzzleManager.instance.idiomaIndex == 1)
        {
            if (!PuzzleManager.firstRunDoned)
            {
                SubtitulosManager.instance.PlayDialogue(linesFirstRun_EN, clipsFirstRun);
                PuzzleManager.firstRunDoned = true;
            }
            else
            {
                SubtitulosManager.instance.PlayDialogue(linesResumen_EN, clipsResumen);
            }
        }
        else if (PuzzleManager.instance.idiomaIndex == 2)
        {
            if (!PuzzleManager.firstRunDoned)
            {
                SubtitulosManager.instance.PlayDialogue(linesFirstRun_CA, clipsFirstRun);
                PuzzleManager.firstRunDoned = true;
            }
            else
            {
                SubtitulosManager.instance.PlayDialogue(linesResumen_CA, clipsResumen);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
