using UnityEngine;
using UnityEngine.UI;

public class PDPipeCell : MonoBehaviour
{
    public PDPipeType pipe = null;
    public Vector2 position = Vector2.zero;

    private void Update()
    {
        if (pipe)
        {
            GetComponent<Image>().sprite = pipe.emptySprite;
        }
    }

    public void SetPipe()
    {
        if (pipe == null)
        {
            pipe = PDPipePreview.instance.NextPipe();
            pipe.RotatePipe(pipe.initialRotation);
            transform.rotation = Quaternion.Euler(0, 0, pipe.initialRotation * -90);
        }
    }

    public void FillPipe()
    {
        if (pipe != null)
        {
            pipe.isFilled = true;
            GetComponent<Image>().sprite = pipe.filledSprite;
        }
    }

    public void FillPipeWithEffect(GameObject waterPrefab)
    {
        if (pipe != null)
        {
            GetComponent<Image>().sprite = pipe.filledSprite;

            // Instancia un efecto visual (opcional)
            Instantiate(waterPrefab, transform.position, Quaternion.identity, transform);
        }
    }

    // Dibuja las conexiones en Gizmos
    private void OnDrawGizmos()
    {
        if (pipe == null) return;

        Color connectionColor = Color.green; // Conexión válida
        Color blockedColor = Color.red;     // Conexión bloqueada
        Color flowPossibleColor = Color.blue; // Posibilidad de flujo hacia otra celda

        Vector3 center = transform.position;

        // Dibujar conexiones actuales
        for (int direction = 0; direction < 4; direction++)
        {
            Vector3 directionVector = GetDirectionVector(direction);

            if (pipe.connections[direction])
            {
                // Línea verde para conexión actual
                Gizmos.color = connectionColor;
                Gizmos.DrawLine(center, center + directionVector * 0.5f);
                DrawArrow(center + directionVector * 0.5f, directionVector);

                // Verificar si la celda vecina está conectada correctamente
                if (IsFlowPossible(direction))
                {
                    // Línea azul para posibilidad de flujo
                    Gizmos.color = flowPossibleColor;
                    Gizmos.DrawLine(center + directionVector * 0.5f, center + directionVector * 0.8f);
                }
                else
                {
                    // Línea roja para conexión bloqueada
                    Gizmos.color = blockedColor;
                    Gizmos.DrawLine(center + directionVector * 0.5f, center + directionVector * 0.8f);
                }
            }
            else
            {
                // Línea roja más corta si no hay conexión
                Gizmos.color = blockedColor;
                Gizmos.DrawLine(center, center + directionVector * 0.3f);
            }
        }
    }

    private bool IsFlowPossible(int direction)
    {
        int neighborX = (int)position.x, neighborY = (int)position.y;
        switch (direction)
        {
            case 0: neighborX -= 1; break; // Arriba
            case 1: neighborY += 1; break; // Derecha
            case 2: neighborX += 1; break; // Abajo
            case 3: neighborY -= 1; break; // Izquierda
        }

        var neighborCell = PDGridManager.instance.GetPipeAt(neighborX, neighborY);
        if (neighborCell == null || neighborCell.pipe == null) return false;

        return pipe.IsConnectedTo(neighborCell.pipe, direction);
    }

    private Vector3 GetDirectionVector(int direction)
    {
        switch (direction)
        {
            case 0: return Vector3.up;
            case 1: return Vector3.right;
            case 2: return Vector3.down;
            case 3: return Vector3.left;
            default: return Vector3.zero;
        }
    }

    private void DrawArrow(Vector3 position, Vector3 direction)
    {
        Vector3 right = Quaternion.Euler(0, 0, 45) * -direction * 0.1f;
        Vector3 left = Quaternion.Euler(0, 0, -45) * -direction * 0.1f;

        Gizmos.DrawLine(position, position + right);
        Gizmos.DrawLine(position, position + left);
    }
}
