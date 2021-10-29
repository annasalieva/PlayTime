using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float runSpeed = 20;
    [SerializeField] private float rotationSpeed = 5;

    private Vector3 moveDirection;
    private Vector3 velocity;
    private RaycastHit Hit;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity = 9.81f;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxJumpHeight;

    private CharacterController controller;
    //public Rigidbody rb;
    private float current_speed;

    public float fallMultiplier = 5.0f;
    
    private Animator anim;

    private float moveZ;
    private float moveX;
    private float starting_y;
    private float current_y;
    private bool jumpKeyHeld;
    //private float jumpMultiplier = 0; 


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        checkGrounded();
        BetterJump();
        Move();
        Rotate();
        current_y = transform.position.y;
    }

    private void Move()
    {
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpKeyHeld = true;
                starting_y = transform.position.y;
                Jump();
            }

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) //getting vertical or horizontal input
            {
                if (current_speed < moveSpeed)
                {
                    current_speed += acceleration;
                }
                moveZ = Input.GetAxisRaw("Vertical") * current_speed;
                moveX = Input.GetAxisRaw("Horizontal") * current_speed;
                moveDirection *= current_speed;
            }
            else if(current_speed > 0 && (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0))//no input
            {
                current_speed -= acceleration;
                moveZ = current_speed;
                moveX = current_speed;
            }


            if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                Idle();
            }

        }
        else
        {
            print("NOT grounded");
            anim.SetBool("OnGround", false); 
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpKeyHeld = false;
        }
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        moveDirection.y = velocity.y / current_speed;

        controller.Move(moveDirection * current_speed * Time.deltaTime);
    }

    private void Idle()
    {
        anim.SetFloat("Forward", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Forward", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Forward", 1, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {
        print("jump");
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity)*2;
    }

    private void BetterJump()
    {
        if(controller.velocity.y <= 0)
        {
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
            //print("velocity.y <= 0");
        }
        else if (controller.velocity.y > 0)
        {
            velocity.y += gravity * Time.deltaTime * 2;
            //print("velocity > 0");
        }
        
        if(jumpKeyHeld && (current_y - starting_y < maxJumpHeight))
        {
            //print("hit max height");
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity)*2 + Mathf.Sqrt((current_y-starting_y)/5 * -2 * gravity)*5;
        }
        else
        {
            jumpKeyHeld = false;
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Rotate()
    {
        float inputZ = Input.GetAxis("Vertical");
        float inputX = Input.GetAxis("Horizontal");
        Vector3 inputDirection = new Vector3(inputX, 0, inputZ);
        inputDirection.Normalize();
        if (inputDirection != Vector3.zero)
        {
            Quaternion rotateDirection = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateDirection, rotationSpeed * Time.deltaTime);
        }
        anim.SetFloat("Turn", Input.GetAxis("Horizontal") , rotationSpeed * 0.1f, Time.deltaTime);
    }

    private void checkGrounded()
    {
        //make sure to put any floors on the "ground" layer
        Debug.DrawRay(transform.position + new Vector3(0, 2, 0), -Vector3.up * groundCheckDistance, Color.red);
        if (Physics.BoxCast(transform.position + new Vector3(0, 2, 0), transform.lossyScale / 2, -Vector3.up, out Hit, transform.rotation, groundCheckDistance))
        {

            if (Hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = true;
                print("grounded");
                anim.SetBool("OnGround", true);
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            print("racyast has not hit anything");
            isGrounded = false;
        }
    }
}
