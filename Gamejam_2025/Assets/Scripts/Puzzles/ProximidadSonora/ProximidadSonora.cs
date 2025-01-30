using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ProximidadSonora : MonoBehaviour
{
    public ObjectInteraction objectInteraction;
    [Header("Grid Settings")]
    public int gridWidth = 10;
    public int gridHeight = 10;
    [Tooltip("Área rectangular donde el jugador puede interactuar con el minijuego")]
    public RectTransform gridContainer;
    [Tooltip("Imagen que muestra las líneas de la cuadrícula")]
    public Image gridBackground;

    [Header("Audio Settings")]
    public AudioSource radarAudioSource;
    public AudioSource effectsAudioSource;
    public AudioClip successSound;
    public AudioClip failSound;
    public float maxVolume = 1f;
    public float minVolume = 0.1f;
    public float maxPitchVariation = 2f;
    [Tooltip("Tiempo entre pulsos de sonido cuando está más lejos")]
    public float maxPulseInterval = 1.5f;
    [Tooltip("Tiempo entre pulsos de sonido cuando está más cerca")]
    public float minPulseInterval = 0.1f;
    private float nextPulseTime;

    [Header("Target Settings")]
    private Vector2 targetPosition;
    public float proximityThreshold = 50f;

    [Header("UI Elements")]
    [Tooltip("Texto 3D para mostrar las coordenadas en la tablet")]
    public TextMeshPro coordinatesText;
    public CrosshairController crosshairController;

    [Header("Grid Visuals")]
    public GameObject linePreFab;
    public Color gridColor = Color.white;
    private List<GameObject> gridLines = new List<GameObject>();

    [Header("References")]
    public GameObject mainCanvas;
    public FirstPersonLook cameraFirstPerson;

    [Header("Camera Shake")]
    public TraumaInducer shakeEffect;
    public float errorShakeIntensity = 0.3f;

    [Header("Level Settings")]
    public int currentLevel = 1;
    public TextMeshProUGUI levelText; // Cambiado de TextMeshPro a TextMeshProUGUI

    private Vector3 targetPosition3D; // Ahora almacenamos las coordenadas en 3D

    private bool gameActive = false;
    private Vector2 lastMousePosition;

    public static float level1Sum = 0f;
    public static float level2Sum = 0f;
    public static float level3Sum = 0f;

    public  float copylevel1Sum = 0f;
    public  float copylevel2Sum = 0f;
    public  float copylevel3Sum = 0f;

    public NavigationManager navigationManager;

    [Header("Click Settings")]
    public float clickCooldown = 3f;
    private float lastClickTime = -3f;

    public AudioClip[] clipsSub;

    public string[] lineES;
    public string[] lineEN;
    public string[] lineCA;

    private void Start()
    {
        InitializeGame();
        if (coordinatesText != null)
        {
            coordinatesText.gameObject.SetActive(false);
        }
        if (mainCanvas == null)
        {
            mainCanvas = transform.parent?.gameObject;
            if (mainCanvas == null)
                Debug.LogError("No se ha asignado el canvas principal y no se pudo encontrar automáticamente");
        }

        // Initialize random values based on puzzle seed
        Puzzle puzzle = PuzzleManager.instance.GetPuzzle("ProximidadSonoraPuzzle");
        if (puzzle != null)
        {
            Random.InitState(puzzle.seed);
            level1Sum = Random.Range(0f, 30f);
            level2Sum = Random.Range(0f, 30f);
            level3Sum = Random.Range(0f, 30f);
            copylevel1Sum = level1Sum;
            copylevel2Sum = level2Sum;
            copylevel3Sum = level3Sum;
        }
        navigationManager.InitializeNavigationSonor();
        UpdateLevelText();
    }

    private void OnEnable()
    {
        StartGame();
    }

    private void OnDisable()
    {
        gameActive = false;
        if (radarAudioSource != null)
            radarAudioSource.Stop();
        if (coordinatesText != null)
            coordinatesText.gameObject.SetActive(false);
        if (crosshairController != null)
            crosshairController.ShowCrosshair(true);

    }

    private void StartGame()
    {
        gameActive = true;
        GenerateNewTarget();
        if (crosshairController != null)
            crosshairController.ShowCrosshair(false);
        UpdateLevelText();
    }

    private void InitializeGame()
    {
        if (radarAudioSource == null)
        {
            radarAudioSource = gameObject.AddComponent<AudioSource>();
            radarAudioSource.playOnAwake = false;
            radarAudioSource.spatialBlend = 0f;
        }

        if (effectsAudioSource == null)
        {
            effectsAudioSource = gameObject.AddComponent<AudioSource>();
            effectsAudioSource.playOnAwake = false;
            effectsAudioSource.spatialBlend = 0f;
        }
        
        CreateGrid();
    }

    private void CreateGrid()
    {
        foreach (var line in gridLines)
        {
            if (line != null)
                Destroy(line);
        }
        gridLines.Clear();

        float cellWidth = gridContainer.rect.width / gridWidth;
        float cellHeight = gridContainer.rect.height / gridHeight;

        for (int i = 0; i <= gridWidth; i++)
        {
            GameObject line = Instantiate(linePreFab, gridContainer);
            RectTransform rectTransform = line.GetComponent<RectTransform>();
            Image lineImage = line.GetComponent<Image>();

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.sizeDelta = new Vector2(2f, 0);
            rectTransform.anchoredPosition = new Vector2(i * cellWidth, 0);
            
            lineImage.color = gridColor;
            gridLines.Add(line);
        }

        for (int i = 0; i <= gridHeight; i++)
        {
            GameObject line = Instantiate(linePreFab, gridContainer);
            RectTransform rectTransform = line.GetComponent<RectTransform>();
            Image lineImage = line.GetComponent<Image>();

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 0);
            rectTransform.sizeDelta = new Vector2(0, 2f);
            rectTransform.anchoredPosition = new Vector2(0, i * cellHeight);
            
            lineImage.color = gridColor;
            gridLines.Add(line);
        }
    }

    private void GenerateNewTarget()
    {
        Vector2 target2D = new Vector2(
            Random.Range(0, gridWidth),
            Random.Range(0, gridHeight)
        );
        
        targetPosition = target2D;
        // Guardamos también un valor Z para el nivel 3
        targetPosition3D = new Vector3(target2D.x, target2D.y, Random.Range(0, gridHeight));
        
        Debug.Log("New Target Position 2D: " + targetPosition);
        Debug.Log("New Target Position 3D: " + targetPosition3D);
    }

    private void Update()
    {
        if (mainCanvas.activeSelf)
        {
            OnEnable();
        }
        if (!gameActive) return;

        HandleMouseInput();

        
        if (!mainCanvas.activeSelf)
        {
            OnDisable();
        }
        //if (mainCanvas && Input.GetKeyDown(KeyCode.E) && gameActive)
        //{
        //    OnDisable();
        //}



    }

    private void HandleMouseInput()
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(gridContainer, Input.mousePosition))
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gridContainer,
            Input.mousePosition,
            null,
            out Vector2 localPoint
        );

        Vector2 gridPosition = new Vector2(
            (localPoint.x + gridContainer.rect.width / 2) / gridContainer.rect.width * gridWidth,
            (localPoint.y + gridContainer.rect.height / 2) / gridContainer.rect.height * gridHeight
        );

        UpdateAudioFeedback(gridPosition);

        if (Input.GetMouseButtonDown(0) && Time.time >= lastClickTime + clickCooldown)
        {
            lastClickTime = Time.time;
            CheckHit(gridPosition);
        }
    }

    private void UpdateAudioFeedback(Vector2 currentPosition)
    {
        float distance = Vector2.Distance(currentPosition, targetPosition);
        float maxDistance = Mathf.Sqrt(gridWidth * gridWidth + gridHeight * gridHeight);
        float normalizedDistance = distance / maxDistance;
        
        float volume = Mathf.Lerp(maxVolume, minVolume, normalizedDistance);
        float pitch = 1f + maxPitchVariation * (1 - normalizedDistance);

        float pulseInterval = Mathf.Lerp(minPulseInterval, maxPulseInterval, normalizedDistance);

        if (Time.time >= nextPulseTime)
        {
            radarAudioSource.volume = volume;
            radarAudioSource.pitch = pitch;
            radarAudioSource.Play();
            
            nextPulseTime = Time.time + pulseInterval;
        }
    }

    private void CheckHit(Vector2 clickPosition)
    {
        float distance = Vector2.Distance(clickPosition, targetPosition);
        Debug.Log("Distancia al objetivo: " + distance);
        Debug.Log("Posición del clic: " + clickPosition);
        
        if (distance <= proximityThreshold / 100f)
        {
            ShowCoordinates();
            radarAudioSource.Stop();
            if (successSound != null)
            {
                effectsAudioSource.clip = successSound;
                effectsAudioSource.Play();
            }
            
            if (currentLevel < 3)
            {
                currentLevel++;
                StartCoroutine(StartNextLevelAfterDelay(1f));
            }
            else
            {
                gameActive = false;



                SubtitulosManager.instance.PlayDialogue(lineES, lineEN, lineCA, clipsSub);







                StartCoroutine(CloseGameAfterDelay(1f));
            }
        }
        else 
        {
            if (failSound != null)
            {
                effectsAudioSource.clip = failSound;
                effectsAudioSource.Play();
            }
            
            if (shakeEffect != null)
            {
                shakeEffect.MaximumStress = errorShakeIntensity;
                shakeEffect.Delay = 0f;
                shakeEffect.InduceTrauma();
            }
        }
    }

    private System.Collections.IEnumerator CloseGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // if (cameraFirstPerson != null)
        //     cameraFirstPerson.isPanelOpen = false;
            
        // if (crosshairController != null)
        //     crosshairController.ShowCrosshair(true);
            
        // if (mainCanvas != null)
        //     mainCanvas.SetActive(false);
        // else
        //     Debug.LogWarning("No se puede cerrar el canvas porque la referencia es null");
        objectInteraction.EndFocusTransition();

    }

    private System.Collections.IEnumerator StartNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartGame();
    }

    private void ShowCoordinates()
    {
        if (coordinatesText != null)
        {
            string coordsText = "";
            
            if (currentLevel >= 1)
            {
                coordsText = $"X = {level1Sum:0}";
            }
            
            if (currentLevel >= 2)
            {
                coordsText += $"\nY = {level2Sum:0}";
            }
            
            if (currentLevel >= 3)
            {
                coordsText += $"\nZ = {level3Sum:0}";
            }

            coordinatesText.text = coordsText;
            coordinatesText.gameObject.SetActive(true);
        }
    }

    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = $"Nivel {currentLevel}";
        }
    }

    public float GetLevel1Sum()
    {
        Debug.Log("returnX = " + copylevel1Sum);
        return copylevel1Sum;
    }
    public float GetLevel2Sum()
    {
        Debug.Log("returnY = " + copylevel2Sum);
        return copylevel2Sum;
        
    }
    public float GetLevel3Sum()
    {
        Debug.Log("returnZ = " + copylevel3Sum);
        return copylevel3Sum;
        
    }
}
