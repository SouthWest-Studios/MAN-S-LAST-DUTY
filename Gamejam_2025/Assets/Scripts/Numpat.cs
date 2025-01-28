using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numpat : MonoBehaviour
{
    private int indexPad = 0;
    public SpriteRenderer[] codeNumbers;
    public Sprite[] numbers;
    private string actualCode = "";
    public Animator doorAnimator;
    public AudioSource[] doorAudioSources;
    private bool isOpened = false;
    public Color errorColor = Color.red;
    public Color correctColor = Color.green;
    private Color defaultColor = Color.white;

    public void SendNumber(int number)
    {
        if (isOpened || indexPad >= 4) return;

        codeNumbers[indexPad].sprite = numbers[number];
        indexPad++;
        actualCode += number.ToString();

        if (indexPad >= 4)
        {
            if (actualCode == PuzzleManager.numpadFinalCode)
            {


                foreach (var spriteRenderer in codeNumbers)
                {
                    spriteRenderer.color = correctColor;
                }

                // Código correcto, abre la puerta
                doorAnimator.Play("DoubleOpeningDoor");
                foreach (AudioSource audioSource in doorAudioSources)
                {
                    audioSource.Play();
                }
                isOpened = true;
            }
            else
            {
                // Código incorrecto, cambiar a rojo y luego reiniciar
                StartCoroutine(WrongCodeSequence());
            }
        }
    }

    private IEnumerator WrongCodeSequence()
    {
        foreach (var spriteRenderer in codeNumbers)
        {
            spriteRenderer.color = errorColor;
        }

        yield return new WaitForSeconds(1);

        foreach (var spriteRenderer in codeNumbers)
        {
            spriteRenderer.color = defaultColor;
            spriteRenderer.sprite = null;
        }

        actualCode = "";
        indexPad = 0;
    }
}
