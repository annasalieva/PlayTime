using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLadder : MonoBehaviour
{
    public Ladder ladder;
    public bool doneClimb;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (doneClimb)
        {
            ladder.player.GetComponent<PlayerMovement>().enabled = true;
            Debug.Log("Enable Movement");
            doneClimb = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && ladder.isClimbingDown == false)
        {
            Debug.Log("Touch Top!");
            ladder.OnLadder = false;
            Debug.Log("Reach End State!");
            doneClimb = true;
            ladder.player.transform.position = ladder.endPosition.transform.position;
            Debug.Log("Set End State");

        }
        if (other.tag == "Player" && ladder.isClimbingDown == true)
        {
            ladder.PlayerPresent = false;
            ladder.OnLadder = false;
            ladder.isClimbingDown = false;
            ladder.player.GetComponent<PlayerMovement>().enabled = true;

        }
    }
}