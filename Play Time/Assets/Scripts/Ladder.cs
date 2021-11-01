using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool OnLadder;
    public bool PlayerPresent;
    public float ClimbingSpeed = 5f;
    public PlayerMovement player;
    public Transform endPosition;
    public bool isClimbingDown;
    public Vector3 current_velocity;
    public Transform lockpoint;

    // Start is called before the first frame update
    void Start()
    {
        OnLadder = false;
    }

    // Update is called once per frame
    void Update()
    {
        //when the character is near the ladder/any wall that is able to climb, press E to climb
        if (PlayerPresent && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed");
            OnLadder = true;
            PlayerPresent = false;
            Debug.Log("Set Start Point");
            current_velocity = player.velocity;
            player.velocity = new Vector3(0, 0, 0);

            //there was a bug where the player could have a bad trajectory and miss the top, so this lockpoint
            //snaps lemon to a starting position/rotation such that holding w brings them straight up
            player.transform.position = lockpoint.position;
            player.transform.rotation = lockpoint.rotation;
        }
        //only allow straight up and down movement currently
        if (OnLadder && Input.GetKey(KeyCode.W))
        {
            player.GetComponent<PlayerMovement>().enabled = false;

            Debug.Log("Block usual Movement");
            Debug.Log("Climb Up");
            isClimbingDown = false;
            
            Vector3 movement = new Vector3(0, Input.GetAxis("Vertical"), 0);
            player.transform.position += movement * Time.deltaTime * ClimbingSpeed;
        }
        if (OnLadder && Input.GetKey(KeyCode.S))
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            
            Debug.Log("Block usual Movement");
            Debug.Log("Climb Down");
            isClimbingDown = true;
            Vector3 movement = new Vector3(0, Input.GetAxis("Vertical"), 0);
            player.transform.position += movement * Time.deltaTime * ClimbingSpeed;

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (OnLadder == false && player != null)
            {
                PlayerPresent = true;
            }

        }
    }


}