using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static PuzzleManager;

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

    [Header("Audio")]
    [SerializeField] private AudioSource[] buttonSounds = new AudioSource[9];

    // Add TMP reference at the top with other variables
    public TextMeshPro numeroResultado;

    private bool[] lightStates = new bool[9];

    private const float POSICION_PRESIONADO = 0.0465f;
    private const float DURACION_ANIMACION = 0.1f;
    private Vector3[] posicionesOriginalesBottones = new Vector3[9];
    private PuzzleManager puzzlemanager;

    private void Start()
    {
        // Store original button positions
        for (int i = 0; i < buttons.Length; i++)
        {
            posicionesOriginalesBottones[i] = buttons[i].transform.localPosition;
        }
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
        for (int i = 0; i < buttons.Length; i++)
        {
            int buttonIndex = i; // Capture the index for the closure
            buttons[i].onClick.AddListener(() => StartCoroutine(AnimateButtonPress(buttonIndex)));
        }
    }

    private IEnumerator AnimateButtonPress(int buttonIndex)
    {
        Button button = buttons[buttonIndex];
        Vector3 posOriginal = posicionesOriginalesBottones[buttonIndex];
        Vector3 posBajada = new Vector3(posOriginal.x, POSICION_PRESIONADO, posOriginal.z);

        // Play the specific button sound
        if (buttonSounds[buttonIndex] != null)
        {
            buttonSounds[buttonIndex].Play();
        }

        // Press animation
        float tiempoTranscurrido = 0;
        while (tiempoTranscurrido < DURACION_ANIMACION)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / DURACION_ANIMACION;
            button.transform.localPosition = Vector3.Lerp(posOriginal, posBajada, t);
            yield return null;
        }

        // Release animation
        tiempoTranscurrido = 0;
        while (tiempoTranscurrido < DURACION_ANIMACION)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / DURACION_ANIMACION;
            button.transform.localPosition = Vector3.Lerp(posBajada, posOriginal, t);
            yield return null;
        }

        button.transform.localPosition = posOriginal;

        // Handle the button press logic after animation
        switch(buttonIndex)
        {
            case 0: PressButton(new int[] { 0, 1, 3 }); break;
            case 1: PressButton(new int[] { 0, 1, 2, 4 }); break;
            case 2: PressButton(new int[] { 1, 2, 5 }); break;
            case 3: PressButton(new int[] { 0, 3, 4, 6 }); break;
            case 4: PressButton(new int[] { 1, 3, 4, 5, 7 }); break;
            case 5: PressButton(new int[] { 2, 4, 5, 8 }); break;
            case 6: PressButton(new int[] { 3, 6, 7 }); break;
            case 7: PressButton(new int[] { 4, 6, 7, 8 }); break;
            case 8: PressButton(new int[] { 5, 7, 8 }); break;
        }
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
        bool allLightsOn = true;

        for (int i = 0; i < lightStates.Length; i++)
        {
            if (lightStates[i])
            {
                allLightsOff = false;
            }
            else
            {
                allLightsOn = false;
            }
        }

        if (allLightsOff || allLightsOn)
        {
            Debug.Log("¡Puzzle completado! (Aleix haz lo tuyo)");
            CompletarJuego();
        }
    }

    private void CompletarJuego()
    {
        puzzlemanager = FindObjectOfType<PuzzleManager>();
        puzzlemanager.CompletePuzzle("PintarCasillasPuzzle");
        
        // Get and display the fourth digit
        if (numeroResultado != null)
        {
            string code = PuzzleManager.numpadFinalCode;
            if (code.Length >= 4)
            {
                numeroResultado.text = $"***{code[3]}";
                numeroResultado.gameObject.SetActive(true);
            }
        }
    }
}
