using UnityEngine;
using UnityEngine.EventSystems;


public class DraggableMolecule : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int moleculeID;
    private Transform originalParent;
    private Canvas canvas;
    private RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    private Vector3 initialPosition;
    private FetusScript fetus;

    [SerializeField]
    private float snapDistance = 80f; // Distancia máxima para ajustar automáticamente al slot más cercano
    [SerializeField]
    private Vector2 pointerOffset = new Vector2(40f, 40f); // Desfase del puntero

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (originalParent == null)
        {
            originalParent = transform.parent;
            initialPosition = transform.position;
        }

        //transform.SetParent(canvas.transform);
        //transform.SetAsLastSibling(); // Asegura que este objeto se dibuje al frente

        // Hacer el objeto más fácil de arrastrar (opcional)
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false; // Evitar conflictos con raycasts
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Ajustar la posición al centro del puntero con un desfase
        Vector3 pointerPosition = eventData.position;
        transform.position = pointerPosition - new Vector3(pointerOffset.x, pointerOffset.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Transform closestSlot = GetClosestSlot();

        if (closestSlot != null)
        {
            // Ajustar al centro del slot más cercano

            var existingMolecule = closestSlot.GetComponentInChildren<DraggableMolecule>();
            if (existingMolecule != null && existingMolecule != this)
            {
                existingMolecule.ReturnToInitialPosition();
            }
            transform.SetParent(closestSlot);

            // Asegurar que el objeto se coloca en el centro del slot
            rectTransform.anchoredPosition = Vector2.zero + new Vector2(6,6);
        }
        else
        {
            
            ReturnToInitialPosition();
        }

        // Restaurar el raycast
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }
        fetus = FindAnyObjectByType<FetusScript>();
        fetus.currentHint = "WordlePuzzle";
    }

    private Transform GetClosestSlot()
    {
        Transform closestSlot = null;
        float closestDistance = snapDistance;

        foreach (var slot in FindObjectsOfType<SlotScript>()) // Busca todos los slots disponibles
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position - new Vector3(pointerOffset.x, pointerOffset.y, 0f)) ;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSlot = slot.transform;
            }
        }

        return closestSlot;
    }


    public void ReturnToInitialPosition()
    {
        transform.SetParent(originalParent);
        transform.position = initialPosition;
    }
}
