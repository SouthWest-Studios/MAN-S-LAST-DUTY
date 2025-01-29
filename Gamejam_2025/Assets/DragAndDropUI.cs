using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 offset; // Offset del mouse respecto al objeto
    private bool isDragging = false; // Indica si el objeto debe moverse

    [Header("Zona de arrastre (en porcentaje)")]
    [Range(0f, 1f)] public float dragAreaWidth = 1f;  // Ancho de la zona de arrastre
    [Range(0f, 1f)] public float dragAreaHeight = 1f; // Alto de la zona de arrastre

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Convertir la posición del mouse en coordenadas locales del objeto
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localMousePosition);

        // Calcular la zona de arrastre
        Vector2 size = rectTransform.sizeDelta;
        float minX = -size.x * dragAreaWidth * 0.5f;
        float maxX = size.x * dragAreaWidth * 0.5f;
        float minY = -size.y * dragAreaHeight * 0.5f;
        float maxY = size.y * dragAreaHeight * 0.5f;

        // Verificar si el clic está dentro de la zona de arrastre
        if (localMousePosition.x >= minX && localMousePosition.x <= maxX &&
            localMousePosition.y >= minY && localMousePosition.y <= maxY)
        {
            isDragging = true;
            offset = localMousePosition; // Guardar el offset correcto
        }
        else
        {
            isDragging = false; // No iniciar el arrastre si está fuera de la zona
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return; // Solo arrastrar si el clic fue dentro de la zona permitida

        Vector2 newPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out newPosition))
        {
            rectTransform.localPosition = newPosition - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false; // Finalizar el arrastre
    }

    // Dibujar la zona de arrastre en la vista de Scene
    void OnDrawGizmos()
    {
        if (rectTransform == null) return;

        Gizmos.color = Color.green;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // Calcular el centro del rectángulo
        Vector3 center = (corners[0] + corners[2]) / 2f;

        // Calcular el tamaño de la zona de arrastre basado en porcentaje
        Vector3 size = new Vector3(
            rectTransform.rect.width * dragAreaWidth,
            rectTransform.rect.height * dragAreaHeight,
            0f);

        // Dibujar la caja representando la zona de arrastre
        Gizmos.DrawWireCube(center, size);
    }
}
