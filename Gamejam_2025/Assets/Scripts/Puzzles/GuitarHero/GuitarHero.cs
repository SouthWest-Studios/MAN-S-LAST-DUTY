using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static PuzzleManager;

public class GuitarHero : MonoBehaviour
{
    private PuzzleManager puzzlemanager;
    private string puzzleName;

    [Header("Carriles")]
    public GameObject[] carril1;
    public GameObject[] carril2;
    public GameObject[] carril3;   

    [Header("Luces de Fallos")]
    public GameObject[] lucesFallos;

    [Header("Timer Visual")]
    public GameObject[] indicadoresTiempo;

    [Header("Configuración")]
    public KeyCode teclaCarril1 = KeyCode.J;
    public KeyCode teclaCarril2 = KeyCode.K;
    public KeyCode teclaCarril3 = KeyCode.L;
    public float velocidadJuego = 1f;
    public float tiempoTotalJuego = 20f;

    [Header("UI")]
    public GameObject botonComenzar; 
    public GameObject botonCarril1;
    public GameObject botonCarril2;
    public GameObject botonCarril3;
    public TextMeshPro numeroResultado; // Add this line
    private BoxCollider carril1Collider;
    private BoxCollider carril2Collider;
    private BoxCollider carril3Collider;

    [Header("Audio")]
    [SerializeField] private AudioSource[] buttonSounds = new AudioSource[4]; 

    private float posicionOriginalBoton;
    private const float POSICION_PRESIONADO = -0.9891f;
    private const float DURACION_ANIMACION = 0.1f;
    
    private float tiempoRestante;
    private int fallos = 0;
    private bool juegoActivo = true;
    private Coroutine generarLucesCoroutine;
    private Coroutine[] moverLucesCoroutines = new Coroutine[3];
    public Button start;

    void Start()
    {
        
        juegoActivo = false;
        ApagarTodasLasLuces();
        if (botonComenzar != null)
        {
            posicionOriginalBoton = botonComenzar.transform.localPosition.y;
        }

        // Initialize and disable colliders
        carril1Collider = botonCarril1.GetComponent<BoxCollider>();
        carril2Collider = botonCarril2.GetComponent<BoxCollider>();
        carril3Collider = botonCarril3.GetComponent<BoxCollider>();
        SetCarrilesColliders(false);
    }

    public void ComenzarJuego() //Funcion para botón
    {
        if (!juegoActivo)
        {
            StartCoroutine(AnimarBoton());
        }
    }

    private IEnumerator AnimarBoton()
    {
        if (botonComenzar != null)
        {
            if (buttonSounds[0] != null)
            {
                buttonSounds[0].Play();
            }

            Vector3 posOriginal = botonComenzar.transform.localPosition;
            Vector3 posBajada = new Vector3(posOriginal.x, POSICION_PRESIONADO, posOriginal.z);

            float tiempoTranscurrido = 0;
            while (tiempoTranscurrido < DURACION_ANIMACION)
            {
                tiempoTranscurrido += Time.deltaTime;
                float t = tiempoTranscurrido / DURACION_ANIMACION;
                botonComenzar.transform.localPosition = Vector3.Lerp(posOriginal, posBajada, t);
                yield return null;
            }

            tiempoTranscurrido = 0;
            while (tiempoTranscurrido < DURACION_ANIMACION)
            {
                tiempoTranscurrido += Time.deltaTime;
                float t = tiempoTranscurrido / DURACION_ANIMACION;
                botonComenzar.transform.localPosition = Vector3.Lerp(posBajada, posOriginal, t);
                yield return null;
            }

            botonComenzar.transform.localPosition = posOriginal;
        }

        IniciarJuego();
    }

    private void LimpiarEstadoJuego()
    {
        StopAllCoroutines();
        
        generarLucesCoroutine = null;
        for (int i = 0; i < moverLucesCoroutines.Length; i++)
        {
            moverLucesCoroutines[i] = null;
        }
        
        juegoActivo = false;
        SetCarrilesColliders(false);
        fallos = 0;
        
        ApagarTodasLasLuces();
    }

    private void IniciarJuego()
    {
        LimpiarEstadoJuego();
        
        tiempoRestante = tiempoTotalJuego;
        juegoActivo = true;
        SetCarrilesColliders(true);
        generarLucesCoroutine = StartCoroutine(GenerarLuces());
    }

    void Update()
    {        
        if (!juegoActivo) return;

        tiempoRestante -= Time.deltaTime;
        ActualizarVisualizacionTiempo();

        if (tiempoRestante <= 0)
        {
            CompletarJuego();
            return;
        }

        // Inputs de teclado
        // if (Input.GetKeyDown(teclaCarril1)) VerificarPulsacion(0);
        // if (Input.GetKeyDown(teclaCarril2)) VerificarPulsacion(1);
        // if (Input.GetKeyDown(teclaCarril3)) VerificarPulsacion(2);
    }

    public void PulsarCarril1()
    {
        if (juegoActivo)
        {
            StartCoroutine(AnimarBotonCarril(botonCarril1, 0));
        }
    }

    public void PulsarCarril2()
    {
        if (juegoActivo)
        {
            StartCoroutine(AnimarBotonCarril(botonCarril2, 1));
        }
    }

    public void PulsarCarril3()
    {
        if (juegoActivo)
        {
            StartCoroutine(AnimarBotonCarril(botonCarril3, 2));
        }
    }

