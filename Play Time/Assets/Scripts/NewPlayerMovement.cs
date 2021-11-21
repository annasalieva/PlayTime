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
    //controls lemon's rotation
    [SerializeField] private float rotationSpeed;

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
            float moveZ = Input.GetAxisRaw("Vertical");

            //pressing A sets this to 1, pressing D sets this to -1 (I think)
            float moveX = -1 * Input.GetAxisRaw("Horizontal");

            //move x and z are relative to lemon, 
            //so moveZ moves lemon forward and back, and moveX moves Lemon 
            //left and right relative to HIM, not the world
            moveDirection = new Vector3(moveX, 0, moveZ);

            //uses lemon's forward as the one in the movement vector
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.Normalize();

            if (isGrounded) //if lemon is on the ground, he
            {
                //function just calculates how Lemon Should move
                Move();

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
            }
            else
            {
                //works similarly to move, but controls lemon's movements in the air
                Fall();
            }
            //the following lines apply the movement we calculated

            //applies speed to our direction vector
            moveDirection *= moveSpeed;

            //applies the calculated move Direction vector to the vector 3
            controller.Move(moveDirection * Time.deltaTime);

            //calculates the gravity acting on Lemon
            velocity.y += gravity * Time.deltaTime;

            //actually applies the gravity to lemon
            controller.Move(velocity * Time.deltaTime);

            
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
            //rotate lemon appropriately for the current direction he is facing
            Rotate();
        }
        //if player is moving and pressing shift
        else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            //run
            Run();
            //rotate lemon appropriately for the current direction he is facing
            Rotate();
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
        anim.SetFloat("Forward", 0, 0.1f, Time.deltaTime);
    }

    //set player's speed to the walkspeed and have the walk animation play
    private void Walk()
    {
        moveSpeed = walkSpeed;
        //Apparently the animator can affect lemon's movement speed
        anim.SetFloat("Forward", 0.5f, 0.1f, Time.deltaTime);
    }

    //set player's speed to the runspeed and have the run animation play
    private void Run()
    {
        moveSpeed = runSpeed;
        //Apparently the animator can affect lemon's movement speed
        anim.SetFloat("Forward", 1, 0.1f, Time.deltaTime);
    }

    public void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    private void Rotate()
    {
        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, step, 90.0f);
        this.transform.rotation = Quaternion.LookRotation(newDirection);
        
    }

    private void Fall()
    {
        anim.SetBool("OnGround", false);
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

    /*
    public void setCamera(CinemachineVirtualCamera Cam)
    {
        mainCamera = Cam.transform;
    }
    */
}
