using UnityEditor.UI;
using UnityEngine;

public class PDGridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform gameplayGrid;
    public PDPipeType startPipe;

    public static PDGridManager instance;

    public int rows = 5, cols = 10;
    private PDPipeCell[,] grid;

    private Puzzle puzzle;
    private int seed = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {

        puzzle = PuzzleManager.instance.GetPuzzle("PipeDreamPuzzle");
        if (puzzle != null)
        {
            seed = puzzle.seed;

            Random.InitState(seed);
        }
       
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        Random.InitState(seed);
        grid = new PDPipeCell[rows, cols];
        int numRow = 0, numCol = 0;

        for (int i = 0; i < rows * cols; i++)
        {
            GameObject cell = Instantiate(cellPrefab, gameplayGrid);
            cell.GetComponent<PDPipeCell>().position = new Vector2(numRow, numCol);
            grid[numRow, numCol] = cell.GetComponent<PDPipeCell>();
            numCol++;
            if (numCol >= cols)
            {
                numRow++;
                numCol = 0;
            }
        }

        int randomRow = Random.Range(rows - 3, rows - 1);
        int randomCol = Random.Range(2, cols-2);

        Debug.Log(randomRow + ", " + randomCol);

        GetPipeAt(randomRow, randomCol).pipe = startPipe;
        GetPipeAt(randomRow, randomCol).SetPipe();

        PDFlowManager.instance.StartFlow(randomRow, randomCol);
    }

    public PDPipeCell GetPipeAt(int x, int y)
    {
        if (x < 0 || x >= rows || y < 0 || y >= cols) return null;
        return grid[x, y];
    }

    public void ResetGrid()
    {
        int numRow = 0, numCol = 0;
        for (int i = 0; i < rows * cols; i++)
        {

            Destroy(grid[numRow, numCol].gameObject);
            numCol++;
            if (numCol >= cols)
            {
                numRow++;
                numCol = 0;
            }
        }
    }
}
