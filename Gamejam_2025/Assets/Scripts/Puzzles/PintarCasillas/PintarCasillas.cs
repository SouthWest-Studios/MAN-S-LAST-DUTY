using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PintarCasillas : MonoBehaviour
{
    [Header("Lights")]
    [SerializeField] private MeshRenderer[] lightBulbs = new MeshRenderer[9];
    [SerializeField] private GameObject[] pointLights = new GameObject[9];
    
    [Header("Materials")]
    [SerializeField] private Material onMaterial; 
    [SerializeField] private Material offMaterial; 

    [Header("Buttons")]
    [SerializeField] private Button[] buttons = new Button[9];

    private bool[] lightStates = new bool[9];

    private void Start()
    {
        SetupButtons();
        InitializeLights();
    }

    private void InitializeLights()
    {
        for (int i = 0; i < 9; i++)
        {
            ToggleLight(i, false);
        }

        // luces encendidas 1,3,4,5,6 ==> solución del puzzle 1,2,3,4,5,6,8.
        int[] lightsToTurnOn = new int[] { 0, 2, 3, 4, 5 };
        foreach (int index in lightsToTurnOn)
        {
            ToggleLight(index, true);
        }
    }

    private void SetupButtons()
    {
        buttons[0].onClick.AddListener(() => PressButton(new int[] { 0, 1, 3 }));
        buttons[1].onClick.AddListener(() => PressButton(new int[] { 0, 1, 2, 4 }));
        buttons[2].onClick.AddListener(() => PressButton(new int[] { 1, 2, 5 }));
        buttons[3].onClick.AddListener(() => PressButton(new int[] { 0, 3, 4, 6 }));
        buttons[4].onClick.AddListener(() => PressButton(new int[] { 1, 3, 4, 5, 7 }));
        buttons[5].onClick.AddListener(() => PressButton(new int[] { 2, 4, 5, 8 }));
        buttons[6].onClick.AddListener(() => PressButton(new int[] { 3, 6, 7 }));
        buttons[7].onClick.AddListener(() => PressButton(new int[] { 4, 6, 7, 8 }));
        buttons[8].onClick.AddListener(() => PressButton(new int[] { 5, 7, 8 }));
    }

    private void PressButton(int[] lightsToToggle)
    {
        foreach (int lightIndex in lightsToToggle)
        {
            ToggleLight(lightIndex, !lightStates[lightIndex]);
        }
        CheckWinCondition();
    }

    private void ToggleLight(int index, bool state)
    {
        lightStates[index] = state;
        lightBulbs[index].material = state ? onMaterial : offMaterial;
        pointLights[index].SetActive(state);
    }

    private void CheckWinCondition()
    {
        bool allLightsOff = true;
        for (int i = 0; i < lightStates.Length; i++)
        {
            if (lightStates[i])
            {
                allLightsOff = false;
                break;
            }
        }

        if (allLightsOff)
        {
            Debug.Log("¡Puzzle completado! (Aleix haz lo tuyo)");
        }
    }
}
