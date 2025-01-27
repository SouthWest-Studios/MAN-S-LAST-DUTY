using UnityEngine;

[CreateAssetMenu(fileName = "NewPipeType", menuName = "PipeDream/Pipe Type")]
public class PDPipeType : ScriptableObject
{
    public string pipeName;
    public Sprite emptySprite;
    public Sprite[] fillAnimationSprites; // Sprites de la animación de llenado
    public Sprite[] fillLeftAnimationSprites; // Sprites de la animación de llenado
    public Sprite[] fillRightAnimationSprites; // Sprites de la animación de llenado
    public bool[] connections = new bool[4]; // Arriba, Derecha, Abajo, Izquierda
    public int initialRotation = 0;
    public bool isFilled = false;

    // Verifica si está conectada correctamente con la tubería vecina
    public bool IsConnectedTo(PDPipeType other, int direction)
    {
        return connections[direction] && other.connections[(direction + 2) % 4];
    }

    // Rota las conexiones de la tubería según la cantidad de rotaciones (90 grados por rotación)
    public void RotatePipe(int rotations)
    {
        bool[] newConnections = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            newConnections[(i + rotations) % 4] = connections[i];
        }
        connections = newConnections;
    }
}
