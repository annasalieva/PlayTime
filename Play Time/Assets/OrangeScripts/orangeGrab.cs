using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orangeGrab : MonoBehaviour
{
    [SerializeField] private bool canGrab;
    [SerializeField] private bool isGrabbing;
    [SerializeField] private float grabCheckDistance = 1.5f;
    [SerializeField] private Transform grabbableObject;

    [SerializeField] private GameObject grabJoint;

    private RaycastHit Hit;
    private orangeMovement orangeMove;
    // Start is called before the first frame update
    void Start()
    {
        orangeMove = gameObject.GetComponent<orangeMovement>();
    }

    // Update is called once per frame
    void Update()
    {
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

        //State check
        isGrabbing = orangeMove.fetchGrab();

        
        if((Input.GetKey(KeyCode.Space) && canGrab) || (Input.GetKey(KeyCode.Space) && isGrabbing))
            {
                if(grabbableObject != null)
                {
                    if(grabJoint.GetComponent<FixedJoint>().connectedBody != grabbableObject.gameObject.GetComponent<Rigidbody>())
                    {
                        grabJoint.GetComponent<FixedJoint>().connectedBody = grabbableObject.gameObject.GetComponent<Rigidbody>();
                    }
                }
                
                orangeMove.setGrab(true);

            } else {
                if(grabbableObject != null)
                {
                    grabJoint.GetComponent<FixedJoint>().connectedBody = null;
                    orangeMove.setGrab(false);
                }
            }
    }

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
            float moveSpeed = orangeMove.fetchMoveSpeed();
            float grabSpeedReduction = orangeMove.fetchGrabSpeed();
            
            // Apply the push
            body.velocity = pushDir*(moveSpeed*(grabSpeedReduction*2)); //Testing temporary variable
        }
    }
}