using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownScreen : MonoBehaviour
{
    public TextMeshPro countdownText;
    private Rewind rewindScript;

    void Start()
    {
        rewindScript = FindObjectOfType<Rewind>();
    }

    void Update()
    {
        if (rewindScript != null && countdownText != null)
        {
            countdownText.text = rewindScript.minuteCounter.ToString();
        }
    }
}
