using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewClimbing : MonoBehaviour
{
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
