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
        GenerateRandomCombination();
    }

    private void GenerateRandomCombination()
    {
        correctCombination = new List<int>();

        // Crear una lista de n�meros disponibles
        List<int> availableIDs = new List<int>();
        for (int i = 0; i < maxMoleculeID; i++)
        {
            availableIDs.Add(i);
        }

        // Elegir n�meros aleatorios sin repetici�n
        for (int i = 0; i < numberOfSlots; i++)
        {
            int randomIndex = Random.Range(0, availableIDs.Count); // Elegir un �ndice aleatorio
            int randomID = availableIDs[randomIndex]; // Obtener el ID
            correctCombination.Add(randomID); // Agregar a la combinaci�n

            // Eliminar el n�mero elegido para evitar duplicados
            availableIDs.RemoveAt(randomIndex);
        }

        Debug.Log("Combinaci�n Correcta: " + string.Join(", ", correctCombination));
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
            if (molecule != null)
            {
                if (molecule.moleculeID == correctCombination[i])
                {
                    // Verde: Correcta y en la posici�n correcta
                    slots[i].GetComponent<SlotScript>().SetSlotColor(Color.green);
                    EnableMoleculeInteraction(molecule, true); // Habilitar interacci�n si es correcta
                }
                else if (correctCombination.Contains(molecule.moleculeID))
                {
                    // Amarillo: Correcta pero en la posici�n incorrecta
                    slots[i].GetComponent<SlotScript>().SetSlotColor(Color.yellow);
                    EnableMoleculeInteraction(molecule, true); // Habilitar interacci�n si es correcta pero en lugar incorrecto
                    allCorrect = false; // Si alguna mol�cula est� en amarillo, no est� completamente correcta
                }
                else
                {
                    // Rojo: Incorrecta
                    slots[i].GetComponent<SlotScript>().SetSlotColor(Color.red);
                    EnableMoleculeInteraction(molecule, false); // Deshabilitar interacci�n si es incorrecta
                    molecule.GetComponent<Image>().color = Color.red; // Cambiar color a rojo
                    allCorrect = false; // Si alguna mol�cula es incorrecta, no est� completamente correcta
                }
            }
            else
            {
                // Sin mol�cula en este slot
                slots[i].GetComponent<SlotScript>().SetSlotColor(Color.white);
                allCorrect = false; // Si hay un slot vac�o, el juego no est� completado
            }
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
