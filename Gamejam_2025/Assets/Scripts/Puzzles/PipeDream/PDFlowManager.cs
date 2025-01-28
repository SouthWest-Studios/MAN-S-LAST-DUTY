using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PDFlowManager : MonoBehaviour
{
    public GameObject waterPrefab; // Prefab de agua
    public static PDFlowManager instance;
    public float flowSpeed = 5f; // Velocidad del flujo, ajustable desde el Inspector
    public int totalPoints = 8;
    public Image timer;
    public bool winPipes = false;
    public ObjectInteraction objectInteraction;

    private Puzzle puzzle;
    private int seed;

    private PuzzleManager puzzleManager;

    public TextMeshPro finalText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        timer.fillAmount = 0;
    }

    private void Start()
    {
        puzzle = PuzzleManager.instance.GetPuzzle("PipeDreamPuzzle");
        if (puzzle != null)
        {
            seed = puzzle.seed;
            Random.InitState(seed);
        }
    }

    public void StartFlow(int startX, int startY)
    {
        StartCoroutine(FlowRoutine(startX, startY));
    }

    private IEnumerator FlowRoutine(int x, int y, int lastX = -1, int lastY = -1)
    {
        var currentCell = PDGridManager.instance.GetPipeAt(x, y);

        if (currentCell == null || currentCell.pipe == null) yield break;

        // Calcular la dirección de entrada
        int entryDirection = -1;
        if (lastX != -1 && lastY != -1)
        {
            if (lastX < x) entryDirection = 2; // Viene desde arriba
            else if (lastX > x) entryDirection = 0; // Viene desde abajo
            else if (lastY < y) entryDirection = 3; // Viene desde la izquierda
            else if (lastY > y) entryDirection = 1; // Viene desde la derecha
        }

        // Reproducir animación de llenado
        currentCell.FillPipeWithAnimation(entryDirection);
        timer.fillAmount += (1.0f / totalPoints);

        if (timer.fillAmount >= 0.98f)
        {
            objectInteraction.EndFocusTransition();
            winPipes = true;
            puzzleManager = FindAnyObjectByType<PuzzleManager>();
            puzzleManager.CompletePuzzle("PipeDreamPuzzle");

            int indexCode = 1;
            finalText.gameObject.SetActive(true);
            finalText.text = "*" + PuzzleManager.numpadFinalCode[indexCode].ToString() + "**";
            
            char[] auxList =  PuzzleManager.numpadActualCode.ToCharArray();
            auxList[indexCode] = PuzzleManager.numpadFinalCode[indexCode];
            string finalCharacters = "";
            for (int i = 0; i < auxList.Length; i++)
            {
                finalCharacters += auxList[i].ToString();
            }
            PuzzleManager.numpadActualCode = finalCharacters;
            
        }
        else
        {
            yield return new WaitForSeconds(flowSpeed);

            for (int direction = 0; direction < 4; direction++)
            {
                if (!currentCell.pipe.connections[direction]) continue;

                int nextX = x, nextY = y;
                switch (direction)
                {
                    case 0: nextX -= 1; break; // Arriba
                    case 1: nextY += 1; break; // Derecha
                    case 2: nextX += 1; break; // Abajo
                    case 3: nextY -= 1; break; // Izquierda
                }

                var nextCell = PDGridManager.instance.GetPipeAt(nextX, nextY);

                if (nextCell != null && nextCell.pipe != null)
                {
                    if (currentCell.pipe.IsConnectedTo(nextCell.pipe, direction) && !nextCell.pipe.isFilled)
                    {
                        yield return StartCoroutine(FlowRoutine(nextX, nextY, x, y));
                    }
                }
            }

            if (!winPipes)
            {
                Debug.Log("FIN DE LAS TUBERIAS :c");
                ResetGame(); // Reinicia el minijuego
            }
        }
    }

    public void ResetGame()
    {
        foreach (Transform child in PDGridManager.instance.gameplayGrid)
        {
            Destroy(child.gameObject);
        }
        StopAllCoroutines();
        Random.InitState(seed);

        PDPipePreview.instance.ResetPreview();
        PDGridManager.instance.ResetGrid();

        winPipes = false;
        timer.fillAmount = 0;

        PDGridManager.instance.GenerateGrid();
        Debug.Log("El juego ha sido reiniciado.");
    }
}
