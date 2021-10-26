using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orangeMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float runSpeed = 20;
    [SerializeField] private float rotationSpeed = 720;

    private Vector3 moveDirection;
    private Vector3 inputDirection;
    private Vector3 velocity;
    private RaycastHit Hit;

    [SerializeField] private bool isGrounded; //used for grab script
    [SerializeField] private float groundCheckDistance = 1.25f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity = 9.81f;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxJumpHeight;

    [SerializeField] private bool isGrabbing; //Used for grab script
    [SerializeField] private float grabSpeedReduction;
    private CharacterController controller;

    private float current_speed;
    public float fallMultiplier = 5.0f;

    private float moveZ;
    private float moveX;
    private float starting_y;
    private float current_y;
    private bool jumpKeyHeld;
    //private Animator anim;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        //anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Move();
        current_y = transform.position.y;
    }

    private void Move()
    {
        //make sure to put any floors on the "ground" layer
        if (Physics.BoxCast(transform.position, transform.lossyScale/2, -Vector3.up, out Hit, transform.rotation, groundCheckDistance))
        {
            print(Hit.transform.gameObject.name);
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
            print("racyast has not hit anything");
            isGrounded = false;
        }

        if (isGrounded)
        {
            //print("grounded");
            if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) //getting vertical or horizontal input
            {
                if (current_speed < moveSpeed)
                {
                    current_speed += acceleration;
                }
                moveZ = Input.GetAxis("Vertical") * current_speed;
                moveX = Input.GetAxis("Horizontal") * current_speed;
                //anim.SetBool("grounded", true);
                if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
                {
                    Walk();
                }
                else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift) && !isGrabbing)
                {
                    Run();
                }
                else if (moveDirection == Vector3.zero)
                {
                    Idle();
                }

                moveDirection *= current_speed;

                
            }
            else if(current_speed > 0 && (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0))//no input
            {
                current_speed -= acceleration;
                moveZ = current_speed;
                moveX = current_speed;
            }
        }
        else
        {
            //print("NOT grounded");
            //anim.SetBool("grounded", false);

            BetterJump();
        }
        
        moveDirection = new Vector3(moveX, velocity.y, moveZ);

        if(isGrabbing) //movement is slowed when moving objects
        {
            controller.Move(moveDirection*grabSpeedReduction*Time.deltaTime);
        } 
        else 
        {
            controller.Move(moveDirection*Time.deltaTime);
        }

        Rotate();
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

    private void Run()
    {
        moveSpeed = runSpeed;
        //anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }

    private void BetterJump()
    {
        if(controller.velocity.y <= 0)
        {
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
            print("hey its me");
        }
        else if (controller.velocity.y > 0)
        {
            velocity.y += gravity * Time.deltaTime * 2;
            print("velocity is more");
        }
        
        if(jumpKeyHeld && (current_y - starting_y < maxJumpHeight))
        {
            print("hit max height");
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

        if(!isGrabbing)
        {
            if(inputDirection != Vector3.zero)
            {
                Quaternion rotateDirection = Quaternion.LookRotation(inputDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateDirection, rotationSpeed * Time.deltaTime);
            }
        }
    }

    public float fetchMoveSpeed()
    {
        return moveSpeed;
    }

    public float fetchGrabSpeed()
    {
        return grabSpeedReduction;
    }

    public bool fetchGrab()
    {
        return isGrabbing;
    }

    public void setGrab(bool grabState)
    {
        isGrabbing = grabState;
    }

    public bool fetchGround()
    {
        return isGrounded;
    }
}