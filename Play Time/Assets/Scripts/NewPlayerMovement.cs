using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    private float moveSpeed;//variable for current movement speed
    [SerializeField] private float walkSpeed = 5f; //variable for max speed when walking
    [SerializeField] private float runSpeed = 20f; //max speed when running
    [SerializeField] private float turnSmoothTime = 0.1f; //used for rotating lemon?
    float turnSmoothVelocity; //also used for rotating lemon, not sure how it works

    public Animator anim; //lemon's animator controller

    [SerializeField] private bool isGrounded; //if true, lemon is on the ground
    [SerializeField] private float groundCheckDistance = 1f; //size of the box checking the ground
    [SerializeField] private LayerMask groundMask; //layermask representing the ground layer
    [SerializeField] private float maxJumpHeight; //the maximum Y distance between lemon when he starts his jump vs when he reaches the apex of the jump
    public float gravity = 9.81f; //downward acceleration on lemon
    private float current_y; //lemon's current y position
    public bool allowMovement; //if false, lemon should be unable to move
    // Start is called before the first frame update
    void Start()
    {
        //get the character controller
        controller = GetComponent<CharacterController>();
        //get the animator
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if lemon is on the ground every frame
        checkGrounded();

        if(allowMovement && isGrounded) //if lemon is allowed to move and is on the ground
        {
            move(); //move lemon
            Rotate(); //rotate lemon so he is facing the direction he moved in the previous line
        }
        else if(allowMovement) //if lemon is allowed to move but he's not grounded, perform airmovement functions
        {
            Rotate(); //he should still rotate when he's in the air
        }
        else
        {
            Idle(); //set lemon to his idle animation
            controller.Move(Vector3.zero); //zero lemon's movement so he cant move without input
        }

        current_y = transform.position.y;
    }

    private void Move()
    {
        if(Input.Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    //set lemon's animation to his idle stance
    private void Idle()
    {
        anim.SetFloat("Forward", 0, 0.1f, Time.deltaTime);
    }

    //move lemon at his full walkspeed and have the animator play his walk animation
    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Forward", 0.5f, 0.1f, Time.deltaTime);
    }

    //move lemon at his full runspeed and have the animator play his running animation
    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Forward", 1, 0.1f, Time.deltaTime);
    }

    //make lemon jump
    public void Jump()
    {
        print("jump");
        //gets lemon's y at the start of the jump and compares it to his current height
        //if the difference between the currentheight and the start height is >= to the max height, then lemon needs to start falling
        starting_y = transform.position.y; 
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity) * 2;
    }

    private void checkGrounded()
    {
        //make sure to put any floors on the "ground" layer
        //creates a box cast below lemon, not entirely sure how this works so please do not mess with it unless absolutely necessary
        if (Physics.BoxCast(transform.position + new Vector3(0, 2, 0), transform.lossyScale / 2, -Vector3.up, out Hit, transform.rotation, groundCheckDistance))
        {
            //if the boxcast hits a ground object, lemon should be considered on the ground
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
            // anim.SetFloat("Turn", Input.GetAxis("Horizontal") , turnSmoothVelocity * 0.1f, Time.deltaTime);
        }
    }
}
