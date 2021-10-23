﻿using System.Collections;
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
    [SerializeField] private float maxJumpHeight;

    private CharacterController controller;
    //public Rigidbody rb;
    private float current_speed;

    public float fallMultiplier = 5.0f;
    
    //private Animator anim;

    private float moveZ;
    private float moveX;
    private float starting_y;
    private float current_y;
    private bool jumpKeyHeld;
    //private float jumpMultiplier = 0; 


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        //anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Move();
        //print(controller.velocity);
        current_y = transform.position.y;
    }

    private void Move()
    {
        //make sure to put any floors on the "ground" layer
        //isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance,groundMask);
        Debug.DrawRay(transform.position, new Vector3(0, -groundCheckDistance, 0), Color.green);
        Ray groundcast = new Ray(transform.position, -Vector3.up);
        if (Physics.Raycast(groundcast, out Hit, groundCheckDistance))
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
                else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpKeyHeld = true;
                starting_y = transform.position.y;
                Jump();
                print("getting start pos");
            }
        }
        else
        {
            //print("NOT grounded");
            //anim.SetBool("grounded", false);

            BetterJump();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpKeyHeld = false;
            print("button up");
        }

        

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
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity)*2;
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
}
