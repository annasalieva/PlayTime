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

    private Animator anim;

    private float moveZ;
    private float moveX;
    private float starting_y;
    private float current_y;
    private bool jumpKeyHeld;
    //private Animator anim;

    public bool allowOrangeMovement;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        checkGrounded();
        
        if(allowOrangeMovement)
        {
            Move();
            Rotate();
        }
        current_y = transform.position.y;
    }

    private void Move()
    {
        if (isGrounded)
        {
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
            else if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && Input.GetKey(KeyCode.LeftShift) && !isGrabbing)
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
            // print("NOT grounded");
            BetterJump();
            anim.SetBool("OnGround", false); 
        }
        
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if(isGrabbing) //movement is slowed when moving objects
        {
            controller.Move(velocity * Time.deltaTime);
            controller.Move(moveDirection * current_speed * grabSpeedReduction *Time.deltaTime);
        } 
        else 
        {
            controller.Move(velocity * Time.deltaTime);
            controller.Move(moveDirection * current_speed * Time.deltaTime);
        }
    }

    private void Idle()
    {
        anim.SetFloat("Forward", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;

        if(!isGrabbing)
        {
            anim.SetFloat("Forward", 0.5f, 0.1f, Time.deltaTime);
        }
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        if(!isGrabbing)
        {
        anim.SetFloat("Forward", 1, 0.1f, Time.deltaTime);
        }
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

        if(!isGrabbing)
        {
            if(inputDirection != Vector3.zero)
            {
                Quaternion rotateDirection = Quaternion.LookRotation(inputDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateDirection, rotationSpeed * Time.deltaTime);
                anim.SetFloat("Turn", Input.GetAxis("Horizontal") , rotationSpeed * 0.1f, Time.deltaTime);
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

        private void checkGrounded()
    {
        //make sure to put any floors on the "ground" layer
        Debug.DrawRay(transform.position + new Vector3(0, 2, 0), -Vector3.up * groundCheckDistance, Color.red);
        if (Physics.BoxCast(transform.position + new Vector3(0, 2, 0), transform.lossyScale / 2, -Vector3.up, out Hit, transform.rotation, groundCheckDistance))
        {

            if (Hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = true;
                // print("grounded");
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