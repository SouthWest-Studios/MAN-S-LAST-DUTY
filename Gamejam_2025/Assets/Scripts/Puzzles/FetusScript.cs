using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetusScript : MonoBehaviour
{
    public string currentObject;
    private PuzzleManager manager;
   public void FetusInteract()
    {
        manager = FindAnyObjectByType<PuzzleManager>();

        if (currentObject != null)
        {            
          manager.GivePuzzle(currentObject);
            
        }
    }
}
