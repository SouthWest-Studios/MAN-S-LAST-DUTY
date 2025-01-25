using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickChecker : MonoBehaviour
{
    private string puzzleName;
    public ObjectInteraction trigger;

    public PuzzleManager puzzleManager;
    // Start is called before the first frame update
    void Start()
    {
        puzzleManager = FindAnyObjectByType<PuzzleManager>();
        puzzleName = "WavePuzzle";
        foreach (var puzzle in puzzleManager.puzzles)
        {
            if (puzzle.name == puzzleName)
            {                
               if (puzzle.itHasbeenCompleted)
                  {
                    trigger.enabled = false;
                  }
                
                 

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
