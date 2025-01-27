using System.Collections;
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        timer.fillAmount = 0;
    }

    public void StartFlow(int startX, int startY)
    {
        StartCoroutine(FlowRoutine(startX, startY));
    }

    private IEnumerator FlowRoutine(int x, int y)
    {
        var currentCell = PDGridManager.instance.GetPipeAt(x, y);

        if (currentCell == null || currentCell.pipe == null) yield break;

        currentCell.FillPipe();
        timer.fillAmount += (1.0f / totalPoints);
        if(timer.fillAmount >= 0.98)
        {
            //Terminar
            winPipes = true;
        }
        else
        {
            yield return new WaitForSeconds(flowSpeed);

            if (waterPrefab != null)
            {
                GameObject water = Instantiate(waterPrefab, currentCell.transform);
                water.transform.localPosition = Vector3.zero;
            }

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

                //Debug.Log("Curr:" + currentCell.position + "Dir: " + currentCell.pipe.connections + "Next: " + nextCell.position + " Dir: " + nextCell.pipe.connections);

                if (nextCell != null && nextCell.pipe != null)
                {
                    if (currentCell.pipe.IsConnectedTo(nextCell.pipe, direction) && !nextCell.pipe.isFilled)
                    {
                        yield return StartCoroutine(FlowRoutine(nextX, nextY));
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
        // Reiniciar la cuadrícula eliminando todos los hijos del grid
        foreach (Transform child in PDGridManager.instance.gameplayGrid)
        {
            Destroy(child.gameObject);
        }

        // Reiniciar PipePreview
        PDPipePreview.instance.ResetPreview();
        PDGridManager.instance.ResetGrid();

        // Reiniciar valores globales
        winPipes = false;
        timer.fillAmount = 0;

        // Regenerar la cuadrícula
        PDGridManager.instance.GenerateGrid();

        Debug.Log("El juego ha sido reiniciado.");
    }

}
