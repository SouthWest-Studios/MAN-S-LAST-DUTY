using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomQueue<T>
{
    private Queue<T> queue;

    public CustomQueue()
    {
        queue = new Queue<T>();
    }

    // Añadir elemento al final de la cola
    public void Enqueue(T item)
    {
        queue.Enqueue(item);
    }

    // Sacar el elemento al principio de la cola
    public T Dequeue()
    {
        if (queue.Count > 0)
        {
            return queue.Dequeue();
        }
        else
        {
            Debug.LogWarning("La cola está vacía. No se puede hacer Dequeue.");
            return default(T);
        }
    }

    // Obtener todos los elementos de la cola (sin eliminarlos)
    public List<T> GetAllElements()
    {
        return new List<T>(queue);
    }

    // Ver el elemento en la parte frontal sin sacarlo
    public T Peek()
    {
        if (queue.Count > 0)
        {
            return queue.Peek();
        }
        else
        {
            Debug.LogWarning("La cola está vacía. No se puede hacer Peek.");
            return default(T);
        }
    }

    // Verificar el número de elementos en la cola
    public int Count()
    {
        return queue.Count;
    }

    internal void Clear()
    {
        while(queue.Count > 0)
        {
            queue.Dequeue();
        }
    }
}
