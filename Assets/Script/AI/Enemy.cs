using UnityEngine;

public class Enemy : MonoBehaviour, IInteractable
{
    private AIController _controller;

    private void Awake()
    {
        // Tomamos el componente AIController del enemigo
        _controller = GetComponent<AIController>();
    }

    // Este método se ejecuta cuando algo "interactúa" con el enemigo
    public void Interact()
    {
        // Cuando el Rifle dispare al enemigo, este se aturde
        if (_controller != null)
        {
            _controller.Stun();
        }
    }
}
