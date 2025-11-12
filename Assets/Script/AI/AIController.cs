using NUnit.Framework.Interfaces;
using UnityEngine;

/// <summary>
/// Controlador principal de la IA. Gestiona el estado actual y las transiciones
/// </summary>

public class AIController : MonoBehaviour
{
    [Header("AI Settings")]
    public Transform[] waypoints; // Para que el Diseñador asigne la ruta
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRadius = 10f;
    public float loseSightRadius = 15f;
    private AIState _currentState;

    [Header("Stun Settings")]
    public float stunDuration = 2f; // Duración del aturdimiento en segundos
    private Coroutine _stunCoroutine;

    public void Stun()
    {
        // Evita aplicar stun si ya está aturdido
        if (_currentState is StunState) return;

        ChangeState(new StunState(this));
    }


    private void Awake()
    {
        // El estado inicial
        ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        // Delega la lógica de actualización al estado actual
        // Principio de Responsabilidad Única
        _currentState?.UpdateState();
    }

    public void ChangeState(AIState newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    }
}