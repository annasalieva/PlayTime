using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float runSpeed = 20;
    [SerializeField] private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    private Vector3 moveDirection;
    public Vector3 velocity;
    private RaycastHit Hit;
    //public Ladder ladder;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private LayerMask groundMask;
    public float gravity = 9.81f;

    public float jumpHeight;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxJumpHeight;

    private CharacterController controller;
    //public Rigidbody rb;
    private float current_speed;

    public float fallMultiplier = 5.0f;
    
    public Animator anim;

    private float moveZ;
    private float moveX;
    private float starting_y;
    private float current_y;
    private bool jumpKeyHeld;
    //private float jumpMultiplier = 0; 

    public bool allowLemonMovement;

    [SerializeField] private Transform mainCamera;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        //mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].transform;
    }

    private void Update()
    {
        checkGrounded();
        
        if(allowLemonMovement)
        {
            Move();
            Rotate();
        }
        else
        {
            Idle();
            controller.Move(Vector3.zero);
        }

        current_y = transform.position.y;
    }

    private void Move()
    {
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
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
            BetterJump();
            anim.SetBool("OnGround", false); 
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpKeyHeld = false;
        }
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        Vector3 moveDir = Vector3.zero;
        

        controller.Move(velocity * Time.deltaTime);
        if(moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) *  Vector3.forward;
            controller.Move(moveDir * current_speed * Time.deltaTime);
        }
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

    public void Jump()
    {
        print("jump");
        jumpKeyHeld = true;
        starting_y = transform.position.y;
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity)*2;
    }

    private void BetterJump()
    {
        if(controller.velocity.y <= 0) //if velocity < 0 aka player is falling apply fall multiplier to gravity
        {
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
            //print("velocity.y <= 0");
        }
        else if (controller.velocity.y > 0) //else if jumping apply normal gravity
        {
            velocity.y += gravity * Time.deltaTime * 2;
            //print("velocity > 0");
        }
        
        if(jumpKeyHeld && (current_y - starting_y < maxJumpHeight)) //if jump key held and max height not reached increase vertical velocity
        {
            //print("hit max height");
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity)*2 + Mathf.Sqrt((current_y-starting_y)/5 * -2 * gravity)*5;
        }
        else //else if max reached apply the multiplied gravity so character starts falling
        {
            jumpKeyHeld = false;
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        //tbh idk why i did the conditions like this they look kinda wack especially that last if/else statement
        //basically we need to make sure he's falling if he reaches max height and/or you let go of the jump key
        //if we're doing forces apply constant gravity to char and then multiply it when he's falling
        //and add a force upward when he jumps that continues to work if you hold the key down unless you've reached max height
    }

    private void Rotate()
    {
        float inputZ = Input.GetAxis("Vertical");
        float inputX = Input.GetAxis("Horizontal");
        Vector3 inputDirection = new Vector3(inputX, 0, inputZ).normalized;


        if(inputDirection.magnitude >=  0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            // anim.SetFloat("Turn", Input.GetAxis("Horizontal") , turnSmoothVelocity * 0.1f, Time.deltaTime);
        }
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

    public void setCamera(CinemachineVirtualCamera Cam){
        mainCamera = Cam.transform;
    }
}
