using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrimerPuzzleButtons : MonoBehaviour
{
    private Button button;  
    private bool isPressed = false;


    public int num;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnButtonPressed()
    {
        if (!isPressed)
        {
            var colors = button.colors;
            colors.normalColor = colors.pressedColor;
            button.colors = colors;

            isPressed = true;

            //button.onClick.RemoveAllListeners();
            //Debug.Log("Botón presionado y desactivado temporalmente.");
        }
    }

    private void Update()
    {
        
    }

    public void ResetButton()
    {
        var colors = button.colors;
        colors.normalColor = colors.highlightedColor;
        button.colors = colors;

        isPressed = false;
        button.onClick.AddListener(OnButtonPressed);
        Debug.Log("Botón restaurado.");
    }
}
