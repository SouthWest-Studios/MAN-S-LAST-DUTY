using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rewind : MonoBehaviour
{
    public PuzzleManager puzzleManager;
    [Header("Rewind Settings")]
    public bool isRewinding = false;
    private bool isCountdownStart = false;
    public Transform cameraTransform;
    public float timeBetweenSaves = 0.2f;
    public float timeToReloadScene = 4f;
    private float contadorReloadScene = 0;
    private float contadorInicioScene = 0;
    public float initialFadetoBlackSceneTime = 2;
    public Image fadeToBlackImage;


    [Header("Smooth Time Settings")]
    public float smoothTime = 0.1f;
    public float smoothTimeDecrement = 0.01f;
    public float smoothTimeMin = 0.3f;
    private float currentSmoothTime;

    [Header("Time Settings")]
    public int resetTime = 60;
    private int minuteCounter;
    public TextMeshProUGUI minuteText;

    [Header("Blur Settings")]
    public Material blurMaterial;
    public float maxBlurSize = 30.0f;
    public float blurIncrementSpeed = 5.0f;
    private float currentBlurSize = 0.0f;

    private Stack<Vector3> positionStack = new Stack<Vector3>();
    private Stack<Quaternion> rotationStack = new Stack<Quaternion>();
    private float timeCounter = 0;

    [Header("Post-Processing Settings")]
    public Volume timeTravelVolume;
    private ChromaticAberration chromaticAberration;
    private FilmGrain filmGrain;
    private LensDistortion lensDistortion;
    private ColorAdjustments colorAdjustments;
    private Vignette vignette;

    public float maxChromaticAberration = 0.3f;
    public float chromaticAberrationSpeed = 2f;
    public float maxFilmGrain = 0.5f;
    public float filmGrainSpeed = 2f;
    public float maxLensDistortion = 0.3f;
    public float lensDistortionSpeed = 2f;
    public float maxVignette = 1f;
    public float vignetteSpeed = 2f;
    public Color targetColor = Color.blue;

    // Variables para el rebote de la distorsión de lente
    private bool isLensDistortionRebounding = false;
    private float lensDistortionTime = 0f;
    private float lensDistortionInitialValue = 0f;
    private float lensDistortionMaxValue = 0.5f; // Tamaño máximo de la distorsión (ajusta según necesites)
    private int reboundCount = 0;  // Número de repeticiones del rebote
    private int maxRebounds = 4;   // Máximo número de repeticiones
    private float reboundIntensityDecay = 0.5f;  // Factor para reducir la intensidad después de cada rebote

    // Start is called before the first frame update
    void Start()
    {
        contadorInicioScene = initialFadetoBlackSceneTime;
        InitializeRewind();
        InitializePostProcessing();
        //StartCoroutine(CountdownTimer());
        StartLensDistortionRebound(); // Inicia el rebote de la distorsión de lente al inicio
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();
        UpdateMinuteText();

        if (isRewinding)
        {
            IncreaseBlurEffect();
            RewindPlayerState();
            ApplyPostProcessingEffects();


            if(contadorReloadScene >= timeToReloadScene)
            {
                puzzleManager.ResetAllPuzzles();
                puzzleManager.SaveAllPuzzles();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                
            }
            else
            {

                fadeToBlackImage.color = new Color(6f / 255f, 6f / 255f, 6f / 255f, (contadorReloadScene / timeToReloadScene));
                contadorReloadScene += Time.deltaTime;
            }


        }
        else
        {

            if(contadorInicioScene > 0)
            {
                contadorInicioScene -= Time.deltaTime;
                fadeToBlackImage.color = new Color(6f / 255f, 6f / 255f, 6f / 255f, (contadorInicioScene / initialFadetoBlackSceneTime));
            }


            ResetBlur();
            ResetPostProcessingEffects();
            SavePlayerState();
        }

        if (!isRewinding && !isLensDistortionRebounding)
        {
            // Inicia el rebote de la distorsión de lente al final del rewind
            StartLensDistortionRebound();
        }

        // Maneja la animación de rebote de la distorsión de lente
        if (isLensDistortionRebounding)
        {
            HandleLensDistortionRebound();
        }
    }

    // Inicializa las variables del rewind
    private void InitializeRewind()
    {
        minuteCounter = resetTime;
        currentSmoothTime = smoothTime;
    }

    private void InitializePostProcessing()
    {
        timeTravelVolume.profile.TryGet(out chromaticAberration);
        timeTravelVolume.profile.TryGet(out filmGrain);
        timeTravelVolume.profile.TryGet(out lensDistortion);
        timeTravelVolume.profile.TryGet(out colorAdjustments);
        timeTravelVolume.profile.TryGet(out vignette);
    }

    // Deshabilita el movimiento del jugador durante el rewind
    private void HandlePlayerMovement()
    {
        GetComponent<FirstPersonMovement>().enabled = !isRewinding;
    }

    // Incrementa el efecto de blur durante el rewind
    private void IncreaseBlurEffect()
    {
        if (currentBlurSize < maxBlurSize)
        {
            currentBlurSize += blurIncrementSpeed * Time.deltaTime;
            blurMaterial.SetFloat("_BlurSize", currentBlurSize);
        }
    }

    // Resetea el blur cuando no estamos en rewind
    private void ResetBlur()
    {
        if (currentBlurSize > 0)
        {
            currentBlurSize = 0.0f;
            blurMaterial.SetFloat("_BlurSize", currentBlurSize);
        }
    }

    // Guarda el estado del jugador
    private void SavePlayerState()
    {
        if (timeCounter > timeBetweenSaves)
        {
            timeCounter = 0;
            positionStack.Push(transform.position);
            rotationStack.Push(cameraTransform.rotation);
        }
        else
        {
            timeCounter += Time.deltaTime;
        }
    }

    // Rewinds el estado del jugador (movimiento y rotación)
    private void RewindPlayerState()
    {
        if (timeCounter > currentSmoothTime)
        {
            timeCounter = 0;
            currentSmoothTime -= smoothTimeDecrement;

            if (currentSmoothTime < smoothTimeMin)
                currentSmoothTime = smoothTimeMin;

            if (positionStack.Count > 0) transform.position = positionStack.Pop();
            if (rotationStack.Count > 0) cameraTransform.rotation = rotationStack.Pop();
        }
        else
        {
            timeCounter += Time.deltaTime;
        }

        // Si no quedan más estados que retroceder, para el rewind
        if (positionStack.Count <= 0 && rotationStack.Count <= 0)
        {
            StopRewind();
        }
    }

    // Para el rewind y resetea el blur
    private void StopRewind()
    {
        isRewinding = false;
        minuteCounter = resetTime;
        StartCoroutine(CountdownTimer());

        ResetBlur();
    }

    // Actualiza el contador de minutos
    private void UpdateMinuteText()
    {
        minuteText.text = minuteCounter.ToString();
    }

    // Temporizador de cuenta regresiva para activar el rewind después de que pase el tiempo
    private IEnumerator CountdownTimer()
    {
        while (minuteCounter > 0)
        {
            minuteCounter--;
            yield return new WaitForSeconds(1f);
        }

        isRewinding = true;
        minuteCounter = resetTime;
        currentSmoothTime = smoothTime;
    }

    // Aplica efectos de post procesamiento durante el rewind
    private void ApplyPostProcessingEffects()
    {
        ApplyChromaticAberration();
        ApplyFilmGrain();
        ApplyLensDistortion();
        ApplyColorAdjustment();
        ApplyVignette();
    }

    // Aplica el efecto de aberración cromática
    private void ApplyChromaticAberration()
    {
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = Mathf.MoveTowards(chromaticAberration.intensity.value, maxChromaticAberration, chromaticAberrationSpeed * Time.deltaTime);
        }
    }

    // Aplica el efecto de grano de película
    private void ApplyFilmGrain()
    {
        if (filmGrain != null)
        {
            filmGrain.intensity.value = Mathf.MoveTowards(filmGrain.intensity.value, maxFilmGrain, filmGrainSpeed * Time.deltaTime);
        }
    }

    // Aplica el efecto de distorsión de lente
    private void ApplyLensDistortion()
    {
        if (lensDistortion != null)
        {
            lensDistortion.intensity.value = Mathf.MoveTowards(lensDistortion.intensity.value, maxLensDistortion, lensDistortionSpeed * Time.deltaTime);
            if(lensDistortion.intensity.value >= (maxLensDistortion - 0.2))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    // Aplica ajustes de color (tono y saturación)
    private void ApplyColorAdjustment()
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.colorFilter.value = Color.Lerp(colorAdjustments.colorFilter.value, targetColor, Time.deltaTime * chromaticAberrationSpeed);
        }
    }

    // Aplica el efecto de viñeta
    private void ApplyVignette()
    {
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, maxVignette, vignetteSpeed * Time.deltaTime);
        }
    }

    // Resetea los efectos de post procesamiento cuando no estamos en rewind
    private void ResetPostProcessingEffects()
    {
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = 0;
        }
        if (filmGrain != null)
        {
            filmGrain.intensity.value = 0;
        }
        if (lensDistortion != null)
        {
            lensDistortion.intensity.value = 0;
        }
        if (colorAdjustments != null)
        {
            colorAdjustments.colorFilter.value = Color.white;
        }
        if (vignette)
        {
            vignette.intensity.value = 0.25f;
        }
    }

    // Resetea el blur cuando deshabilitamos el componente o cerramos la aplicación
    private void OnDisable()
    {
        ResetBlur();
        ResetPostProcessingEffects();
    }

    private void OnApplicationQuit()
    {
        ResetBlur();
        ResetPostProcessingEffects();
    }

    // Inicia el rebote de la distorsión de lente al principio
    private void StartLensDistortionRebound()
    {
        isLensDistortionRebounding = true;
        lensDistortionTime = 0f; // Restablece el tiempo del rebote
        lensDistortionInitialValue = lensDistortion.intensity.value; // Guarda el valor inicial
        lensDistortion.intensity.value = 2;
        reboundCount = 0;  // Reinicia el contador de repeticiones
    }

    // Maneja el rebote de la distorsión de lente (expansión y contracción) con intensidad decreciente
    private void HandleLensDistortionRebound()
    {
        if (reboundCount < maxRebounds)
        {
            if (lensDistortionTime < 1f)
            {
                // Rebotar la distorsión de lente con una onda (expansión y contracción)
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortionInitialValue, lensDistortionMaxValue, Mathf.Sin(lensDistortionTime * Mathf.PI));
                lensDistortionTime += Time.deltaTime * 2f; // Acelera o desacelera el rebote
            }
            else
            {
                // Al finalizar el rebote, reduce la intensidad y reinicia el tiempo
                lensDistortionTime = 0f;
                lensDistortionMaxValue *= reboundIntensityDecay; // Reduce la intensidad del siguiente rebote
                reboundCount++; // Incrementa el contador de repeticiones
            }
        }
        else
        {
            // Al finalizar los rebotes, restablece la distorsión de lente a su valor inicial
            lensDistortion.intensity.value = lensDistortionInitialValue;
            isLensDistortionRebounding = false; // Detener el rebote
        }
    }

    public void StartCountdown()
    {
        StartCoroutine(CountdownTimer());
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SaveZone") && !isCountdownStart)
        {
            isCountdownStart = true;
            StartCountdown();
        }
    }

}
