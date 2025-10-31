using AAA.Outline;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Gestiona la capacidad del jugador para interactuar con objetos en el mundo
/// Utiliza un Raycast para detectar objetos que implementen la interfaz IInteractable
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float _interactionDistance = 2f;

    // Eventos que otras clases pueden escuchar o ser usadas en el Inspector de Unity
    [Header("Events")]
    [SerializeField] private UnityEvent<GameObject> _onTargetFocused;
    [SerializeField] private UnityEvent<GameObject> _onTargetLost;

    private Camera _mainCamera;
    private PlayerInputActions _inputActions;
    private GameObject _current;

    private void Awake()
    {
        _mainCamera = Camera.main;
        if (_mainCamera == null)
        {
            Debug.LogWarning("PlayerInteractor: No se encontró cámara principal al iniciar. Intentando asignarla más tarde...");
            StartCoroutine(AssignCameraWhenReady());
        }

        _inputActions = new PlayerInputActions();
    }

    private System.Collections.IEnumerator AssignCameraWhenReady()
    {
        yield return new WaitUntil(() => Camera.main != null);
        _mainCamera = Camera.main;
        Debug.Log("PlayerInteractor: Cámara asignada correctamente después de cargar la escena.");
    }



    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        _inputActions.Player.Interact.performed -= OnInteract;
        _inputActions.Player.Disable();
    }

    private void Update()
    {
        // Verificamos en cada frame si tenemos un objeto interactuable en frente
        DetectAndShowFeedback();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        PerformRaycastInteraction();
    }

    // Lanza un rayo y ejecuta la interaccion del objeto en frente
    private void PerformRaycastInteraction()
    {
        Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    // Detectamos y mostramos retroalimentacion para el usuario
    private void DetectAndShowFeedback()
    {
        // Evitar error si la cámara o el objeto actual fueron destruidos
        if (_mainCamera == null) return;
        if (this == null) return;

        Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance))
        {
            if (hit.collider != null && hit.collider.GetComponent<IInteractable>() != null)
            {
                if (_current == null) return;
                SetFocus(hit.collider.gameObject);
            }
            else
            {
                if (_current == null) return;
                ClearFocus();
            }
        }
        else
        {
            if (_current == null) return;
            ClearFocus();
        }
    }

    /// <summary>
    /// Asigna el objeto como focus aplicando el Outline
    /// </summary>
    /// <param name="target"></param>
    private void SetFocus(GameObject target)
    {
        if (_current == target) return;

        if (_current != null)
        {
            _current.GetComponent<OutlineComponent>()?.Remove();
            _onTargetLost?.Invoke(_current);
        }

        _current = target;
        _current.GetComponent<OutlineComponent>()?.Apply();
        _onTargetFocused?.Invoke(_current);

        GameEvents.TriggerTargetFocused(_current);
    }

    /// <summary>
    /// Limpia el focus al objeto al perderlo de vista
    /// </summary>
    private void ClearFocus()
    {
        if (_current == null) return;

        _current.GetComponent<OutlineComponent>()?.Remove();

        _onTargetLost?.Invoke(_current);

        GameEvents.TriggerTargetLost(_current);

        _current = null;
    }
}