using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PDPipeCell : MonoBehaviour
{
    public PDPipeType pipe = null;
    public Vector2 position = Vector2.zero;

    public void SetPipe()
    {
        if (pipe == null)
        {
            pipe = PDPipePreview.instance.NextPipe();
            pipe.RotatePipe(pipe.initialRotation);
            transform.rotation = Quaternion.Euler(0, 0, pipe.initialRotation * -90);
            GetComponent<Image>().sprite = pipe.emptySprite;
        }
    }

    public void FillPipe()
    {
        if (pipe != null)
        {
            pipe.isFilled = true;
            GetComponent<Image>().sprite = pipe.fillAnimationSprites[^1];
        }
    }

    public void FillPipeWithAnimation(int entryDirection)
    {
        if (pipe == null) return;

        Sprite[] animationSprites = pipe.fillAnimationSprites;

        if (animationSprites == null || animationSprites.Length == 0)
        {
            FillPipe();
            return;
        }

        StartCoroutine(PlayFillAnimation(animationSprites));
    }

    private IEnumerator PlayFillAnimation(Sprite[] animationSprites)
    {
        Image pipeImage = GetComponent<Image>();

        foreach (var sprite in animationSprites)
        {
            pipeImage.sprite = sprite;
            yield return new WaitForSeconds(PDFlowManager.instance.flowSpeed / animationSprites.Length);
        }

        pipe.isFilled = true;
        pipeImage.sprite = animationSprites[^1];
    }
}
