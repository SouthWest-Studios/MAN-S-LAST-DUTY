using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class Puzzle
    {
        public string name; 
        public bool isCompleted; 
        public bool isReseteable;
    }

    public List<Puzzle> puzzles = new List<Puzzle>(); 

   
    public void CompletePuzzle(string puzzleName)
    {
        foreach (var puzzle in puzzles)
        {
            if (puzzle.name == puzzleName)
            {
                puzzle.isCompleted = true;
                Debug.Log($"Puzzle '{puzzleName}' completado!");
                return;
            }
        }
        Debug.LogWarning($"Puzzle '{puzzleName}' no encontrado.");
    }

    
    public bool AreAllPuzzlesCompleted()
    {
        foreach (var puzzle in puzzles)
        {
            if (!puzzle.isCompleted)
                return false;
        }
        return true;
    }

    
    public void ResetAllPuzzles()
    {
        foreach (var puzzle in puzzles)
        {
            if(puzzle.isReseteable)
            {
                puzzle.isCompleted = false;
            }
            
        }
        Debug.Log("Todos los puzzles han sido reiniciados.");
    }

    
    public bool IsPuzzleCompleted(string puzzleName)
    {
        foreach (var puzzle in puzzles)
        {
            if (puzzle.name == puzzleName)
                return puzzle.isCompleted;
        }
        Debug.LogWarning($"Puzzle '{puzzleName}' no encontrado.");
        return false;
    }
}
