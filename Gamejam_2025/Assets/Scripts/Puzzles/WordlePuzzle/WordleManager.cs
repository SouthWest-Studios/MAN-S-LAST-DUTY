using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WordleController : MonoBehaviour
{
    public static List<SlotScript> slots; // Lista de todos los slots
    public int numberOfSlots = 5; // Número de slots en el juego
    public int maxMoleculeID = 11; // Rango máximo de IDs de moléculas
    public static List<int> correctCombination; // Lista de la combinación correcta
    private PuzzleManager puzzleManager;
    bool allCorrect;
    public GameObject canvas;
    public GameObject finalText;

    public GameObject hint;

    public List<InitialSlotScript> initialSlotScripts;

    public ObjectInteraction trigger;

    private void Start()
    {
        GenerateRandomCombination();
    }

    private void GenerateRandomCombination()
    {
        correctCombination = new List<int>();

        // Crear una lista de números disponibles
        List<int> availableIDs = new List<int>();
        for (int i = 0; i <= maxMoleculeID; i++)
        {
            availableIDs.Add(i);
        }

        // Elegir números aleatorios sin repetición
        for (int i = 0; i < numberOfSlots; i++)
        {
            int randomIndex = Random.Range(0, availableIDs.Count); // Elegir un índice aleatorio
            int randomID = availableIDs[randomIndex]; // Obtener el ID
            correctCombination.Add(randomID); // Agregar a la combinación

            // Eliminar el número elegido para evitar duplicados
            availableIDs.RemoveAt(randomIndex);
        }

        Debug.Log("Combinación Correcta: " + string.Join(", ", correctCombination));
    }

    private void Update()
    {
        if (allCorrect)
        {
            if(canvas.activeSelf)
            {
                puzzleManager = FindAnyObjectByType<PuzzleManager>();
                puzzleManager.CompletePuzzle("WordlePuzzle");
                //finalText.SetActive(true);
                trigger.EndFocusTransition();
                trigger.enabled = false;
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
                    // Verde: Correcta y en la posición correcta
                    slots[i].GetComponent<SlotScript>().SetSlotColor(Color.green);
                    EnableMoleculeInteraction(molecule, true); // Habilitar interacción si es correcta
                }
                else if (correctCombination.Contains(molecule.moleculeID))
                {
                    // Amarillo: Correcta pero en la posición incorrecta
                    slots[i].GetComponent<SlotScript>().SetSlotColor(Color.yellow);
                    EnableMoleculeInteraction(molecule, true); // Habilitar interacción si es correcta pero en lugar incorrecto
                    allCorrect = false; // Si alguna molécula está en amarillo, no está completamente correcta
                }
                else
                {
                    // Rojo: Incorrecta
                    slots[i].GetComponent<SlotScript>().SetSlotColor(Color.red);
                    molecule.ReturnToInitialPosition();
                    EnableMoleculeInteraction(molecule, false); // Deshabilitar interacción si es incorrecta
                    initialSlotScripts[molecule.moleculeID].gameObject.GetComponent<Image>().color = Color.red;
                    allCorrect = false; // Si alguna molécula es incorrecta, no está completamente correcta
                }
            }
            else
            {
                // Sin molécula en este slot
                slots[i].GetComponent<SlotScript>().SetSlotColor(Color.black);
                allCorrect = false; // Si hay un slot vacío, el juego no está completado
            }
        }
        
        if (allCorrect)
        {
            
        }
    }

    private void EnableMoleculeInteraction(DraggableMolecule molecule, bool enable)
    {
        // Aquí puedes deshabilitar la interacción con las moléculas que han sido marcadas como rojas
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

    public void SetHintActive()
    {
        if(hint != null)
        {
            hint.SetActive(true);
        }
        
    }
}
