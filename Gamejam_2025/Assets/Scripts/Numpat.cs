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
    public AudioSource audioSource;
    public AudioClip buttonSound;
    public AudioClip errorClip;
    public AudioClip correctClip;
    public TraumaInducer traumaInducer;

    public void SendNumber(int number)
    {
        if (isOpened || indexPad >= 4) return;

        audioSource.PlayOneShot(buttonSound);
        codeNumbers[indexPad].sprite = numbers[number];
        indexPad++;
        actualCode += number.ToString();

        if (indexPad >= 4)
        {
            if (actualCode == PuzzleManager.numpadFinalCode)
            {
                audioSource.PlayOneShot(correctClip);
                foreach (var spriteRenderer in codeNumbers)
                {
                    spriteRenderer.color = correctColor;
                }

                doorAnimator.Play("DoubleOpeningDoor");
                foreach (AudioSource audioSource in doorAudioSources)
                {
                    audioSource.Play();
                }
                isOpened = true;
            }
            else
            {
                audioSource.PlayOneShot(errorClip);
                traumaInducer.InduceTrauma();
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
