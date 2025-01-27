using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TangramManager : MonoBehaviour
{
    public List<SlotScript> slots; // Lista de todos los slots
    public int numberOfSlots = 5; // N�mero de slots en el juego
    public int maxMoleculeID = 15; // Rango m�ximo de IDs de mol�culas
    private List<int> correctCombination; // Lista de la combinaci�n correcta
    private PuzzleManager puzzleManager;
    bool allCorrect;
    public GameObject canvas;

    private void Start()
    {
        
    }

   

    private void Update()
    {
        if (allCorrect)
        {
            if(canvas.activeSelf)
            {
                puzzleManager = FindAnyObjectByType<PuzzleManager>();
                puzzleManager.CompletePuzzle("WordlePuzzle");
                return;
            }
        }
    }

    public void CheckCombination()
    {
         allCorrect = true;
        for (int i = 0; i < slots.Count; i++)
        {

            var molecule = slots[i].GetComponentInChildren<DraggableMolecule>();
          
            
            
        }
        if (allCorrect)
        {
            
        }
    }

    private void EnableMoleculeInteraction(DraggableMolecule molecule, bool enable)
    {
        // Aqu� puedes deshabilitar la interacci�n con las mol�culas que han sido marcadas como rojas
        var draggable = molecule.GetComponent<DraggableMolecule>();
        if (draggable != null)
        {
            draggable.enabled = enable; // Si 'enable' es false, deshabilita el script DraggableMolecule
            
            if (draggable.canvasGroup != null)
            {
                draggable.canvasGroup.blocksRaycasts = enable; // Si 'enable' es false, bloquea los raycasts para que no pueda ser arrastrada
            }
        }
    }
}
