using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float runSpeed = 20;
    
    private Vector3 moveDirection;
    private Vector3 velocity;
    private RaycastHit Hit;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance = 1.25f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity = 9.81f;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float acceleration;

    private CharacterController controller;
    public Rigidbody rb;
    private float current_speed;

    public float fallMultiplier = 5.0f;
    //private Animator anim;

    private float moveZ;
    private float moveX;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        //anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        //make sure to put any floors on the "ground" layer
        //isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance,groundMask);
        Debug.DrawRay(transform.position, new Vector3(0, -groundCheckDistance, 0), Color.green);
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

        if (isGrounded)
        {
            print("grounded");
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
                else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
                {
                    Run();
                }
                else if (moveDirection == Vector3.zero)
                {
                    Idle();
                }

                moveDirection *= current_speed;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
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
            print("NOT grounded");
            //anim.SetBool("grounded", false);
        }

        BetterJump();

        moveDirection = new Vector3(moveX, velocity.y, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        controller.Move(moveDirection*Time.deltaTime);

        //velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);
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

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        //velocity.y = ;
    }
    private void BetterJump()
    {
        if(rb.velocity.y < 0)
        {
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;
             
        }
        else if (rb.velocity.y > 0)
        {
            velocity.y += gravity * Time.deltaTime;
            if(!Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y += 1;
            }
        }
    }
}
