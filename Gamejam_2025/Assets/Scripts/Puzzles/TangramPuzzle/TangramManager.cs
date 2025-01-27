using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TangramManager : MonoBehaviour
{
    public List<SlotTangramScript> slots; // Lista de todos los slots
    public int numberOfSlots = 7; // N�mero de slots en el juego
    public int maxMoleculeID = 7; // Rango m�ximo de IDs de mol�culas
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
            return;
        }
        bool check = true;
        for (int i = 0; i < slots.Count; i++)
        {

           if(slots[i].GetComponentInChildren<DraggableTangram>() == null)
            {
                check = false;
            }
        }
        if (check)
        {
            CheckCombination();
        }
        
    }

    public void CheckCombination()
    {
         allCorrect = true;
        for (int i = 0; i < slots.Count; i++)
        {

            var molecule = slots[i].GetComponentInChildren<DraggableTangram>();

            if(molecule.pieceID == 1 && molecule.pieceRotation != 3)
            {
                allCorrect = false;
            }
            if (molecule.pieceID == 2 && molecule.pieceRotation != 2)
            {
                allCorrect = false;
            }
            if (molecule.pieceID == 3 && molecule.pieceRotation != 0)
            {
                allCorrect = false;
            }
            if (molecule.pieceID == 4 && molecule.pieceRotation != 0)
            {
                allCorrect = false;
            }
            if (molecule.pieceID == 5 && molecule.pieceRotation != 3)
            {
                allCorrect = false;
            }
            if (molecule.pieceID == 6 && molecule.pieceRotation != 3)
            {
                allCorrect = false;
            }
            if (molecule.pieceID == 7 && molecule.pieceRotation != 0)
            {
                allCorrect = false;
            }



        }
        if (allCorrect)
        {
            puzzleManager = FindAnyObjectByType<PuzzleManager>();
            puzzleManager.CompletePuzzle("TangramPuzzle");
            
        }
    }

    
}
