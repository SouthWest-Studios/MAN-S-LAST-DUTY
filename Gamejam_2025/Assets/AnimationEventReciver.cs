using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReciver : MonoBehaviour
{

    public MainMenuManager mainMenuManager;

    public void OnTurnOffMonitorEnd()
    {
        if (mainMenuManager != null)
        {
            mainMenuManager.OnTurnOffMonitorEnd();
        }
    }
}
