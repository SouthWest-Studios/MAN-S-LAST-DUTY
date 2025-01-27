using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitulosManager : MonoBehaviour
{

    public static SubtitulosManager instance;


    public AudioSource[] audiosSources;
    public GameObject dialoguePanel;
    public TextMeshProUGUI textDialogue;
    public float textSpeed = 0.1f;
    public float textSpeedNewLine = 2f;
    private int lineIndex = 0;

    private string[] lines;
    private AudioClip[] audiosClips;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        dialoguePanel.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        textDialogue.text = string.Empty;
    }
    public void PlayDialogue(string[] lines, AudioClip[] clip)
    {
        StopAllCoroutines();
        textDialogue.text = string.Empty;
        foreach (AudioSource audioSource in audiosSources)
        {

            audioSource.Stop();
        }

        this.lines = lines;
        this.audiosClips = clip;
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {


        foreach (AudioSource audioSource in audiosSources) {

            audioSource.PlayOneShot(audiosClips[lineIndex]);
        }

        foreach(char c in lines[lineIndex].ToCharArray())
        {
            textDialogue.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        if(textDialogue.text == lines[lineIndex])
        {
            yield return new WaitForSeconds(textSpeedNewLine);
            NextLine();
        }

    }

    void NextLine()
    {
        if(lineIndex < lines.Length - 1)
        {
            lineIndex++;
            textDialogue.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //fin dialogo
            StopAllCoroutines();
            textDialogue.text = string.Empty;
            lineIndex = 0;
            lines = null;
            audiosClips = null;
            dialoguePanel.SetActive(false);

        }
    }

}
