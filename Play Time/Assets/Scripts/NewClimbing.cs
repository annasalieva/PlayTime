using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewClimbing : MonoBehaviour
{
    
    [Header("Colliders")]
    [SerializeField]
    private Collider LADDER_COLLIDER; //main collider for the ladder
    
    [SerializeField]
    private Collider EXIT_COLLIDER;

    [Header("Movement")]
    [SerializeField]
    private float ClimbingSpeed = 1f; //how fast the character moves while climbing
    
    [Header("Climbing Bounds")]
    private float X_MIN; 
    private float X_MAX; 
    private float Y_MIN; 
    private float Y_MAX; 
    [SerializeField]
    private Transform startpoint;  //transform in the middle of the ladder 

    [Header("Bools")]
    private bool isClimbing = false; 
    private bool inRange = false;

    [Header("Character Elements")]
    
    [SerializeField]
    private CharacterController LemonCharacterController;
    
    [SerializeField]
    private PlayerMovement LemonMovement; 

    [SerializeField]
    private GameObject Lemon; 


    void Start()
    {
        //gets x min/max of collider
        X_MIN = LADDER_COLLIDER.bounds.min.x; 
        X_MAX = LADDER_COLLIDER.bounds.max.x; 

        //gets y min/max of collider
        Y_MIN = LADDER_COLLIDER.bounds.min.y; 
        Y_MAX = LADDER_COLLIDER.bounds.max.y; 

    }

    void Update()
    {
        if (inRange) //player is in range of colliders and able to climb 
        {
            if (Input.GetKeyDown(KeyCode.E) && !isClimbing) //player presses e and not currently climbing
            {
                StartClimb(); 
            }   
            else if (Input.GetKeyDown(KeyCode.E) && isClimbing) //stop climbing once the player hits E again
            {
                //player is no longer climbing
                //makes the player drop to the floor again
                Fall(); 
            }
            else if (Input.GetKeyDown(KeyCode.Space) && isClimbing)
            {
                //code for when the player wishes to jump off the ladder
                LadderJump(); 
            }
            else if(isClimbing) //player is currently in climbing state and hasn't input any other commands
            {
                Climb(); 
            }

        } 
    }
    
    void StartClimb()
    {
        //climbing scripts go here
        //restrict lemon on the x min/max and y min/max
        isClimbing = true; 

        Physics.gravity = new Vector3(0, 0, 0); //turns off physics for character
        LemonMovement.allowLemonMovement = false; 
        LemonCharacterController.SimpleMove(Vector3.zero); //turning velocity to 0

        //animator components
        LemonMovement.anim.SetFloat("Forward", 0, 0.1f, Time.deltaTime); 
        LemonMovement.anim.SetBool("OnGround", true); 

        //locks the player's rotation and distance from the ladder
        Lemon.transform.position = startpoint.position; 
        Lemon.transform.rotation = startpoint.rotation; 
        
    }

    void Climb()
    {
        //handles player input for climbing
        //WATCH OUT -> Needs to be changed since horizontal axis will change based on camera rotation so 
        //MUST BE CHANGED (Ask Teal)
        Vector3 movement = new Vector3 ( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0); 
        Lemon.transform.position += movement * Time.deltaTime * ClimbingSpeed;
    }

    void LadderJump()
    {
        
    }

    void Fall() //undoes everything that happens in the climb function
    {
        isClimbing = false;
        Physics.gravity = new Vector3 (0, LemonMovement.gravity, 0);  
        LemonMovement.allowLemonMovement = true; 
        LemonMovement.anim.SetBool("OnGround", false); 
    }

    void OnTriggerEnter(Collider other) //is called as long as the player is in the collider
    {
        
        if (other.tag == "Player")
        {
            //if the player has entered the entrance collider -> we want to do climbing stuff once player hits e
            inRange = true; 
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //player has made it to the end and is no longer climbing
            inRange = false; 

            //call climbing exit animation here
        }
    }
    
    
    /*
    LOGIC:

    1. check if player is within range of the ladder
        //Use OnTriggerStay to constantly check if player in ladder range
        //Can possible use this to call another function that checks if player hits e since it will only ever call the function if the trigger is being activated
    
    2. check if player wants to climb (presses e)
        //start climbing when player hits e
        //possibly lock player? -? might want to change this to restricting player to the bounds of the the climbing trigger so that they can also move left to right for future puzzles 
            //get the x and y coordinates for the trigger so that if the player wants to go past it, their movement in that direction is locked (unless of course it's a place marked as the exit)
        //allow for player to stop climbing when they hit e or jump
            //pressing e should unlock the player and cause them to fall to the floor
            //pressing jump should unlock the player and cause them to jump in the direction the player is inputting
    
    3. check if the player reaches somewhere to exit -> if so disconnect them from climbing 
        //input a place for climbing animation for enter, climbing, and exit

    */
}
