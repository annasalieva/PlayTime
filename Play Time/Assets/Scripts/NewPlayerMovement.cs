using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NewPlayerMovement : MonoBehaviour
{
    //Container for the current movement speed
    private float moveSpeed = 0f;
    //the maximum speed of a walk
    [SerializeField] private float walkSpeed;
    //the maximum speed of a run
    [SerializeField] private float runSpeed;
    //controls the max height of Lemon's jump
    [SerializeField] private float jumpHeight;
    //Multiplies gravity when Lemon is falling so he falls faster
    [SerializeField] private float FallMultiplier;
    //Used to control how high lemon goes on a tap as opposed to a hold
    [SerializeField] private float LowMultiplier;
    //the acceleration and decelleration of lemon's walk
    [SerializeField] private float acceleration;
    //controls lemon's rotation
    private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    [Range(0, 1)]
    [SerializeField] private float lemonAirSpeed = 0.9f; 
    //needs the main camera to make the movement camera based
    [SerializeField] private Transform mainCamera;


    //the direction we are currently moving in
    //does not use the Y movement
    private Vector3 moveDirection;

    //velocity is SPECIFICALLY for any Y movement
    //all gravity and jumping is done with velocity
    private Vector3 velocity;

    //raycasthit for the ground check
    private RaycastHit Hit;

    //bool to enable/disable lemon's movement
    public bool allowLemonMovement;

    //lemon's character controller
    private CharacterController controller;

    //lemon's animation controller
    public Animator anim;

    //if true, player is on the ground
    private bool isGrounded;
    //Layer of items that can be considered ground
    [SerializeField] private LayerMask groundMask;
    //the value of our gravity, MUST be negative for physics calculations to work
    public float gravity = -9.81f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        //function checks if lemon is on the ground
        checkGrounded();

        if(allowLemonMovement)//if lemon is allowed to move
        {
            //if we're grounded, we want to stop applying gravity
            if (isGrounded && velocity.y < 0)
            {
                //set y velocity to -2, because 0 might not ground us fully
                velocity.y = 0f;
            }

            //pressing W sets this to 1, pressing S sets this to -1
            float moveZ = Input.GetAxis("Vertical");

            //pressing A sets this to 1, pressing D sets this to -1 (I think)
            float moveX = Input.GetAxis("Horizontal");

            float targetAngle;
            moveDirection = new Vector3(moveX,0.0f,moveZ);
            //if the player is providing any input, this will be true and we need to update
            //lemon's move direction
            if(moveX != 0 || moveZ != 0)
            {
                //not quite sure how this works, but its part of the rotation calculations that Teal made
                targetAngle = Mathf.Atan2(moveX, moveZ) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            //if no input, then the move direction will be zero
            else
            {
                moveDirection = Vector3.zero;
            }

            //normalize lemon's direction vector
            moveDirection.Normalize();

            if (isGrounded) //if lemon is on the ground
            {
                //function just calculates how Lemon Should move based on current inputs
                Move();

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
            } 
            //adjusts lemon's fall speed so his jump feels better
            FastFall();
            //the following lines apply the movement we calculated
            if(isGrounded)
            {
                //applies speed to our direction vector
                moveDirection *= moveSpeed;
            }
            else
            {
                //applies speed to our direction vector
                moveDirection *= lemonAirSpeed * moveSpeed;
            }
            

            //applies the calculated move Direction vector to the character controller
            controller.Move(moveDirection * Time.deltaTime);

            //calculates the gravity acting on Lemon
            velocity.y += gravity * Time.deltaTime;

            //actually applies the gravity to lemon
            controller.Move(velocity * Time.deltaTime);
            
            //rotate lemon appropriately for the current direction he is facing
            Rotate();
        }

    }

    //most of this function just calculates lemon's movement and applies this movement
    //as a vector 3 in the last line.
    //entire function assumes the player is grounded
    private void Move()
    {
        //if player is moving and not pressing shift
        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)) 
        {
            //walk
            Walk();
        }
        //if player is moving and pressing shift
        else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            //run
            Run();
        }
        //if the player is not moving
        else if(moveDirection == Vector3.zero)
        {
            //Idle
            Idle();
        }
    }

    //set the player's animation to the idle anim
    private void Idle()
    {
        if (moveSpeed > 0)
        {
            moveSpeed -= acceleration;
        }
        else
        {
            moveSpeed = 0;
        }
        anim.SetFloat("Forward", 0, 0.1f, Time.deltaTime);
    }

    //set player's speed to the walkspeed and have the walk animation play
    private void Walk()
    {
        if(moveSpeed < walkSpeed)
        {
            moveSpeed += acceleration;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
        //Apparently the animator can affect lemon's movement speed
        anim.SetFloat("Forward", 0.5f, 0.1f, Time.deltaTime);
    }

    //set player's speed to the runspeed and have the run animation play
    private void Run()
    {
        if (moveSpeed < runSpeed)
        {
            moveSpeed += acceleration;
        }
        else
        {
            moveSpeed = runSpeed;
        }
        //Apparently the animator can affect lemon's movement speed
        anim.SetFloat("Forward", 1, 0.1f, Time.deltaTime);
    }

    public void Jump()
    {
        //added a *4 because the fall function makes this equation too low
        velocity.y = Mathf.Sqrt(jumpHeight * 4 * -2 * gravity);
    }

    //rotates lemon based on his movement
    //Not entirely sure how it works, teal made it and told me to use it
    //at the beginning of Q1
    private void Rotate()
    {
        float inputZ = Input.GetAxis("Vertical");
        float inputX = Input.GetAxis("Horizontal");
        Vector3 inputDirection = new Vector3(inputX, 0, inputZ).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            anim.SetFloat("Turn", Input.GetAxis("Horizontal") , turnSmoothVelocity * 0.1f, Time.deltaTime);
        }
    }

    private void FastFall()
    {
        //if lemon is falling
        if(controller.velocity.y < 0)
        {
            //apply fall multiplier to gravity
            velocity.y += gravity * (FallMultiplier - 1) * Time.deltaTime;
        }
        else if(controller.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            velocity.y += gravity * (LowMultiplier - 1) * Time.deltaTime;
        }
    }

    private void checkGrounded()
    {
        //make sure to put any floors on the "ground" layer
        if (Physics.BoxCast(transform.position + new Vector3(0, 2, 0), transform.lossyScale / 2, -Vector3.up, out Hit, transform.rotation, 1.89f))
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

    
    public void setCamera(CinemachineVirtualCamera Cam)
    {
        mainCamera = Cam.transform;
    }
    
}
