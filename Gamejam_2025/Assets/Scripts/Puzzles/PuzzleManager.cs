using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class Puzzle
    {
        public string name;
        public bool isCompleted = false;
        public bool itHasbeenCompleted = false;
        public bool isReseteable = false;
        public bool isGiven = false;
    }

    public List<Puzzle> puzzles = new List<Puzzle>();
    private static List<Puzzle> persistentPuzzles = null;

    [Header("UI Elements")]
    public GameObject completionMessage; // Referencia al objeto del mensaje
    public float messageDuration = 2f;   // Duración del mensaje en segundos

    private static PuzzleManager instance;

    private FetusScript fetus;

    void Awake()
    {
        // Asegurarse de que solo exista una instancia de PuzzleManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evita que el GameObject se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruye el duplicado si ya existe una instancia
        }
        foreach (var puzzle in puzzles)
        {
            if (puzzle.name == "BloodPuzzle")
            {
                puzzle.isCompleted = false;
                puzzle.itHasbeenCompleted = false;
                puzzle.isReseteable = true;

            }
            if (puzzle.name == "PeriodicTablePuzzle")
            {
                puzzle.isCompleted = false;
                puzzle.itHasbeenCompleted = false;
                puzzle.isReseteable = true;

            }
            if (puzzle.name == "WavePuzzle")
            {
                puzzle.isCompleted = false;
                puzzle.itHasbeenCompleted = false;
                puzzle.isReseteable = false;

            }
            if (puzzle.name == "GuitarHeroPuzzle")
            {
                puzzle.isCompleted = false;
                puzzle.itHasbeenCompleted = false;
                puzzle.isReseteable = false;

            }
        }
    }

    private void Start()
    {
        
    }

    public void CompletePuzzle(string puzzleName)
    {
        foreach (var puzzle in puzzles)
        {
            if (puzzle.name == puzzleName)
            {
                puzzle.isCompleted = true;
                puzzle.itHasbeenCompleted = true;

                if(puzzle.isReseteable == true)
                {
                    fetus = FindAnyObjectByType<FetusScript>();
                    fetus.currentObject = puzzleName;
                }
                
                ShowCompletionMessage();
                return;
            }
        }
        
    }

    public void GivePuzzle(string puzzleName)
    {
        foreach (var puzzle in puzzles)
        {
            if (puzzle.name == puzzleName)
            {
                puzzle.isGiven = true;
                ShowCompletionMessage();
                return;
            }
        }

    }

    public void ShowCompletionMessage()
    {
        if (completionMessage != null)
        {
            completionMessage.SetActive(true); // Muestra el mensaje
            // Inicia una animación o desvanecimiento
            StartCoroutine(HideMessageAfterDelay());
        }
    }

    private System.Collections.IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        if (completionMessage != null)
        {
            completionMessage.SetActive(false); // Oculta el mensaje después del tiempo
        }
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
            if (puzzle.isReseteable)
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

    public void SaveAllPuzzles()
    {
        persistentPuzzles = new List<Puzzle>(puzzles);


    }

    public void LoadAllPuzzles()
    {
        if( persistentPuzzles != null ){
            puzzles = new List<Puzzle>(persistentPuzzles);
        }
        


    }

    public List<Puzzle> GetCopyList()
    {
        return persistentPuzzles;
    }

}
