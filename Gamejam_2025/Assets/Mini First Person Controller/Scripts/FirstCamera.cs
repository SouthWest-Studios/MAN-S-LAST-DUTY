using UnityEngine;

public class FirstCamera : MonoBehaviour
{
    [SerializeField] private Transform player; // Referencia al jugador
    Transform character;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 1.8f, 0); // Offset para los ojos
    public float sensitivity = 2f;
    public float smoothing = 1.5f;

    private Vector2 velocity;
    private Vector2 frameVelocity;
    private Vector3 velocitySmoothDamp = Vector3.zero; // Necesario para SmoothDamp

    public Rewind rewindManager;
    public bool isPanelOpen = true;
    public CrosshairController crosshairController;
    public static FirstCamera instance;

    private float verticalRotation = 0f; // Para manejar la rotación vertical

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        character = player.GetComponentInParent<FirstPersonMovement>().transform;
    }

    private void LateUpdate()
    {
        if (isPanelOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            crosshairController.ShowCrosshair(false);
            return;
        }

        if (!rewindManager.isRewinding)
        {
            Cursor.lockState = CursorLockMode.Locked;
            crosshairController.ShowCrosshair(true);

            // Obtener el movimiento del mouse
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
            velocity += frameVelocity;

            // Limitar la rotación vertical (evita que la cámara haga un giro completo)
            velocity.y = Mathf.Clamp(velocity.y, -90, 90);

            // Aplicar la rotación
            transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
            character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up); // Rotación horizontal (girar el personaje)
        }

        // Posición deseada con el offset
        Vector3 targetPosition = player.position + player.TransformDirection(cameraOffset);

        // Suavizar la posición con SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocitySmoothDamp, 0.1f);
    }
}
