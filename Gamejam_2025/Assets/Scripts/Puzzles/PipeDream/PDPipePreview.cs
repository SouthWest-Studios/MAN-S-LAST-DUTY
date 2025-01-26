using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PDPipePreview : MonoBehaviour
{
    public static PDPipePreview instance;
    public Image[] previewRender;
    public PDPipeType[] pipes;

    private CustomQueue<PDPipeType> pipesQueue = new CustomQueue<PDPipeType>();

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        while (pipesQueue.Count() < 4)
        {
            int pipeRandom = Random.Range(0, pipes.Length);
            int randomRotation = Random.Range(0, 4);

            PDPipeType newPipe = Instantiate(pipes[pipeRandom]);
            newPipe.initialRotation = randomRotation;

            pipesQueue.Enqueue(newPipe);

            List<PDPipeType> pipesQueueList = pipesQueue.GetAllElements();
            for (int i = 0; i < pipesQueue.Count(); i++)
            {
                previewRender[i].sprite = pipesQueueList[i].emptySprite;
                previewRender[i].transform.rotation = Quaternion.Euler(0, 0, pipesQueueList[i].initialRotation * -90);
            }
        }
    }

    public PDPipeType NextPipe()
    {
        return pipesQueue.Dequeue();
    }
}
