using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float walkSpeed = 5;
    //[SerializeField] private float runSpeed = 20;
    [SerializeField] private float airSpeed = 10;

    private Vector3 moveDirection;
    private Vector3 velocity;
    private RaycastHit Hit;

    [Header("Ground Elements")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance = 1.25f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity = 9.81f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight;

    [Header("Character Components")]
    private CharacterController controller;
    private BoxCollider playerCollider; 
    private Rigidbody playerRB;

    //private Animator anim;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCollider = GetComponent<BoxCollider>(); 
        playerRB = GetComponent<Rigidbody>(); 
        
        //anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Move();
    }


    //IsGrounded function
    private bool IsGrounded()
    {
        Debug.Log("Checking if Grounded"); 
        //constructing box cast which needs (center point, size, rotation, direction, max distance to project cast, layerMask)
            //LAYER MASK -> what layers the raycast will hit
            //down because we are detecting the floor and is just slightly offset from player position
        bool hit = Physics.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, Vector3.down, Quaternion.identity, groundCheckDistance, groundMask);
        
        //The box cast will return False if the character is on the ground, so it returns the opposite of that so it isGrounded is true when character on ground
        return !hit; 
    }
    
    private void Move()
    {
        //make sure to put any floors on the "ground" layer
        
        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float moveZ = Input.GetAxis("Vertical") * moveSpeed;
        float moveX = Input.GetAxis("Horizontal") * moveSpeed;
        
        if (IsGrounded())
        {
            print("grounded");
            //anim.SetBool("grounded", true);
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }

            moveDirection *= moveSpeed;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            print("NOT grounded");
            //if lemon is in the air, use this version of movement so he goes faster
            //and has more air control
            moveZ = Input.GetAxis("Vertical") * airSpeed;
            moveX = Input.GetAxis("Horizontal") * airSpeed;
            //anim.SetBool("grounded", false);
        }
        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        controller.Move(moveDirection*Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle()
    {
        //anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    
    private void Walk()
    {
        moveSpeed = walkSpeed;
        //anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

/*    private void Run()
    {
        moveSpeed = runSpeed;
        //anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }*/

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
}
