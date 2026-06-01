using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerTPS : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump & Gravity")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -15f;
    private float velocityY;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private LayerMask groundMask;

    private bool isGrounded;
    private CharacterController controller;
    private TPS inputActions;
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool isRunning;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new TPS();
    }

    private void OnEnable() 
    { 
        if (inputActions != null)
        {
            inputActions.Enable(); 
            // Menggunakan fungsi wrapper formal agar aman saat OnDisable
            inputActions.Player.Jump.performed += OnJumpInput; 
        }
    }
    
    private void OnDisable() 
    {
        // Pengaman kodingan (Defense Coding) agar bebas dari NullReferenceException
        if (inputActions != null)
        {
            inputActions.Player.Jump.performed -= OnJumpInput; 
            inputActions.Disable(); 
        }
    }

    // Fungsi formal pembaca tombol lompat
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        jumpPressed = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        HandleMovement();
        HandleJump();
        ApplyGravity();
        
        // Update status isGrounded dan yVelocity ke Animator secara berkala
        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("yVelocity", velocityY); // Mengirim data kecepatan vertikal untuk transisi floating/falling
        }
    }

    private void HandleMovement()
    {
        if (inputActions == null || animator == null) return;

        // 1. Baca nilai input terlebih dahulu
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        isRunning = inputActions.Player.Sprint.IsPressed();
        
        // 2. Jika sedang memutar animasi ambil kunci, matikan semua input pergerakan & animasi berjalan
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PickupKey")) 
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
            return; 
        }

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        if (move.magnitude > 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0; camRight.y = 0;

            Vector3 moveDirection = camForward * move.z + camRight * move.x;
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed * Time.deltaTime);
            }

            float targetSpeed = isRunning ? runSpeed : walkSpeed;
            controller.Move(moveDirection.normalized * targetSpeed * Time.deltaTime);

            // Mengatur animasi berdasarkan lari atau jalan
            if (isRunning)
            {
                animator.SetBool("isRun", true);
                animator.SetBool("isWalk", false);
            }
            else
            {
                animator.SetBool("isWalk", true);
                animator.SetBool("isRun", false);
            }
        }
        else
        {
            // Jika berhenti bergerak
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
        }
    }

    private void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            velocityY = Mathf.Sqrt(jumpForce * -2f * gravity);
            if (animator != null)
            {
                animator.SetTrigger("jump");
            }
        }
        jumpPressed = false;
    }

    private void ApplyGravity()
    {
        if (isGrounded && velocityY < 0) velocityY = -2f;
        velocityY += gravity * Time.deltaTime;
        controller.Move(new Vector3(0, velocityY, 0) * Time.deltaTime);
    }
}