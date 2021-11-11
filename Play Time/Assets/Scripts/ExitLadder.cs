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
        doneClimb = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && ladder.isClimbingDown == false && ladder.OnLadder == true)
        {
            Debug.Log("Touch Top!");
            ladder.OnLadder = false;
            Debug.Log("Reach End State!");
            doneClimb = true;
            ladder.player.transform.position = ladder.endPosition.transform.position;
            ladder.player.GetComponent<PlayerMovement>().allowLemonMovement = true;
            Physics.gravity = new Vector3(0, -9.81f, 0);
            ladder.player.velocity = ladder.current_velocity;
            Debug.Log("Set End State");

        }
        
        else if (other.tag == "Player" && ladder.isClimbingDown == true && ladder.OnLadder == true)
        {
            Debug.Log("Touch Bottom");
            doneClimb = true;
            ladder.OnLadder = false;
            ladder.isClimbingDown = false;
            ladder.player.transform.position = ladder.lockpoint.transform.position;
            ladder.player.GetComponent<PlayerMovement>().allowLemonMovement = true;
            Physics.gravity = new Vector3(0, -9.81f, 0);
            ladder.player.velocity = ladder.current_velocity;

        }
    }
}