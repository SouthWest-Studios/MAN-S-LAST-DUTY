using System.Collections.Generic;
using TMPro;
using UnityEngine;





[System.Serializable]
public class Puzzle
{
    public string name;
    public bool isCompleted = false;
    public bool itHasbeenCompleted = false;
    public bool isReseteable = false;
    public bool isGiven = false;
    public int seed;
}
public class PuzzleManager : MonoBehaviour
{
    

    public List<Puzzle> puzzles = new List<Puzzle>();
    private static List<Puzzle> persistentPuzzles = null;
    public static string numpadFinalCode = "1234";
    public static string numpadActualCode = "****";

    [Header("UI Elements")]
    public TextMeshProUGUI completionMessage; // Referencia al objeto del mensaje
    public float messageDuration = 2f;   // Duración del mensaje en segundos

    public static PuzzleManager instance;

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
                if (!puzzle.itHasbeenCompleted)
                {
                    NotasMentalesManager.instance.ShowDrawIcon();
                }
                puzzle.itHasbeenCompleted = true;

                if(puzzle.isReseteable == true)
                {
                    fetus = FindAnyObjectByType<FetusScript>();
                    fetus.currentObject = puzzleName;
                }
                
                ShowCompletionMessage("PuzzleCompletado");
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
                ShowCompletionMessage("Puzzle Entregado");
                return;
            }
        }

    }

    public void ShowCompletionMessage(string message)
    {
        if (completionMessage != null)
        {
            completionMessage.text = message;
            completionMessage.gameObject.SetActive(true); // Muestra el mensaje
            // Inicia una animación o desvanecimiento
            StartCoroutine(HideMessageAfterDelay());
        }
    }

    private System.Collections.IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        if (completionMessage != null)
        {
            completionMessage.gameObject.SetActive(false); // Oculta el mensaje después del tiempo
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

    public Puzzle GetPuzzle(string puzzleName)
    {
        foreach (var puzzle in puzzles)
        {
            if (puzzle.name == puzzleName)
                return puzzle;
        }
        return null;
    }


    public void SaveGame()
    {
        PlayerPrefs.SetInt("PuzzleCount", puzzles.Count);
        PlayerPrefs.SetString("umpadFinalCode", numpadFinalCode);

        for (int i = 0; i < puzzles.Count; i++)
        {
            PlayerPrefs.SetString($"Puzzle_{i}_name", puzzles[i].name);
            PlayerPrefs.SetInt($"Puzzle_{i}_isCompleted", puzzles[i].isCompleted ? 1 : 0);
            PlayerPrefs.SetInt($"Puzzle_{i}_itHasbeenCompleted", puzzles[i].itHasbeenCompleted ? 1 : 0);
            PlayerPrefs.SetInt($"Puzzle_{i}_isReseteable", puzzles[i].isReseteable ? 1 : 0);
            PlayerPrefs.SetInt($"Puzzle_{i}_isGiven", puzzles[i].isGiven ? 1 : 0);
            PlayerPrefs.SetInt($"Puzzle_{i}_seed", puzzles[i].seed);
        }

        PlayerPrefs.Save();
        Debug.Log("Juego guardado con PlayerPrefs.");
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("PuzzleCount"))
        {
            Debug.LogWarning("No hay datos de guardado.");
            return;
        }

        numpadFinalCode = PlayerPrefs.GetString("numpadFinalCode");

        int puzzleCount = PlayerPrefs.GetInt("PuzzleCount");
        puzzles.Clear();

        for (int i = 0; i < puzzleCount; i++)
        {
            Puzzle puzzle = new Puzzle
            {
                name = PlayerPrefs.GetString($"Puzzle_{i}_name", ""),
                isCompleted = PlayerPrefs.GetInt($"Puzzle_{i}_isCompleted", 0) == 1,
                itHasbeenCompleted = PlayerPrefs.GetInt($"Puzzle_{i}_itHasbeenCompleted", 0) == 1,
                isReseteable = PlayerPrefs.GetInt($"Puzzle_{i}_isReseteable", 0) == 1,
                isGiven = PlayerPrefs.GetInt($"Puzzle_{i}_isGiven", 0) == 1,
                seed = PlayerPrefs.GetInt($"Puzzle_{i}_seed", 0)
            };

            puzzles.Add(puzzle);
        }

        Debug.Log("Juego cargado desde PlayerPrefs.");
    }

    public void NewGame()
    {
        int seed = (int)System.DateTime.Now.Ticks; // Usa el timestamp actual como semilla
        Random.InitState(seed);

        int randomCode = Random.Range(0, 10000); // Genera un número entre 0 y 9999
        numpadFinalCode = randomCode.ToString("D4"); // Convierte a string con 4 dígitos (rellenando con ceros si es necesario)

        foreach (var puzzle in puzzles)
        {
            puzzle.isCompleted = false;
            puzzle.itHasbeenCompleted = false;
            puzzle.isGiven = false;
            puzzle.seed = Random.Range(0, 100000); // Genera una semilla aleatoria
        }

        SaveGame(); // Guarda el estado inicial
        Debug.Log("Nueva partida iniciada.");
    }

}
