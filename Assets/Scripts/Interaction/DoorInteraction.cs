using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    public float openAngle = 90.0f;
    public float speed = 3.0f;
    private bool isOpen = false;
    private Quaternion defaultRotation;
    private Quaternion openRotation;

    void Start()
    {
        defaultRotation = transform.localRotation;
        openRotation = defaultRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : defaultRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * speed);
    }

    public string GetPrompt()
    {
        return isOpen ? "Press [E] to close" : "Press [E] to open";
    }

    public void Interact()
    {
        isOpen = !isOpen;
    }
}