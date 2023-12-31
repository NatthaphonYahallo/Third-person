using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorller : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private float playerspeed = 5f;
    [SerializeField] private Camera followCamera;
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 playerVelocuty;
    [SerializeField] private float gravityValue = -13f;

    public bool groundedPlayer;
    [SerializeField] private float jumpHeight = 2.5f;

    public Animator animator;

    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    public static PlayerContorller instance;
    private void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    private void Update()
    {
        switch(CheckWinner.instance.isWinner)
        {
            case true:
                animator.SetBool("Victory", CheckWinner.instance.isWinner);
                break;
                case false:
                Movement();
                break;
        }
           
    }
    void Movement()
    {
        groundedPlayer = characterController.isGrounded;
        
        if (characterController.isGrounded && playerVelocuty.y < -2)
        {
            playerVelocuty.y = -1f;
        }
        float horizentalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(horizentalInput, 0, verticalInput);

        Vector3 movementDirection = movementInput.normalized;
        characterController.Move(movementDirection*playerspeed*Time.deltaTime);

        if(movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation,rotationSpeed* Time.deltaTime);
        }
        
        if(Input.GetButtonDown("Jump") &&groundedPlayer )
        {
            playerVelocuty.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetTrigger("Jumping");
        }

        playerVelocuty.y += gravityValue * Time.deltaTime; 
        characterController.Move(playerVelocuty * Time.deltaTime);
        animator.SetFloat("Speed", Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("Ground", characterController.isGrounded);
    }
}

