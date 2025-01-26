using UnityEngine;
using UnityEngine.UI;

public class PDPipe : MonoBehaviour
{
    public bool[] connections = new bool[4]; // Arriba, Derecha, Abajo, Izquierda
    public int x, y;                         // Posición en la cuadrícula

    private int rotationState = 0;
    public Sprite pipeSprite;

    private void Start()
    {
        pipeSprite = GetComponent<Image>().sprite;
    }

    public void SetPosition(int posX, int posY)
    {
        x = posX;
        y = posY;
    }

    public void RotatePipe()
    {
        rotationState = (rotationState + 1) % 4;
        transform.rotation = Quaternion.Euler(0, 0, rotationState * 90);

        // Rotar conexiones
        bool temp = connections[3];
        for (int i = 3; i > 0; i--)
        {
            connections[i] = connections[i - 1];
        }
        connections[0] = temp;
    }

    public bool IsConnected(PDPipe other, int direction)
    {
        int oppositeDirection = (direction + 2) % 4; // Dirección opuesta
        return connections[direction] && other.connections[oppositeDirection];
    }

    // Método para calcular la próxima dirección del flujo
    public int GetNextDirection(int incomingDirection)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            // Salta la dirección por la que ya entró el flujo
            if (i == (incomingDirection + 2) % 4) continue;

            // Si hay una conexión en otra dirección, úsala
            if (connections[i])
            {
                return i;
            }
        }

        // Si no hay otras conexiones, devuelve -1 (flujo bloqueado)
        return -1;
    }
}
