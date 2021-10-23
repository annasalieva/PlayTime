using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orangeMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float runSpeed = 20;
    [SerializeField] private float airSpeed = 10;
    [SerializeField] private float rotationSpeed = 720;

    private Vector3 moveDirection;
    private Vector3 velocity;
    private RaycastHit Hit;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance = 1.25f;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private bool canGrab;
    [SerializeField] private bool isGrabbing;
    [SerializeField] private float grabCheckDistance = 1.5f;
    [SerializeField] private float grabSpeedReduction = .25f;
    [SerializeField] private Transform grabbableObject;

    [SerializeField] private float gravity = 9.81f;


    [SerializeField] private GameObject grabJoint;

    private CharacterController controller;
    //private Animator anim;

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

        //Raycast for checking if objects can be grabbed
        Vector3 forward = transform.TransformDirection(Vector3.forward) * grabCheckDistance;
        Debug.DrawRay(transform.position, forward, Color.green);
        Ray interactCast = new Ray(transform.position, forward);
        if (Physics.Raycast(interactCast, out Hit, grabCheckDistance))
        {
            if (Hit.transform.gameObject.CompareTag("grabbable"))
            {
                canGrab = true;
                grabbableObject = Hit.transform;
            }
            else
            {
                canGrab = false;
                grabbableObject = null;
            }
        }
        else
        {
            canGrab = false;
            grabbableObject = null;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        if (isGrounded)
        {
            print("grounded");
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

            if((Input.GetKey(KeyCode.Space) && canGrab) || (Input.GetKey(KeyCode.Space) && isGrabbing))
            {
                if(grabbableObject != null)
                {
                    if(grabJoint.GetComponent<FixedJoint>().connectedBody != grabbableObject.gameObject.GetComponent<Rigidbody>())
                    {
                        grabJoint.GetComponent<FixedJoint>().connectedBody = grabbableObject.gameObject.GetComponent<Rigidbody>();
                    }
                }
                
                isGrabbing = true;
            } else {
                if(grabbableObject != null)
                {
                    grabJoint.GetComponent<FixedJoint>().connectedBody = null;
                    isGrabbing = false;
                }
            }
        }
        else
        {

        }
        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection.Normalize();


        //Movement stuff aka controller.move
        if(isGrounded && !isGrabbing)
        {
            controller.Move(moveDirection* moveSpeed *Time.deltaTime);
        }
        else if(isGrounded && isGrabbing) //Movement is slowed when moving objects
        {
                controller.Move(moveDirection* (moveSpeed * grabSpeedReduction) *Time.deltaTime);
        }
        else
        {
            controller.Move(moveDirection* airSpeed *Time.deltaTime);
        }

        //Controls turning/rotation
        if(!isGrabbing)
        {
            if(moveDirection != Vector3.zero)
            {
                Quaternion rotateDirection = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateDirection, rotationSpeed * Time.deltaTime);
            }
        }

        //Falling aka vertical movement
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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


    //treat the controller like a rigidbody when running into objects and grabbed onto them.
        private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(isGrabbing == true)
        {
            Rigidbody body = hit.collider.attachedRigidbody;

            // no rigidbody
            if (body == null || body.isKinematic)
            {
                return;
            }

            // We dont want to push objects below us
            if (hit.moveDirection.y < -0.3)
            {
                return;
            }

            // Calculate push direction from move direction,
            // we only push objects to the sides never up and down
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            Vector3.Normalize(pushDir);

            // If you know how fast your character is trying to move,
            // then you can also multiply the push velocity by that.

            // Apply the push
            body.velocity = pushDir*(moveSpeed*(grabSpeedReduction*2)); //Testing temporary variable
        }
    }
}