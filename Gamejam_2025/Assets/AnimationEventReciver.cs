using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReciver : MonoBehaviour
{

    public MainMenuManager mainMenuManager;
    public FinalSceneManager finalSceneManager;

    public void OnTurnOffMonitorEnd()
    {
        if (mainMenuManager != null)
        {
            mainMenuManager.OnTurnOffMonitorEnd();
        }
        if(finalSceneManager != null)
        {
            finalSceneManager.OnTurnOffMonitorEnd();
        }
    }
}
