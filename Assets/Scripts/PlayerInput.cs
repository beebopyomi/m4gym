using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class player : MonoBehaviour
{

    [SerializeField] private InputActionAsset input;
    [SerializeField] private string actionMapName = "Player1";

    private InputActionMap map;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private CharacterController cc;
    private float verticalVelocity = 0f;
    public float gravity = -9.81f;
    //private Rigidbody rb;
    public float turnSpeed = 3f;
    public float moveSpeed = 0.25f;
    public float jumpHeight = 6.5f;
    private Animator animator;

    void Awake()
    {
        map = input.FindActionMap(actionMapName);
        moveAction = map.FindAction("Move");
        jumpAction = map.FindAction("Jump");
        sprintAction = map.FindAction("Sprint");
    }
    void Onable()
    {
        map.Enable();
    }

    void Osable()
    {
        map.Disable();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        transform.rotation *= quaternion.Euler(0,moveInput.x*turnSpeed*Time.deltaTime,0);
        float currentMoveSpeed = moveInput.y * moveSpeed;
        if (sprintAction.IsPressed())
        {
            currentMoveSpeed *= 2;
        }
        if (cc.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = 0f;
        }

        if (jumpAction.WasPressedThisFrame())
        {
            verticalVelocity = jumpHeight;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 move = transform.forward * currentMoveSpeed + Vector3.up * verticalVelocity;
        cc.Move(move * Time.deltaTime);
        if (cc.isGrounded)
        {
            verticalVelocity = -1f; // kleine downward force om grounded te blijven

            if (jumpAction.WasPressedThisFrame())
            {
                verticalVelocity = Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight); //hoeveel kracht moet je geven om op de juiste hoogte uit te komen?
                animator.SetTrigger("JumpTrigger");
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // zwaartekracht trekt mij naar beneden
        }
        //animator.SetFloat("Speed", movementInput.y);
        //animator.SetBool("Grounded", characterController.isGrounded);

    }

}