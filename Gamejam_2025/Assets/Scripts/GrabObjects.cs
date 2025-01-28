using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GrabObjects : MonoBehaviour
{
    [Header("Grab Settings")]
    [SerializeField] private float grabDistance = 3f;
    [SerializeField] private float holdDistance = 2f;
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;  // Cambiar a una única tecla de interacción

    [Header("Visual Feedback")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color grabbableColor = Color.green;

    private Camera mainCamera;
    private GameObject heldObject;
    private Rigidbody heldRigidbody;
    private bool isHolding = false;
    private UnityEngine.UI.Image crosshairImage;
    private GameObject currentGrabbable;
    private FetusScript fetus;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (crosshair != null)
        {
            crosshairImage = crosshair.GetComponent<UnityEngine.UI.Image>();
        }
        // Obtener referencia a FetusScript
        fetus = FindObjectOfType<FetusScript>();
    }

    private void Update()
    {
        if (!isHolding)
        {
            CheckForGrabbableObject();
            if (currentGrabbable != null && Input.GetKeyDown(interactKey))  // Cambiar a interactKey
            {
                GrabObject(currentGrabbable);
            }
        }
        else
        {
            if (Input.GetKeyDown(interactKey))  // Cambiar a interactKey
            {
                DropObject();
            }
            else
            {
                HoldObject();
            }
        }
    }

    private void CheckForGrabbableObject()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            if (hit.collider.CompareTag("Grabbable"))
            {
                currentGrabbable = hit.collider.gameObject;
                UpdateCrosshair(true);
                return;
            }
        }

        currentGrabbable = null;
        UpdateCrosshair(false);
    }

    private void UpdateCrosshair(bool isGrabbable)
    {
        if (crosshairImage != null)
        {
            crosshairImage.color = isGrabbable ? grabbableColor : normalColor;
        }
    }

    private void GrabObject(GameObject objToGrab)
    {
        heldObject = objToGrab;
        heldRigidbody = heldObject.GetComponent<Rigidbody>();
        
        if (heldRigidbody != null)
        {
            heldRigidbody.useGravity = false;
            heldRigidbody.freezeRotation = true;
            heldRigidbody.drag = 10;
            isHolding = true;

            // Actualizar currentHint en FetusScript con el nombre del objeto
            if (fetus != null)
            {
                fetus.currentHint = heldObject.name;
            }
        }
    }

    private void HoldObject()
    {
        if (heldObject == null) return;

        // Calcular la posición objetivo frente a la cámara
        Vector3 targetPosition = mainCamera.transform.position + mainCamera.transform.forward * holdDistance;
        
        if (heldRigidbody != null)
        {
            Vector3 velocity = (targetPosition - heldObject.transform.position) * smoothSpeed;
            heldRigidbody.velocity = velocity;
        }
        else
        {
            heldObject.transform.position = Vector3.Lerp(
                heldObject.transform.position, 
                targetPosition, 
                Time.deltaTime * smoothSpeed
            );
        }
    }

    private void DropObject()
    {
        if (heldObject == null) return;

        if (heldRigidbody != null)
        {
            heldRigidbody.useGravity = true;
            heldRigidbody.freezeRotation = false;
            heldRigidbody.drag = 1;
            heldRigidbody = null;
            

        }

        heldObject = null;
        isHolding = false;
    }

    public GameObject GetHeldObject()
    {
        return heldObject;
    }

    public void ForceDropObject()
    {
        heldObject = null;
        heldRigidbody = null;
        isHolding = false;
        
        if (fetus != null)
        {
            fetus.currentHint = "";
        }
    }
}
