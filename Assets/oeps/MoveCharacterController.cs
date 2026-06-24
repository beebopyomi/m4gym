using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCharacterController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private string mapName = "Player";
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -20f;
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputActionMap map;
    private CharacterController characterController;
    private Animator animator;
    private float verticalVelocity;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        InputActionMap map  = inputAsset.FindActionMap(mapName);
        moveAction          = map.FindAction("Move");
        sprintAction        = map.FindAction("Sprint");
        jumpAction          = map.FindAction("Jump");
    }

    void OnEnable()  
    { 
        map.Enable(); 
    }
    void OnDisable() 
    { 
        map.Disable();
    }
    void Update()
    {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();
        float speed = movementInput.y * moveSpeed;
        if (sprintAction.IsPressed())
            speed *= 2;

        Vector3 move = transform.forward * speed * Time.deltaTime;
        transform.Rotate(Vector3.up * movementInput.x * rotationSpeed * Time.deltaTime);
        if (characterController.isGrounded)
        {
            verticalVelocity = -1f;

        if (jumpAction.WasPressedThisFrame())
        {
            verticalVelocity = Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight);
            animator.SetTrigger("JumpTrigger");
        }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        move.y = verticalVelocity * Time.deltaTime;
        characterController.Move(move);
        animator.SetFloat("Speed", movementInput.y);
        animator.SetBool("Grounded", characterController.isGrounded);
    }
}