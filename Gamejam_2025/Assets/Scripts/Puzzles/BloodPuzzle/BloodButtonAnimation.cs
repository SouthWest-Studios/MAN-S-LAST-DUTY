using System.Collections;
using UnityEngine;

public class BloodButtonAnimation : MonoBehaviour
{
    public GameObject button; // Asigna el bot�n en el inspector
    public float moveDistance = 40f; // Distancia que se mover� en el eje Y
    public float animationSpeed = 2f; // Velocidad de la animaci�n
    [SerializeField] private AudioSource buttonSound;

    public void ButtonDown()
    {
        // Inicia la corutina para mover el bot�n
        StartCoroutine(AnimateButton());
    }

    private IEnumerator AnimateButton()
    {
        // Reproducir el sonido al inicio de la animación
        if (buttonSound != null)
        {
            buttonSound.Play();
        }

        Vector3 startPosition = button.transform.localPosition; // Posici�n inicial
        Vector3 downPosition = startPosition - new Vector3(0, moveDistance, 0); // Posici�n hacia abajo

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

        // Asegurarse de que vuelva exactamente a la posici�n inicial
        button.transform.localPosition = startPosition;
    }
}
