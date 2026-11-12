using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;

    [Header("Jump & Gravity Settings")]
    public float jumpHeight = 1.5f;
    public float gravity = -20.0f;
    public float fallMultiplier = 1.8f;
    public float terminalVelocity = -50.0f;

    [Header("Look Settings")]
    public float mouseSensitivity = 200.0f;
    public Transform cameraTransform;

    [Header("Interaction Settings")]
    public float interactDistance = 3.0f;
    public LayerMask interactableLayer;
    public UIManager uiManager;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Khóa di chuyển/xoay camera khi UI đang mở
        if (uiManager != null && uiManager.isUIOpen) return;

        HandleLook();
        HandleMovement();
        
    }

    void HandleLook()
    {
        if (cameraTransform == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (velocity.y < 0)
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        velocity.y = Mathf.Max(velocity.y, terminalVelocity);
        controller.Move(velocity * Time.deltaTime);
    }

    
}