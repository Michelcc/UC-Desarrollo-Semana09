using System.Collections; 
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona el estado principal del juego, como jugar, ganar o perder.
/// Implementa el patrón Singleton para un acceso global único.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Playing, Victory, Loss }
    private GameState _currentState;

    [Header("Gameplay Settings")]
    [SerializeField] private int _objectivesToWin = 3;
    private int _objectivesCompleted = 0;

    [Header("Tiempo de juego")]
    [SerializeField] private float _timeLimit = 60f;  // tiempo total en segundos
    private bool _isGameActive = true;                // controla si el temporizador sigue corriendo

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnObjectiveActivated += HandleObjectiveActivated;
    }

    private void OnDisable()
    {
        GameEvents.OnObjectiveActivated -= HandleObjectiveActivated;
    }

    private void Start()
    {
        ChangeState(GameState.Playing);
        StartCoroutine(CountdownTimer()); // ⏱️ inicia el contador al comenzar
    }

    private void HandleObjectiveActivated()
    {
        if (_currentState != GameState.Playing) return;

        _objectivesCompleted++;
        Debug.Log($"Objetivo completado. Progreso: {_objectivesCompleted}/{_objectivesToWin}");

        if (_objectivesCompleted >= _objectivesToWin)
        {
            ChangeState(GameState.Victory);
        }
    }

    /// <summary>
    /// Corrutina que gestiona la cuenta atrás del juego.
    /// </summary>
    private IEnumerator CountdownTimer()
    {
        float remainingTime = _timeLimit;

        while (remainingTime > 0 && _isGameActive && _currentState == GameState.Playing)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;

            Debug.Log($"⏳ Tiempo restante: {remainingTime}s");

            // Actualiza la UI (si tienes un texto de tiempo en pantalla)
            if (UIManager.Instance != null)
                UIManager.Instance.UpdateTimer(remainingTime);
        }

        // Si el tiempo llega a 0 y el jugador no ganó
        if (_isGameActive && _currentState == GameState.Playing)
        {
            ChangeState(GameState.Loss);
        }
    }

    /// <summary>
    /// Corrutina que gestiona la secuencia de eventos cuando el jugador gana.
    /// </summary>
    private IEnumerator VictorySequence()
    {
        Debug.Log("🎉 SECUENCIA DE VICTORIA INICIADA");
        _isGameActive = false;

        FindFirstObjectByType<FirstPersonController>().enabled = false;
        yield return new WaitForSeconds(1f);

        if (UIManager.Instance != null)
            UIManager.Instance.ShowVictoryPanel();

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Corrutina de derrota cuando el tiempo se acaba o se cumple una condición de pérdida.
    /// </summary>
    private IEnumerator LossSequence()
    {
        Debug.Log("💀 SECUENCIA DE DERROTA INICIADA");
        _isGameActive = false;

        FindFirstObjectByType<FirstPersonController>().enabled = false;
        yield return new WaitForSeconds(1f);

        if (UIManager.Instance != null)
            UIManager.Instance.ShowLossPanel();

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }

    public void ChangeState(GameState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        Debug.Log($"🔄 Nuevo estado de juego: {_currentState}");

        switch (_currentState)
        {
            case GameState.Playing:
                // lógica de inicio
                break;
            case GameState.Victory:
                StartCoroutine(VictorySequence());
                break;
            case GameState.Loss:
                StartCoroutine(LossSequence());
                break;
        }
    }
}
