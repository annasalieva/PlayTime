using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewClimbing : MonoBehaviour
{
    
    [Header("Colliders")]
    [SerializeField]
    private Collider LADDER_COLLIDER; //main collider for the ladder
    
    [SerializeField]
    private Collider ENTRANCE_COLLIDER;
    
    [SerializeField]
    private Collider EXIT_COLLIDER;

    [Header("Movement")]
    [SerializeField]
    private float ClimbingSpeed = 1f; //how fast the character moves while climbing
    
    [Header("Climbing Bounds")]
    
    [SerializeField]
    private GameObject Lemon; //Lemon's transform
    private float X_MIN; 
    private float X_MAX; 
    private float Y_MIN; 
    private float Y_MAX; 

    [Header("Bools")]
    private bool isClimbing = false; 

    void Start()
    {
        //gets x min/max of collider
        X_MIN = LADDER_COLLIDER.bounds.min.x; 
        X_MAX = LADDER_COLLIDER.bounds.max.x; 

        //gets y min/max of collider
        Y_MIN = LADDER_COLLIDER.bounds.min.y; 
        Y_MAX = LADDER_COLLIDER.bounds.max.y; 
    }
    
    void Climb()
    {
        //climbing scripts go here
        //restrict lemon on the x min/max and y min/max

        if (Input.GetKeyDown(KeyCode.E)) //stop climbing once the player hits E again
        {
            isClimbing = false; //player is no longer climbing
        }
        else 
        {
            //allow player to move around at specified climbing speed
            //restrict them if their x and y are lower/higher than min/max
            
        }
    
    
    
    }
    void OnTriggerStay(Collider box) //is called as long as the player is in the collider
    {
        if (box == ENTRANCE_COLLIDER)
        {
            //if the player has entered the entrance collider -> we want to do climbing stuff once player hits e
            if (Input.GetKeyDown(KeyCode.E))
            {
                //player hits E and now we want to start climbing
                Climb();
                //player is now in climbing state
                isClimbing = true; 
            }
            
        }
    }

    void OnTriggerEnter(Collider box)
    {
        if (box == EXIT_COLLIDER)
        {
            //player has made it to the end and is no longer climbing
            isClimbing = false; 

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
