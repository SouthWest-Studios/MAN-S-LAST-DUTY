using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class Puzzle
    {
        public string name;
        public bool isCompleted;
        public bool itHasbeenCompleted;
        public bool isReseteable;
    }

    public List<Puzzle> puzzles = new List<Puzzle>();

    private static PuzzleManager instance;

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
    }

    private void Start()
    {
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

    public void CompletePuzzle(string puzzleName)
    {
        foreach (var puzzle in puzzles)
        {
            if (puzzle.name == puzzleName)
            {
                puzzle.isCompleted = true;
                puzzle.itHasbeenCompleted = true;

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
        foreach (var puzzle in puzzles)
        {
            // Guardar el estado de cada puzzle en PlayerPrefs
            PlayerPrefs.SetInt($"{puzzle.name}_isCompleted", puzzle.isCompleted ? 1 : 0);
            PlayerPrefs.SetInt($"{puzzle.name}_itHasBeenCompleted", puzzle.itHasbeenCompleted ? 1 : 0);
            PlayerPrefs.SetInt($"{puzzle.name}_isReseteable", puzzle.isReseteable ? 1 : 0);
        }

        // Asegúrate de guardar los cambios
        PlayerPrefs.Save();

        Debug.Log("Todos los puzzles han sido guardados.");
    }

    public void LoadAllPuzzles()
    {
        foreach (var puzzle in puzzles)
        {
            // Recuperar el estado de cada puzzle desde PlayerPrefs
            puzzle.isCompleted = PlayerPrefs.GetInt($"{puzzle.name}_isCompleted", 0) == 1;
            puzzle.itHasbeenCompleted = PlayerPrefs.GetInt($"{puzzle.name}_itHasBeenCompleted", 0) == 1;
            puzzle.isReseteable = PlayerPrefs.GetInt($"{puzzle.name}_isReseteable", 0) == 1;
        }

        Debug.Log("Todos los puzzles han sido cargados.");
    }

}
