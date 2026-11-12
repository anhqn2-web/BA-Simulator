using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Cài đặt Raycast")]
    public float interactionDistance = 3.0f; // Thông số chuẩn theo TDD
    public LayerMask interactableLayer;
    public Transform cameraTransform;

    [Header("Giao diện UI")]
    public TextMeshProUGUI promptText;
    public UIManager uiManager;

    private IInteractable currentInteractable;

    void Update()
    {
        // Ẩn thông báo và khóa tương tác nếu đang mở Menu Pause
        if (uiManager != null && uiManager.isUIOpen)
        {
            HidePrompt();
            return;
        }

        CheckForInteractable();

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact();
        }
    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
        {
            // Dùng GetComponentInParent để nhận diện ngay cả khi Collider nằm ở object con
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                ShowPrompt(interactable.GetPrompt());
                return;
            }
        }

        currentInteractable = null;
        HidePrompt();
    }

    void ShowPrompt(string text)
    {
        if (promptText != null)
        {
            promptText.text = text;
            promptText.gameObject.SetActive(true);
        }
    }

    void HidePrompt()
    {
        if (promptText != null && promptText.gameObject.activeSelf)
        {
            promptText.gameObject.SetActive(false);
        }
    }
}