    private IEnumerator AnimarBotonCarril(GameObject boton, int carril)
    {
        if (boton != null)
        {
            if (buttonSounds[carril + 1] != null)
            {
                buttonSounds[carril + 1].Play();
            }

            Vector3 posOriginal = boton.transform.localPosition;
            Vector3 posBajada = new Vector3(posOriginal.x, POSICION_PRESIONADO, posOriginal.z);

            float tiempoTranscurrido = 0;
            while (tiempoTranscurrido < DURACION_ANIMACION)
            {
                tiempoTranscurrido += Time.deltaTime;
                float t = tiempoTranscurrido / DURACION_ANIMACION;
                boton.transform.localPosition = Vector3.Lerp(posOriginal, posBajada, t);
                yield return null;
            }

            tiempoTranscurrido = 0;
            while (tiempoTranscurrido < DURACION_ANIMACION)
            {
                tiempoTranscurrido += Time.deltaTime;
                float t = tiempoTranscurrido / DURACION_ANIMACION;
                boton.transform.localPosition = Vector3.Lerp(posBajada, posOriginal, t);
                yield return null;
            }

            boton.transform.localPosition = posOriginal;
        }
    }

    private void ActualizarVisualizacionTiempo()
    {
        int segundosTranscurridos = Mathf.FloorToInt(tiempoTotalJuego - tiempoRestante);
        for (int i = 0; i < indicadoresTiempo.Length; i++)
        {
            indicadoresTiempo[i].SetActive(i < segundosTranscurridos);
        }
    }

    public void VerificarPulsacion(int carril)
    {        
        GameObject[] carrilActual = carril == 0 ? carril1 : 
                                   carril == 1 ? carril2 : carril3;
                                   
        if (carrilActual[carrilActual.Length - 1].activeSelf)
        {
            carrilActual[carrilActual.Length - 1].SetActive(false);
        }
        else
        {
            Debug.Log($"Fallo en carril {carril+1} - No hay luz en zona de puntuación");
            RegistrarFallo();
        }
    }

    private void RegistrarFallo()
    {
        if (fallos < lucesFallos.Length)
        {
            lucesFallos[fallos].SetActive(true);
            Debug.Log($"¡Fallo! Total: {fallos+1}");
        }
        
        fallos++;
        
        if (fallos >= 3)
        {
            juegoActivo = false;
            StopAllCoroutines();
            StartCoroutine(ReiniciarPuntosConDelay());
        }
    }

    private IEnumerator ReiniciarPuntosConDelay()
    {
        foreach (var luzFallo in lucesFallos)
        {
            luzFallo.SetActive(true);
        }

        yield return new WaitForSeconds(2f);
        
        IniciarJuego();
    }

    private void ReiniciarPuntos()
    {
        StartCoroutine(ReiniciarPuntosConDelay());
    }

    private void CompletarJuego()
    {
        puzzlemanager = FindObjectOfType<PuzzleManager>();
        puzzlemanager.CompletePuzzle("GuitarHeroPuzzle");
        
        // Get and display the third digit
        if (numeroResultado != null)
        {
            int indexCode = 2;
            numeroResultado.text = $"**{PuzzleManager.numpadFinalCode[indexCode]}*";
            numeroResultado.gameObject.SetActive(true);

            char[] auxList = PuzzleManager.numpadActualCode.ToCharArray();
            auxList[indexCode] = PuzzleManager.numpadFinalCode[indexCode];
            string finalCharacters = "";
            for (int i = 0; i < auxList.Length; i++)
            {
                finalCharacters += auxList[i].ToString();
            }
            PuzzleManager.numpadActualCode = finalCharacters;
        }
        
        Debug.Log("¡Victoria! (desbloquea simon dice)");
    }

    IEnumerator GenerarLuces()
    {
        while (juegoActivo)
        {
            int carrilSeleccionado = Random.Range(0, 3);
            GameObject[] carrilActual = carrilSeleccionado == 0 ? carril1 : 
                                      carrilSeleccionado == 1 ? carril2 : carril3;
            
            moverLucesCoroutines[carrilSeleccionado] = StartCoroutine(MoverLuces(carrilActual, carrilSeleccionado));
            
            yield return new WaitForSeconds(velocidadJuego);
        }
    }

    IEnumerator MoverLuces(GameObject[] carril, int carrilIndex)
    {
        foreach (var luz in carril)
        {
            luz.SetActive(false);
        }
        
        for (int i = 0; i < carril.Length; i++)
        {
            if (i > 0) carril[i-1].SetActive(false);
            carril[i].SetActive(true);
            yield return new WaitForSeconds(velocidadJuego * 0.5f);
        }
        
        if (carril[carril.Length-1].activeSelf)
        {
            RegistrarFallo();
        }
        carril[carril.Length-1].SetActive(false);
    }

    private void ApagarTodasLasLuces()
    {
        foreach (var luz in carril1) luz.SetActive(false);
        foreach (var luz in carril2) luz.SetActive(false);
        foreach (var luz in carril3) luz.SetActive(false);
        foreach (var luz in lucesFallos) luz.SetActive(false);
        foreach (var indicador in indicadoresTiempo) indicador.SetActive(false);
    }

    private void SetCarrilesColliders(bool estado)
    {
        if (carril1Collider) carril1Collider.enabled = estado;
        if (carril2Collider) carril2Collider.enabled = estado;
        if (carril3Collider) carril3Collider.enabled = estado;
    }
}

