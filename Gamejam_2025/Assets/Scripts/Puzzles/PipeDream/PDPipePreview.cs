using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PDPipePreview : MonoBehaviour
{
    public static PDPipePreview instance;
    public Image[] previewRender;
    public PDPipeType[] pipes;

    private Queue<PDPipeType> pipesQueue = new Queue<PDPipeType>();

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        while (pipesQueue.Count < 4)
        {
            int pipeRandom = Random.Range(0, pipes.Length);
            int randomRotation = Random.Range(0, 4);

            PDPipeType newPipe = Instantiate(pipes[pipeRandom]);
            newPipe.initialRotation = randomRotation;

            pipesQueue.Enqueue(newPipe);

            UpdatePreviewUI();
        }
    }

    public PDPipeType NextPipe()
    {
        var pipe = pipesQueue.Dequeue();
        UpdatePreviewUI();
        return pipe;
    }

    public void ResetPreview()
    {
        pipesQueue.Clear();
        foreach (Image previewImage in previewRender)
        {
            previewImage.sprite = null;
            previewImage.transform.rotation = Quaternion.identity;
        }

        Debug.Log("Vista previa reiniciada.");
    }

    private void UpdatePreviewUI()
    {
        var pipesList = pipesQueue.ToArray();
        for (int i = 0; i < previewRender.Length; i++)
        {
            if (i < pipesList.Length)
            {
                previewRender[i].sprite = pipesList[i].emptySprite;
                previewRender[i].transform.rotation = Quaternion.Euler(0, 0, pipesList[i].initialRotation * -90);
            }
            else
            {
                previewRender[i].sprite = null;
                previewRender[i].transform.rotation = Quaternion.identity;
            }
        }
    }
}
