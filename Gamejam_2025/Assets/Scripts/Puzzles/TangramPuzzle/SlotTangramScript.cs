using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotTangramScript : MonoBehaviour
{
    private Image slotImage;

    private void Awake()
    {
        slotImage = GetComponent<Image>();
    }

    public void SetSlotColor(Color color)
    {
        if (slotImage != null)
        {
            slotImage.color = color;
        }
    }
}
