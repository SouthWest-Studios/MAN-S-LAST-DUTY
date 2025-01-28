using UnityEngine;
using UnityEngine.UI; // Añadir esta línea
using System.Collections; // Añadir esta línea

public class CordonUmbilical : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform lupaTransform;
    [SerializeField] private Transform[] pivotPoints;
    [SerializeField] private GameObject targetCanvas; // Añadir referencia al canvas
    [SerializeField] private RectTransform canvasRectTransform; // Añadir esta referencia
    [SerializeField] private Transform[] cameraPoints; // Cambiar de Camera[] a Transform[]
    [SerializeField] private float cameraTransitionSpeed = 2f; // Velocidad de transición de la cámara
    [SerializeField] private Camera puzzleCamera; // Nueva referencia para la cámara del puzzle
    [SerializeField] private Transform initialCameraPoint; // Añadir referencia al punto inicial de la cámara
    [SerializeField] private GameObject[] pivotObjects; // Objetos dentro de los pivotes de la lupa
    private PuzzleManager puzzleManager; // Añadir referencia al PuzzleManager
    private FetusScript fetus; // Añadir referencia al FetusScript
    [SerializeField] private ObjectInteraction objectInteraction; // Añadir referencia al ObjectInteraction

    [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material hoverMaterial;
    [SerializeField] private Material wrongPivotMaterial; // Nuevo material para pivotes incorrectos
    [SerializeField] private Material correctPivotMaterial; // Nuevo material para pivotes correctos
    [SerializeField] private Material wrongObjectMaterial; // Nuevo material para objetos incorrectos

    private Material[] originalMaterials;
    private MeshRenderer hoveredRenderer;

    [Header("Movement Settings")]
    [SerializeField] private float minY = -5f;
    [SerializeField] private float maxY = 5f;
    [SerializeField] private float minZ = -5f;
    [SerializeField] private float maxZ = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float snapDistance = 1f;
    [SerializeField] private float unSnapDistance = 2f; // Nueva variable para controlar la distancia de unsnap

    [Header("Selection Objects")]
    [SerializeField] private GameObject[] selectableObjectsGroup1; // Objetos del primer punto
    [SerializeField] private GameObject[] selectableObjectsGroup2; // Objetos del segundo punto
    [SerializeField] private GameObject[] selectableObjectsGroup3; // Objetos del tercer punto
    [SerializeField] private GameObject[] selectableObjectsGroup4; // Objetos del cuarto punto
    [SerializeField] private float hoverScaleSpeed = 0.0005f;

    [Header("Puzzle Settings")]
    [SerializeField] public static int correctGroupIndex = 0; // Grupo 1 (índice 0)
    [SerializeField] public static int correctObjectInGroupIndex = 0; // Primer objeto (índice 0)

    private bool isPuzzleActive = false;
    private bool isSnapped = false;
    private bool isTransitioning = false;
    private bool isFocused = false;
    private Camera currentMainCamera; // Eliminar mainCamera ya que no la usaremos
    private Vector3 lastRayHitPoint; // Añadir esta variable
    private bool hasValidRaycast; // Añadir esta variable
    private Transform hoveredObject = null;
    private Transform selectedObject = null;  // Nueva variable para el objeto seleccionado
    private MeshRenderer selectedRenderer = null;  // Nueva variable para el renderer del objeto seleccionado
    private Vector3[] originalScales;
    private bool gameWon = false;
    private int currentObjectGroup = -1; // Grupo actual de objetos
    private bool isPuzzleComplete = false; // Añadir esta variable

    [Header("Post-Selection")]
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private float deactivationDelay = 2f;
    [SerializeField] private GameObject prefabToSpawn; // Añadir esta variable
    [SerializeField] private Vector3 spawnPosition; // Opcional: posición donde aparecerá el prefab
    [SerializeField] private GameObject[] objectsToDeactivate; // Añadir este array

    void Start()
    {
        currentMainCamera = puzzleCamera;
        
        // Calcular el total de objetos para el array
        int totalObjects = 0;
        if (selectableObjectsGroup1 != null) totalObjects += selectableObjectsGroup1.Length;
        if (selectableObjectsGroup2 != null) totalObjects += selectableObjectsGroup2.Length;
        if (selectableObjectsGroup3 != null) totalObjects += selectableObjectsGroup3.Length;
        if (selectableObjectsGroup4 != null) totalObjects += selectableObjectsGroup4.Length;

        originalScales = new Vector3[totalObjects];
        originalMaterials = new Material[totalObjects];
        int currentIndex = 0;

        // Guardar escalas originales y desactivar objetos
        currentIndex = SaveScalesAndDeactivate(selectableObjectsGroup1, currentIndex);
        currentIndex = SaveScalesAndDeactivate(selectableObjectsGroup2, currentIndex);
        currentIndex = SaveScalesAndDeactivate(selectableObjectsGroup3, currentIndex);
        SaveScalesAndDeactivate(selectableObjectsGroup4, currentIndex);

        // Guardar materiales originales y desactivar objetos
        SaveMaterialsAndDeactivate(selectableObjectsGroup1, currentIndex);
        SaveMaterialsAndDeactivate(selectableObjectsGroup2, currentIndex);
        SaveMaterialsAndDeactivate(selectableObjectsGroup3, currentIndex);
        SaveMaterialsAndDeactivate(selectableObjectsGroup4, currentIndex);

        // Desactivar los objetos de los pivotes al inicio
        foreach (GameObject pivotObj in pivotObjects)
        {
            if (pivotObj != null)
            {
                pivotObj.SetActive(false);
            }
        }

        // Buscar la referencia al ObjectInteraction
    }

    public void ActivatePuzzle()
    {
        isPuzzleActive = true;
        // Activar los objetos de los pivotes cuando empieza el puzzle
        foreach (GameObject pivotObj in pivotObjects)
        {
            if (pivotObj != null)
            {
                pivotObj.SetActive(true);
            }
        }
    }

    void Update()
    {
        // Añadir verificación al inicio del Update
        if (isPuzzleComplete) return;

        if (targetCanvas != null && targetCanvas.activeSelf && !isPuzzleActive)
        {
            ActivatePuzzle();
        }

        // Modificar la lógica de enfoque/desenfoque con E
        if (isSnapped && Input.GetKeyDown(KeyCode.E) && !isTransitioning)
        {
            if (!isFocused)
            {
                int currentPivotIndex = GetCurrentPivotIndex();
                if (currentPivotIndex != -1)
                {
                    isFocused = true;
                    StartCoroutine(TransitionToPoint(cameraPoints[currentPivotIndex]));
                }
            }
            else
            {
                StartCoroutine(TransitionToPoint(initialCameraPoint));
                isFocused = false;
            }
        }

        if (isFocused && !isTransitioning && !gameWon)
        {
            HandleObjectSelection();
        }

        if (!isPuzzleActive || isFocused) return;

        if (!isSnapped)
        {
            MoveLupa();
            CheckPivotSnapping();
        }
        else
        {
            CheckUnsnapping();
        }
    }

    private void MoveLupa()
    {
        Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);
        Plane movementPlane = new Plane(Vector3.right, Vector3.zero); // Plano YZ

        hasValidRaycast = movementPlane.Raycast(ray, out float enter);
        if (hasValidRaycast)
        {
            lastRayHitPoint = ray.GetPoint(enter);
            
            // Limitar el movimiento a los rangos establecidos
            Vector3 newPosition = lupaTransform.position;
            newPosition.y = Mathf.Clamp(lastRayHitPoint.y, minY, maxY);
            newPosition.z = Mathf.Clamp(lastRayHitPoint.z, minZ, maxZ);
            
            lupaTransform.position = Vector3.Lerp(lupaTransform.position, newPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void CheckPivotSnapping()
    {
        // Solo verificar snap si no estamos ya snapped
        foreach (Transform pivot in pivotPoints)
        {
            float distance = Vector2.Distance(
                new Vector2(lupaTransform.position.y, lupaTransform.position.z),
                new Vector2(pivot.position.y, pivot.position.z)
            );

            if (distance < snapDistance)
            {
                isSnapped = true;
                lupaTransform.position = pivot.position;
                return;
            }
        }
        // Eliminar esta línea para evitar que se establezca a false constantemente
        // isSnapped = false;
    }

    private void CheckUnsnapping()
    {
        Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);
        Plane movementPlane = new Plane(Vector3.right, Vector3.zero);

        if (movementPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            
            float distance = Vector2.Distance(
                new Vector2(hitPoint.y, hitPoint.z),
                new Vector2(lupaTransform.position.y, lupaTransform.position.z)
            );

            // Solo cambiar isSnapped si la distancia es suficiente
            if (distance > unSnapDistance)
            {
                isSnapped = false;
                // Mover inmediatamente la lupa a la nueva posición para evitar "saltos"
                Vector3 newPosition = lupaTransform.position;
                newPosition.y = Mathf.Clamp(hitPoint.y, minY, maxY);
                newPosition.z = Mathf.Clamp(hitPoint.z, minZ, maxZ);
                lupaTransform.position = newPosition;
            }
        }
    }

    private int GetCurrentPivotIndex()
    {
        for (int i = 0; i < pivotPoints.Length; i++)
        {
            if (Vector3.Distance(lupaTransform.position, pivotPoints[i].position) < 0.01f)
                return i;
        }
        return -1;
    }

    private IEnumerator TransitionToPoint(Transform targetPoint)
    {
        isTransitioning = true;

        // Desactivar grupo anterior si existe, excepto el objeto seleccionado
        GameObject[] currentGroup = GetCurrentGroup();
        if (currentGroup != null)
        {
            foreach (var obj in currentGroup)
            {
                if (obj != null && obj != selectedObject?.gameObject)
                {
                    obj.SetActive(false);
                }
            }
        }

        bool isReturning = targetPoint == initialCameraPoint;
        
        // Manejar los objetos de los pivotes según si estamos regresando o focuseando
        foreach (GameObject pivotObj in pivotObjects)
        {
            if (pivotObj != null)
            {
                pivotObj.SetActive(isReturning);
            }
        }

        // Activar nuevo grupo si no es el punto inicial
        if (!isReturning)
        {
            currentObjectGroup = GetCurrentPivotIndex();
            currentGroup = GetCurrentGroup();
            if (currentGroup != null)
            {
                foreach (var obj in currentGroup)
                {
                    if (obj != null && obj != selectedObject?.gameObject)
                    {
                        obj.SetActive(true);
                    }
                }
            }
        }
        else
        {
            currentObjectGroup = GetCurrentPivotIndex(); // Mantener el grupo actual en lugar de resetearlo
        }

        // Guardar posición y rotación inicial
        Vector3 startPosition = currentMainCamera.transform.position;
        Quaternion startRotation = currentMainCamera.transform.rotation;

        // Usar la posición y rotación del punto objetivo
        Vector3 targetPosition = targetPoint.position;
        Quaternion targetRotation = targetPoint.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * cameraTransitionSpeed;
            float t = elapsedTime;

            // Interpolar posición y rotación
            currentMainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            currentMainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        // Asegurarse de que la cámara llegue exactamente a la posición final
        currentMainCamera.transform.position = targetPosition;
        currentMainCamera.transform.rotation = targetRotation;

        isTransitioning = false;
    }

    private void HandleObjectSelection()
    {
        if (currentObjectGroup == -1) return;

        Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Resetear objeto hover anterior (pero no el seleccionado)
        if (hoveredObject != null && hoveredObject != selectedObject)
        {
            // Restaurar escala original solo si no es el objeto seleccionado
            hoveredObject.localScale = GetOriginalScale(hoveredObject.gameObject);
            
            // Aplicar material por defecto solo si no es el objeto seleccionado
            if (hoveredRenderer != null && hoveredRenderer != selectedRenderer && defaultMaterial != null)
            {
                hoveredRenderer.material = defaultMaterial;
            }
            
            hoveredObject = null;
            hoveredRenderer = null;
        }

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (IsObjectInCurrentGroup(hitObject))
            {
                // Solo aplicar hover si no es el objeto seleccionado
                if (hit.transform != selectedObject)
                {
                    hoveredObject = hit.transform;
                    hoveredRenderer = hitObject.GetComponent<MeshRenderer>();

                    // Aplicar escala de hover
                    hoveredObject.localScale += new Vector3(hoverScaleSpeed, hoverScaleSpeed, hoverScaleSpeed);
                    
                    // Aplicar material de hover
                    if (hoveredRenderer != null && hoverMaterial != null)
                    {
                        hoveredRenderer.material = hoverMaterial;
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    // Si es el mismo objeto que ya está seleccionado, no hacer nada
                    if (selectedObject == hit.transform)
                        return;

                    // Resetear objeto seleccionado anterior
                    if (selectedObject != null)
                    {
                        selectedObject.localScale = GetOriginalScale(selectedObject.gameObject);
                        if (selectedRenderer != null)
                        {
                            selectedRenderer.material = defaultMaterial;
                        }
                    }

                    // Establecer nuevo objeto seleccionado
                    selectedObject = hit.transform;
                    selectedRenderer = hitObject.GetComponent<MeshRenderer>();
                    
                    // Establecer escala exacta para el objeto seleccionado
                    selectedObject.localScale = GetOriginalScale(selectedObject.gameObject) + new Vector3(hoverScaleSpeed * 2, hoverScaleSpeed * 2, hoverScaleSpeed * 2);
                    
                    if (selectedRenderer != null && hoverMaterial != null)
                    {
                        selectedRenderer.material = hoverMaterial;
                    }

                    OnObjectSelected(hitObject);
                }
            }
        }
    }

    private void OnObjectSelected(GameObject selectedObject)
    {
        // Configurar FetusScript para la verificación posterior
        fetus = FindObjectOfType<FetusScript>();
        fetus.currentHint = "UmbilicalCord";

        // Llamar a EndFocusTransition inmediatamente después de seleccionar
        if (objectInteraction != null)
        {
            objectInteraction.EndFocusTransition();
        }

        // Iniciar la corrutina para desactivar los scripts y activar el objeto
        StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(deactivationDelay);

        // Instanciar el prefab antes de desactivar
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }

        // Activar el objeto especificado
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        // Desactivar el script ObjectInteraction y su collider
        if (objectInteraction != null)
        {
            objectInteraction.enabled = false;
            Collider collider = objectInteraction.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        // Desactivar todos los objetos del array
        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        // Marcar el puzzle como completado
        isPuzzleComplete = true;
        
        // Desactivar elementos visuales
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(false);
        }
        foreach (GameObject pivotObj in pivotObjects)
        {
            if (pivotObj != null)
            {
                pivotObj.SetActive(false);
            }
        }
        
        //gameObject.SetActive(false);
    }

    private void EndPuzzle()
    {
        // Resetear objeto seleccionado al finalizar
        if (selectedObject != null)
        {
            selectedObject.localScale = GetOriginalScale(selectedObject.gameObject);
            if (selectedRenderer != null)
            {
                selectedRenderer.material = wrongObjectMaterial;
            }
            selectedObject = null;
            selectedRenderer = null;
        }

        StartCoroutine(TransitionToPoint(initialCameraPoint));
        isFocused = false;

        if (FindObjectOfType<FirstPersonLook>() is FirstPersonLook fpsLook)
        {
            fpsLook.isPanelOpen = false;
        }
    }

    private bool IsObjectInCurrentGroup(GameObject obj)
    {
        GameObject[] currentGroup = GetCurrentGroup();
        return currentGroup != null && System.Array.IndexOf(currentGroup, obj) != -1;
    }

    private Vector3 GetOriginalScale(GameObject obj)
    {
        int currentIndex = 0;
        
        // Buscar en grupo 1
        if (selectableObjectsGroup1 != null)
        {
            int index = System.Array.IndexOf(selectableObjectsGroup1, obj);
            if (index != -1) return originalScales[currentIndex + index];
            currentIndex += selectableObjectsGroup1.Length;
        }

        // Buscar en grupo 2
        if (selectableObjectsGroup2 != null)
        {
            int index = System.Array.IndexOf(selectableObjectsGroup2, obj);
            if (index != -1) return originalScales[currentIndex + index];
            currentIndex += selectableObjectsGroup2.Length;
        }

        // Buscar en grupo 3
        if (selectableObjectsGroup3 != null)
        {
            int index = System.Array.IndexOf(selectableObjectsGroup3, obj);
            if (index != -1) return originalScales[currentIndex + index];
            currentIndex += selectableObjectsGroup3.Length;
        }

        // Buscar en grupo 4
        if (selectableObjectsGroup4 != null)
        {
            int index = System.Array.IndexOf(selectableObjectsGroup4, obj);
            if (index != -1) return originalScales[currentIndex + index];
        }

        return Vector3.one;
    }

    private Material GetOriginalMaterial(GameObject obj)
    {
        int currentIndex = 0;
        
        // Buscar en cada grupo igual que en GetOriginalScale
        if (selectableObjectsGroup1 != null)
        {
            int index = System.Array.IndexOf(selectableObjectsGroup1, obj);
            if (index != -1) return originalMaterials[currentIndex + index];
            currentIndex += selectableObjectsGroup1.Length;
        }
        if (selectableObjectsGroup2 != null)
        {
            int index = System.Array.IndexOf(selectableObjectsGroup2, obj);
            if (index != -1) return originalMaterials[currentIndex + index];
            currentIndex += selectableObjectsGroup2.Length;
        }
        if (selectableObjectsGroup3 != null)
        {
            int index = System.Array.IndexOf(selectableObjectsGroup3, obj);
            if (index != -1) return originalMaterials[currentIndex + index];
            currentIndex += selectableObjectsGroup3.Length;
        }
        if (selectableObjectsGroup4 != null)
        {
            int index = System.Array.IndexOf(selectableObjectsGroup4, obj);
            if (index != -1) return originalMaterials[currentIndex + index];
        }

        return defaultMaterial;
    }

    private void OnDrawGizmos()
    {
        // Dibujar el raycast si estamos en modo de juego
        if (Application.isPlaying && puzzleCamera != null)
        {
            Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(ray.origin, ray.direction * 100f);

            if (hasValidRaycast)
            {
                // Dibujar el punto de impacto
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(lastRayHitPoint, 0.1f);
            }
        }

        // Dibujar los rangos de snap
        if (pivotPoints == null) return;

        // Dibujar el rango de snap para cada punto
        foreach (Transform pivot in pivotPoints)
        {
            if (pivot == null) continue;

            // Dibujar el rango de snap normal
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pivot.position, snapDistance);

            // Dibujar el rango de unsnap usando la nueva variable
            Gizmos.color = new Color(1, 0.5f, 0, 0.3f);
            Gizmos.DrawWireSphere(pivot.position, unSnapDistance);
        }

        // Si la lupa está snapped, mostrar una línea al punto actual
        if (isSnapped && lupaTransform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(lupaTransform.position, GetCurrentSnapPoint().position);
        }
    }

    private Transform GetCurrentSnapPoint()
    {
        foreach (Transform pivot in pivotPoints)
        {
            if (Vector3.Distance(lupaTransform.position, pivot.position) < 0.01f)
                return pivot;
        }
        return null;
    }

    private GameObject[] GetCurrentGroup()
    {
        switch(currentObjectGroup)
        {
            case 0: return selectableObjectsGroup1;
            case 1: return selectableObjectsGroup2;
            case 2: return selectableObjectsGroup3;
            case 3: return selectableObjectsGroup4;
            default: return null;
        }
    }

    private int SaveScalesAndDeactivate(GameObject[] group, int startIndex)
    {
        if (group == null) return startIndex;
        
        for (int i = 0; i < group.Length; i++)
        {
            if (group[i] != null)
            {
                originalScales[startIndex + i] = group[i].transform.localScale;
                group[i].SetActive(false);
            }
        }
        return startIndex + group.Length;
    }

    private int SaveMaterialsAndDeactivate(GameObject[] group, int startIndex)
    {
        if (group == null) return startIndex;
        
        for (int i = 0; i < group.Length; i++)
        {
            if (group[i] != null)
            {
                MeshRenderer renderer = group[i].GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    originalMaterials[startIndex + i] = renderer.material;
                    renderer.material = defaultMaterial;
                }
                group[i].SetActive(false);
            }
        }
        return startIndex + group.Length;
    }

    public void CheckPuzzle()
    {
        GameObject[] currentGroup = GetCurrentGroup();
        int currentPivotIndex = GetCurrentPivotIndex();
        
        if (currentGroup == null || selectedObject == null) 
        {
            return;
        }

        int selectedIndex = System.Array.IndexOf(currentGroup, selectedObject.gameObject);
        bool isCorrect = (currentObjectGroup == correctGroupIndex && selectedIndex == correctObjectInGroupIndex);
        
        // Cambiar material del pivot según si es el grupo correcto o no
        if (currentPivotIndex >= 0 && currentPivotIndex < pivotObjects.Length)
        {
            MeshRenderer pivotRenderer = pivotObjects[currentPivotIndex].GetComponent<MeshRenderer>();
            if (pivotRenderer != null)
            {
                if (currentPivotIndex == correctGroupIndex)
                {
                    pivotRenderer.material = correctPivotMaterial;
                }
                else
                {
                    pivotRenderer.material = wrongPivotMaterial;
                }
            }
        }

        // Aplicar material al objeto seleccionado según si es correcto o no
        if (selectedRenderer != null)
        {
            selectedRenderer.material = isCorrect ? correctPivotMaterial : wrongObjectMaterial;
            Debug.Log(selectedObject.name + " is correct: " + isCorrect, selectedRenderer.material);
        }

        if (isCorrect)
        {
            puzzleManager = FindObjectOfType<PuzzleManager>();
            puzzleManager.CompletePuzzle("UmbilicalCord");
        }
        
        EndPuzzle();
    }
}