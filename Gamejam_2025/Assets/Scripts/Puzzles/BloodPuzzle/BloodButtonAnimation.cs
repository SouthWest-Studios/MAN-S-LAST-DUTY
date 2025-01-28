using System.Collections;
using UnityEngine;

public class BloodButtonAnimation : MonoBehaviour
{
    public GameObject button; // Asigna el botón en el inspector
    public float moveDistance = 40f; // Distancia que se moverá en el eje Y
    public float animationSpeed = 2f; // Velocidad de la animación

    public void ButtonDown()
    {
        // Inicia la corutina para mover el botón
        StartCoroutine(AnimateButton());
    }

    private IEnumerator AnimateButton()
    {
        Vector3 startPosition = button.transform.localPosition; // Posición inicial
        Vector3 downPosition = startPosition - new Vector3(0, moveDistance, 0); // Posición hacia abajo

        float elapsedTime = 0f;

        // Mover hacia abajo
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * animationSpeed;
            button.transform.localPosition = Vector3.Lerp(startPosition, downPosition, elapsedTime);
            yield return null;
        }

        elapsedTime = 0f;

        // Mover hacia arriba
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * animationSpeed;
            button.transform.localPosition = Vector3.Lerp(downPosition, startPosition, elapsedTime);
            yield return null;
        }

        // Asegurarse de que vuelva exactamente a la posición inicial
        button.transform.localPosition = startPosition;
    }
}
