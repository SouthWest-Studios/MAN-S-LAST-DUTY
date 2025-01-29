using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableTangram : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public int pieceID;
    public int pieceRotation = 0;
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
    [SerializeField] private AudioSource pickupSound;
    [SerializeField] private AudioSource dropSound;
    [SerializeField] private AudioSource rotateSound;

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

        if (pickupSound != null)
            pickupSound.Play();

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
            rectTransform.anchoredPosition = Vector2.zero + new Vector2(6, 6);

            if (dropSound != null)
                dropSound.Play();
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
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Si no se está arrastrando el objeto, rotarlo 90 grados
        if (eventData.clickCount == 1)
        {
            if (rotateSound != null)
                rotateSound.Play();
                
            transform.Rotate(0, 0, 90);
            pieceRotation++;
            if (pieceRotation > 3)
            {
                pieceRotation = 0;
            }
        }
    }

    private Transform GetClosestSlot()
    {
        Transform closestSlot = null;
        float closestDistance = snapDistance;

        foreach (var slot in FindObjectsOfType<SlotTangramScript>()) // Busca todos los slots disponibles
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position - new Vector3(pointerOffset.x, pointerOffset.y, 0f));

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
