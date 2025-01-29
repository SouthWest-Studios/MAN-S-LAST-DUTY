using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public static CrosshairController instance;
    public GameObject crosshair; // Arrastra aquí el punto de apuntado.


    private void Awake()
    {
        if(instance == null) { instance = this; }
    }

    void Start()
    {
        // Asegúrate de que el punto esté activo al inicio.
        crosshair.SetActive(true);
    }

    public void ShowCrosshair(bool show)
    {
        // Activa o desactiva el punto.
        crosshair.SetActive(show);
    }
}