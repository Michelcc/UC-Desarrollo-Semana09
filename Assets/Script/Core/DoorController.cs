using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Animation")]
    public Animator _doorAnimator;

    private bool _isOpen = false;

    public void Interact()
    {
        _isOpen = !_isOpen;
        Debug.Log(_isOpen ? "La puerta se ha ABIERTO." : "La puerta se ha CERRADO.");

        _doorAnimator.SetBool("IsOpen", _isOpen);
    }
}
