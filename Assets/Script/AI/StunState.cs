using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StunState : AIState
{
    public StunState(AIController controller) : base(controller) { }

    public override void OnEnter()
    {
        Debug.Log("Enemigo aturdido 🌀");

        m_agent.isStopped = true; // Detener movimiento
        m_agent.velocity = Vector3.zero;

        // Iniciar corrutina para volver al patrullaje
        m_controller.StartCoroutine(StunTimer());
    }

    public override void UpdateState()
    {
        // No hace nada mientras está aturdido
    }

    public override void OnExit()
    {
        m_agent.isStopped = false; // Reactivar movimiento
    }

    private IEnumerator StunTimer()
    {
        yield return new WaitForSeconds(m_controller.stunDuration);
        m_controller.ChangeState(new PatrolState(m_controller));
    }
}
