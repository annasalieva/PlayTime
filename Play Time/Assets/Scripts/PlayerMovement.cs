using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float accelerationSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f; 

    [SerializeField] private float airSpeed = 10;
    [SerializeField] private float jumpHeight = 60f;
    [SerializeField] private float gravityScale = 5f; 

    private Vector3 moveDirection;
    private Vector3 velocity;
    private RaycastHit Hit;

    [Header("Ground Elements")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance = 1.25f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity = 9.81f;


    [Header("Character Components")]
    private CharacterController controller;
    private BoxCollider playerCollider; 
    private Rigidbody playerRB;

    [Header("Character Components")]
    //private CharacterController controller;
    private Rigidbody rbPlayer; 


    private void Start()
    {
        //controller = GetComponent<CharacterController>();
        rbPlayer = GetComponent<Rigidbody>(); 
        //anim = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate() //changed to FixedUpdate cause physics
    {
        Move();
    }

    
    private void Move()
    {
        //make sure to put any floors on the "ground" layer
        //isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance,groundMask);
        //Debug.DrawRay(transform.position, new Vector3(0, -groundCheckDistance, 0), Color.green);
        Ray groundcast = new Ray(transform.position, -Vector3.up);
        if (Physics.Raycast(groundcast, out Hit, groundCheckDistance))
        {
            if (Hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }

        /*
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }*/

        //jump things -> to make it feel better
        /*
        Wanted to implement something that made the item fall faster when it reached the apex of it's jump... buggy atm
        if (rbPlayer.velocity.y < 0)
        {
            rbPlayer.AddForce(Physics.gravity * (gravityScale - 1) * rbPlayer.mass); //increased the gravity scale so the object falls faster
        }*/
        
        
        
        float moveZ = 0f; 
        float moveX = 0f; 

        if (isGrounded)
        {
            print("grounded");
            moveZ = Input.GetAxisRaw("Vertical") * accelerationSpeed;
            moveX = Input.GetAxisRaw("Horizontal") * accelerationSpeed;
            //anim.SetBool("grounded", true);
            /*
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }*/

            //moveDirection *= moveSpeed;

            if(Input.GetButton("Jump")) //so it works on controller as well
            {
                Jump();
            }
        }
        else
        {
            print("NOT grounded");
            //if lemon is in the air, use this version of movement so he goes faster
            //and has more air control
            moveZ = Input.GetAxisRaw("Vertical") * airSpeed;
            moveX = Input.GetAxisRaw("Horizontal") * airSpeed;
            //anim.SetBool("grounded", false);
        }

        moveDirection = new Vector3(moveX, 0, moveZ);
        rbPlayer.AddForce(moveDirection, ForceMode.Impulse); //need to implement some kind of drag since it's kind of like ice

        if ( rbPlayer.velocity.magnitude > maxSpeed )
        {
            //sets the max speed for the player in x direction
            rbPlayer.velocity = rbPlayer.velocity.normalized * maxSpeed; 

        } 
        //moveDirection = transform.TransformDirection(moveDirection);

        //controller.Move(moveDirection*Time.deltaTime);

        //velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);
    }

    private void Idle()
    {
        //anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
/*
    private void Walk()
    {
        moveSpeed = walkSpeed;
        //anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

/   private void Run()
    {
        moveSpeed = runSpeed;
        //anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }*/

    private void Jump()
    {
        Debug.Log("Jump!"); 
        //velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        rbPlayer.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse); 

    }
}
