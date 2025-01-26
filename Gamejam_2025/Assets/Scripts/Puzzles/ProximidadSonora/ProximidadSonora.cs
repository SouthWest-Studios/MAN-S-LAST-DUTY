using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProximidadSonora : MonoBehaviour
{
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

    private bool gameActive = false;
    private Vector2 lastMousePosition;

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
        targetPosition = new Vector2(
            Random.Range(0, gridWidth),
            Random.Range(0, gridHeight)
        );
    }

    private void Update()
    {
        if (!gameActive) return;

        HandleMouseInput();
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

        if (Input.GetMouseButtonDown(0))
        {
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
            gameActive = false;
            Debug.Log("¡Juego completado! Coordenadas encontradas: " + targetPosition);
            
            StartCoroutine(CloseGameAfterDelay(2f));
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
        
        if (cameraFirstPerson != null)
            cameraFirstPerson.isPanelOpen = false;
            
        if (crosshairController != null)
            crosshairController.ShowCrosshair(true);
            
        if (mainCanvas != null)
            mainCanvas.SetActive(false);
        else
            Debug.LogWarning("No se puede cerrar el canvas porque la referencia es null");
    }

    private void ShowCoordinates()
    {
        if (coordinatesText != null)
        {
            coordinatesText.text = $"X={targetPosition.x:F1}\nY={targetPosition.y:F1}";
            coordinatesText.gameObject.SetActive(true);
        }
    }
}
